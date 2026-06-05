FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj y restaurar dependencias
COPY helpdesk/backend/HelpdeskAPI/*.csproj ./HelpdeskAPI/
RUN dotnet restore ./HelpdeskAPI/HelpdeskAPI.csproj

# Copiar todo el código fuente
COPY helpdesk/backend/HelpdeskAPI/. ./HelpdeskAPI/

WORKDIR /src/HelpdeskAPI
RUN dotnet publish -c Release -o /app/publish

# Etapa final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HelpdeskAPI.dll"]