# ===== base (runtime) =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# copie os .csproj primeiro (cache do restore)
COPY src/OrderService/OrderService.API/OrderService.API.csproj               src/OrderService/OrderService.API/
COPY src/OrderService/OrderService.Application/OrderService.Application.csproj src/OrderService/OrderService.Application/
COPY src/OrderService/OrderService.Domain/OrderService.Domain.csproj         src/OrderService/OrderService.Domain/
COPY src/OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj src/OrderService/OrderService.Infrastructure/
COPY src/Contracts/Ordering.Contracts/Ordering.Contracts.csproj             src/Contracts/Ordering.Contracts/

RUN dotnet restore src/OrderService/OrderService.API/OrderService.API.csproj

# copie o restante do código
COPY . .
WORKDIR /src/src/OrderService/OrderService.API
RUN dotnet build OrderService.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# ===== publish =====
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish OrderService.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ===== final =====
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]