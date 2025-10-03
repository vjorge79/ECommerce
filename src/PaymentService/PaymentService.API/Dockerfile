# ===== base (runtime) =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080 

# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# copie os .csproj primeiro (cache do restore)
COPY src/PaymentService/PaymentService.API/PaymentService.API.csproj               src/PaymentService/PaymentService.API/
COPY src/PaymentService/PaymentService.Application/PaymentService.Application.csproj src/PaymentService/PaymentService.Application/
COPY src/PaymentService/PaymentService.Domain/PaymentService.Domain.csproj         src/PaymentService/PaymentService.Domain/
COPY src/PaymentService/PaymentService.Infrastructure/PaymentService.Infrastructure.csproj src/PaymentService/PaymentService.Infrastructure/
COPY src/Contracts/Ordering.Contracts/Ordering.Contracts.csproj             src/Contracts/Ordering.Contracts/

RUN dotnet restore src/PaymentService/PaymentService.API/PaymentService.API.csproj

# copie o restante do código
COPY . .
WORKDIR /src/src/PaymentService/PaymentService.API
RUN dotnet build PaymentService.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# ===== publish =====
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish PaymentService.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ===== final =====
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentService.API.dll"]