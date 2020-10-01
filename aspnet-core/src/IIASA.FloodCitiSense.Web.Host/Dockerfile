FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/IIASA.FloodCitiSense.Web.Host/IIASA.FloodCitiSense.Web.Host.csproj", "src/IIASA.FloodCitiSense.Web.Host/"]
COPY ["src/IIASA.FloodCitiSense.Web.Core/IIASA.FloodCitiSense.Web.Core.csproj", "src/IIASA.FloodCitiSense.Web.Core/"]
COPY ["src/IIASA.FloodCitiSense.Application/IIASA.FloodCitiSense.Application.csproj", "src/IIASA.FloodCitiSense.Application/"]
COPY ["src/IIASA.FloodCitiSense.Application.Shared/IIASA.FloodCitiSense.Application.Shared.csproj", "src/IIASA.FloodCitiSense.Application.Shared/"]
COPY ["src/IIASA.FloodCitiSense.Core.Shared/IIASA.FloodCitiSense.Core.Shared.csproj", "src/IIASA.FloodCitiSense.Core.Shared/"]
COPY ["src/IIASA.FloodCitiSense.Core/IIASA.FloodCitiSense.Core.csproj", "src/IIASA.FloodCitiSense.Core/"]
COPY ["src/IIASA.FloodCitiSense.EntityFrameworkCore/IIASA.FloodCitiSense.EntityFrameworkCore.csproj", "src/IIASA.FloodCitiSense.EntityFrameworkCore/"]
RUN dotnet restore "src/IIASA.FloodCitiSense.Web.Host/IIASA.FloodCitiSense.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/IIASA.FloodCitiSense.Web.Host"
RUN dotnet build "IIASA.FloodCitiSense.Web.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IIASA.FloodCitiSense.Web.Host.csproj" -c Release -o /app

FROM base AS final
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IIASA.FloodCitiSense.Web.Host.dll"]