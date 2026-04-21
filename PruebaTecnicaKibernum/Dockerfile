# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY PruebaTecnicaKibernum.csproj ./
RUN dotnet restore

# Copiar todo el código
COPY . ./

# Publicar la aplicación
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Crear carpeta para la BD
RUN mkdir /data

# Copiar build
COPY --from=build /out .

# Exponer puerto
EXPOSE 80

# Ejecutar
ENTRYPOINT ["dotnet", "PruebaTecnicaKibernum.dll"]