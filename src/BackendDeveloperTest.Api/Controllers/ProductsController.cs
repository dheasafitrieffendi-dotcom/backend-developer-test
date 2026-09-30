using BackendDeveloperTest.Api.Data;
using BackendDeveloperTest.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDeveloperTest.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BackendDeveloperTest.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] int limit = 10,
        [FromQuery] int page = 1)
    {
        if (limit < 1)
            limit = 10;

        if (page < 1)
            page = 1;

        IQueryable<Product> query = _context.Products
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                EF.Functions.ILike(p.Title, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p =>
                p.Category == category);
        }

        var total = await query.CountAsync();

        var products = await query
            .OrderBy(p => p.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return Ok(new
        {
            data = products,
            meta = new
            {
                page,
                limit,
                total,
                total_pages = (int)Math.Ceiling(total / (double)limit)
            }
        });
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        return Ok(product);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductRequest request)
    {
        var product = new Product
        {
            Title = request.Title,
            Price = request.Price,
            Description = request.Description ?? string.Empty,
            Category = request.Category,
            Images = request.Images,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            CreatedById = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.Id },
            product
        );
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(
        int id,
        [FromBody] UpdateProductRequest request)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        product.Title = request.Title;
        product.Price = request.Price;
        product.Description = request.Description ?? string.Empty;
        product.Category = request.Category;
        product.Images = request.Images;
        
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy =
            User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        product.UpdatedById =
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Product deleted successfully"
        });
    }
}