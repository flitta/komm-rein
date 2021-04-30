#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["komm-rein.api/komm-rein.api.csproj", "komm-rein.api/"]
COPY ["komm-rein.model/komm-rein.model.csproj", "komm-rein.model/"]
RUN dotnet restore "komm-rein.api/komm-rein.api.csproj"
COPY . .
WORKDIR "/src/komm-rein.api"
RUN dotnet build "komm-rein.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "komm-rein.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "komm-rein.api.dll"]
