#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Exchangerat.Requests.Gateway/Exchangerat.Requests.Gateway.csproj", "Exchangerat.Requests.Gateway/"]
COPY ["Exchangerat.Requests.Gateway.Common/Exchangerat.Requests.Gateway.Common.csproj", "Exchangerat.Requests.Gateway.Common/"]
COPY ["Exchangerat.Requests.Gateway.Services/Exchangerat.Requests.Gateway.Services.csproj", "Exchangerat.Requests.Gateway.Services/"]
COPY ["Exchangerat.Requests.Gateway.Models/Exchangerat.Requests.Gateway.Models.csproj", "Exchangerat.Requests.Gateway.Models/"]
COPY ["Exchangerat/Exchangerat.csproj", "Exchangerat/"]
RUN dotnet restore "Exchangerat.Requests.Gateway/Exchangerat.Requests.Gateway.csproj"
COPY . .
WORKDIR "/src/Exchangerat.Requests.Gateway"
RUN dotnet build "Exchangerat.Requests.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exchangerat.Requests.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchangerat.Requests.Gateway.dll"]