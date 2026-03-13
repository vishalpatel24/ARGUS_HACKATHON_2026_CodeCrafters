# .NET 9 API Dockerfile - build context must be the solution root
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all required project files first for layer caching
COPY CodeCrafters.BackEnd/CodeCrafters.Api/CodeCrafters.Api.csproj CodeCrafters.BackEnd/CodeCrafters.Api/
COPY CodeCrafters.BackEnd/CodeCrafters.Application/CodeCrafters.Application.csproj CodeCrafters.BackEnd/CodeCrafters.Application/
COPY CodeCrafters.BackEnd/CodeCrafters.Infrastructure/CodeCrafters.Infrastructure.csproj CodeCrafters.BackEnd/CodeCrafters.Infrastructure/
COPY CodeCrafters.ServiceDefaults/CodeCrafters.ServiceDefaults.csproj CodeCrafters.ServiceDefaults/

# Restore dependencies
RUN dotnet restore CodeCrafters.BackEnd/CodeCrafters.Api/CodeCrafters.Api.csproj

# Copy all source files
COPY CodeCrafters.BackEnd/ CodeCrafters.BackEnd/
COPY CodeCrafters.ServiceDefaults/ CodeCrafters.ServiceDefaults/

# Publish
RUN dotnet publish CodeCrafters.BackEnd/CodeCrafters.Api/CodeCrafters.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CodeCrafters.Api.dll"]