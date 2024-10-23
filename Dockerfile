#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProjectTest.csproj", "."]
COPY ["ProjectTest.Application/ProjectTest.Application.csproj", "ProjectTest.Application/"]
COPY ["ProjectTest.Domain/ProjectTest.Domain.csproj", "ProjectTest.Domain/"]
COPY ["ProjectTest.Infrastructure.Data/ProjectTest.Infrastructure.Data.csproj", "ProjectTest.Infrastructure.Data/"]
RUN dotnet restore "./ProjectTest.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ProjectTest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProjectTest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectTest.dll"]