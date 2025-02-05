# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copia arquivos da solução e projetos
COPY /src/PublicKeyInfrastructure.sln ./src/
COPY src/CertificationAuthority/CertificationAuthority.Application/ ./src/CertificationAuthority/CertificationAuthority.Application/
COPY src/CertificationAuthority/CertificationAuthority.Domain/ ./src/CertificationAuthority/CertificationAuthority.Domain/
COPY src/CertificationAuthority/CertificationAuthority.Infrastructure/ ./src/CertificationAuthority/CertificationAuthority.Infrastructure/
COPY src/CertificationAuthority/CertificationAuthority.Presentation/ ./src/CertificationAuthority/CertificationAuthority.Presentation/
COPY src/CertificationAuthority/CertificationAuthority.Test/ ./src/CertificationAuthority/CertificationAuthority.Test/

COPY src/SharedKernel/*.csproj ./src/SharedKernel/
COPY src/Observability/*.csproj ./src/Observability/

# Restaura dependências
RUN dotnet restore ./src/CertificationAuthority/CertificationAuthority.Presentation/CertificationAuthority.Presentation.csproj

# Copia todo o código
COPY . .

# Publica o projeto ASP.NET Core
RUN dotnet publish src/CertificationAuthority/CertificationAuthority.Presentation/CertificationAuthority.Presentation.csproj \
	-c Release \
	-o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    curl \
    gosu \
    krb5-user \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:5000

COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "CertificationAuthority.Presentation.dll"]
