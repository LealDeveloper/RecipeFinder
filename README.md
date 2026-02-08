# RecipeFinder API

![CI](https://github.com/LealDeveloper/RecipeFinder/actions/workflows/ci.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-10.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

A **.NET 10 Web API** built with **Clean Architecture** to manage recipes and ingredients.

## Features

- Create recipes with ingredients
- Search recipes by ingredients
- PostgreSQL + EF Core
- FluentValidation
- In-memory caching
- Swagger documentation
- CI + CodeQL Security Scan

## Getting Started

```bash
dotnet restore
dotnet ef database update --project src/RecipeFinder.Infrastructure --startup-project src/RecipeFinder.API
dotnet run --project src/RecipeFinder.API
