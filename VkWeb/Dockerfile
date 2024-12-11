#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

    WORKDIR /app
    EXPOSE 80
    EXPOSE 443

    FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
    WORKDIR /src
    COPY ["VkWeb/VkWeb.csproj", "VkWeb/"]
    COPY ["VkCore/VkCore.csproj", "VkCore/"]
    COPY ["VkInfrastructure/VkInfrastructure.csproj", "VkInfrastructure/"]
    RUN dotnet restore "VkWeb/VkWeb.csproj"
    COPY . .
    WORKDIR "/src/VkWeb"
    RUN apt-get update && \
        apt-get install -y curl software-properties-common && \
        curl -sL https://deb.nodesource.com/setup_12.x | bash - && \
        apt-get install -y nodejs  && \
        npm install -g @angular/cli && \
		dotnet build "VkWeb.csproj" -c Release -o /app/build

    FROM build AS publish
    RUN dotnet publish "VkWeb.csproj" -c Release -o /app/publish

    FROM base AS final
    WORKDIR /app
    COPY --from=publish /app/publish .
    ENTRYPOINT ["dotnet", "VkWeb.dll"]