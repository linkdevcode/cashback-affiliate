FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY backend/Cashback.sln ./
COPY backend/Directory.Build.props ./
COPY backend/src/Cashback.Domain/Cashback.Domain.csproj backend/src/Cashback.Domain/
COPY backend/src/Cashback.Application/Cashback.Application.csproj backend/src/Cashback.Application/
COPY backend/src/Cashback.Infrastructure/Cashback.Infrastructure.csproj backend/src/Cashback.Infrastructure/
COPY backend/src/Cashback.Persistence/Cashback.Persistence.csproj backend/src/Cashback.Persistence/
COPY backend/src/Cashback.Api/Cashback.Api.csproj backend/src/Cashback.Api/

RUN dotnet restore backend/src/Cashback.Api/Cashback.Api.csproj

COPY backend/src/. backend/src/

RUN dotnet publish backend/src/Cashback.Api/Cashback.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

RUN adduser --disabled-password --gecos "" appuser \
    && chown -R appuser /app
USER appuser

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Cashback.Api.dll"]
