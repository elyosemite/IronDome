# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copia arquivos da solução e projetos
COPY PublicKeyInfrastructure.sln ./src/
COPY CertificationAuthority/CertificationAuthority.Application/*.csproj ./src/CertificationAuthority/CertificationAuthority.Application/
COPY CertificationAuthority/CertificationAuthority.Domain/*.csproj ./src/CertificationAuthority/CertificationAuthority.Domain/
COPY CertificationAuthority/CertificationAuthority.Infrastructure/*.csproj ./src/CertificationAuthority/CertificationAuthority.Infrastructure/
COPY CertificationAuthority/CertificationAuthority.Presentation/*.csproj ./src/CertificationAuthority/CertificationAuthority.Presentation/
COPY CertificationAuthority/CertificationAuthority.Test/*.csproj ./src/CertificationAuthority/CertificationAuthority.Test/

COPY SharedKernel/*.csproj ./src/SharedKernel/
COPY Observability/*.csproj ./src/Observability/

# Restaura dependências
RUN dotnet restore ./src/CertificationAuthority/CertificationAuthority.Presentation/CertificationAuthority.Presentation.csproj

# Copia todo o código
COPY . ./src

# Publica o projeto ASP.NET Core
RUN dotnet publish ./src/CertificationAuthority/CertificationAuthority.Presentation/CertificationAuthority.Presentation.csproj \
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

#EXPOSE 5000

ENTRYPOINT ["dotnet", "CertificationAuthority.Presentation.dll"]
