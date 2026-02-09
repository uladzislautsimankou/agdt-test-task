FROM mcr.microsoft.com/dotnet/sdk:8.0 AS prepare-restore
WORKDIR /src

# Копируем csproj
COPY src/TestTask.Api/TestTask.Api.csproj TestTask.Api/
COPY src/TestTask.Core/TestTask.Core.csproj TestTask.Core/
COPY src/TestTask.Infrastructure.SQL/TestTask.Infrastructure.SQL.csproj TestTask.Infrastructure.SQL/

RUN dotnet restore TestTask.Api/TestTask.Api.csproj

COPY src/. .

FROM prepare-restore AS build-api
WORKDIR /src/TestTask.Api
RUN dotnet publish TestTask.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ для API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api-final
WORKDIR /app
COPY --from=build-api /app/publish .
ENTRYPOINT ["dotnet", "TestTask.Api.dll"]


# Мигратор
FROM prepare-restore AS build-migrator
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Билдим проект для ef
RUN dotnet build TestTask.Api/TestTask.Api.csproj -c Release

RUN dotnet ef migrations bundle \
    --project TestTask.Infrastructure.SQL/TestTask.Infrastructure.SQL.csproj \
    --startup-project TestTask.Api/TestTask.Api.csproj \
    --context ApplicationDbContext \
    --configuration Release \
    --no-build \
    -o /app/efbundle \
    --force

# Финальный образ для Мигратора
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS migrator-final
WORKDIR /app
COPY --from=build-migrator /app/efbundle .
RUN chmod +x ./efbundle

ENTRYPOINT ["./efbundle"]