FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Exchangerat.Clients/Exchangerat.Clients.csproj", "Exchangerat.Clients/"]
COPY ["Exchangerat/Exchangerat.csproj", "Exchangerat/"]
COPY ["Exchangerat.Clients.Services/Exchangerat.Clients.Services.csproj", "Exchangerat.Clients.Services/"]
COPY ["Exchangerat.Clients.Models/Exchangerat.Clients.Models.csproj", "Exchangerat.Clients.Models/"]
COPY ["Exchangerat.Clients.Common/Exchangerat.Clients.Common.csproj", "Exchangerat.Clients.Common/"]
COPY ["Exchangerat.Clients.Data/Exchangerat.Clients.Data.csproj", "Exchangerat.Clients.Data/"]
RUN dotnet restore "Exchangerat.Clients/Exchangerat.Clients.csproj"

COPY . .
WORKDIR "/src/Exchangerat.Clients"
RUN dotnet build "Exchangerat.Clients.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exchangerat.Clients.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchangerat.Clients.dll"]