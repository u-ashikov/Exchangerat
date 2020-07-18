#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Exchangerat.Requests/Exchangerat.Requests.csproj", "Exchangerat.Requests/"]
COPY ["Exchangerat.Requests.Data/Exchangerat.Requests.Data.csproj", "Exchangerat.Requests.Data/"]
COPY ["Exchangerat.Requests.Common/Exchangerat.Requests.Common.csproj", "Exchangerat.Requests.Common/"]
COPY ["Exchangerat/Exchangerat.csproj", "Exchangerat/"]
COPY ["Exchangerat.Requests.Services/Exchangerat.Requests.Services.csproj", "Exchangerat.Requests.Services/"]
COPY ["Exchangerat.Requests.Models/Exchangerat.Requests.Models.csproj", "Exchangerat.Requests.Models/"]
RUN dotnet restore "Exchangerat.Requests/Exchangerat.Requests.csproj"
COPY . .
WORKDIR "/src/Exchangerat.Requests"
RUN dotnet build "Exchangerat.Requests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exchangerat.Requests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchangerat.Requests.dll"]