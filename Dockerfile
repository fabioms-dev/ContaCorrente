FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 80
EXPOSE 443

# Imagem para construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ContaCorrente/ContaCorrente.WebApi.csproj", "ContaCorrente/"]
COPY ["Application/ContaCorrente.Application.csproj", "Application/"]
COPY ["ContaCorrente.Infrastructure/ContaCorrente.Infrastructure.csproj", "ContaCorrente.Infrastructure/"]
COPY ["Domain/ContaCorrente.Domain.csproj", "Domain/"]
COPY ["ContaCorrente.Testes/ContaCorrente.Testes.csproj", "ContaCorrente.Testes/"]
RUN dotnet restore "./ContaCorrente/ContaCorrente.WebApi.csproj"

COPY . .
WORKDIR "/src/."
RUN dotnet build "./ContaCorrente/ContaCorrente.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ContaCorrente/ContaCorrente.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final com a aplicação publicada
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContaCorrente.WebApi.dll"]