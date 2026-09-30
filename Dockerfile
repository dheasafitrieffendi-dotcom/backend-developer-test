FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/BackendDeveloperTest.Api/BackendDeveloperTest.Api.csproj", "src/BackendDeveloperTest.Api/"]
RUN dotnet restore "src/BackendDeveloperTest.Api/BackendDeveloperTest.Api.csproj"

COPY . .

WORKDIR "/src/src/BackendDeveloperTest.Api"
RUN dotnet publish "BackendDeveloperTest.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BackendDeveloperTest.Api.dll"]