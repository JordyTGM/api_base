# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=84c99db3007b97266f6655e8dd956d78FFFFNRAL \
NEW_RELIC_APP_NAME="RetencionSaludsaBackEnd_Pruebas"

WORKDIR /app
EXPOSE 80
EXPOSE 443
# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG NUGET_USER
ARG NUGET_PAT
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/WebApiRetencionClientes.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["NuGet.Config", "./NuGet.Config"]
RUN sed -i 's|<add key="Username" value=".*" />|<add key="Username" value="'$NUGET_USER'" />|' /src/NuGet.Config
RUN sed -i 's|<add key="ClearTextPassword" value=".*" />|<add key="ClearTextPassword" value="'$NUGET_PAT'" />|' /src/NuGet.Config
RUN dotnet restore "./API/WebApiRetencionClientes.csproj" --configfile /src/NuGet.Config
COPY . .
WORKDIR "/src/API"
RUN dotnet build "./WebApiRetencionClientes.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApiRetencionClientes.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiRetencionClientes.dll"]