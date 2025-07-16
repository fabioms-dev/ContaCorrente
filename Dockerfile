# Copia os arquivos do projeto e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante dos arquivos e publica em modo Release
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados da etapa de build
COPY --from=build /app/out ./

# Define o ponto de entrada
ENTRYPOINT ["dotnet", "ContaCorrente.WebApi.dll"]