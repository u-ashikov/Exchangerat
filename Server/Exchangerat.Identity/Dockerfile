FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Exchangerat.Identity/Exchangerat.Identity.csproj", "Exchangerat.Identity/"]
COPY ["Exchangerat.Identity.Data/Exchangerat.Identity.Data.csproj", "Exchangerat.Identity.Data/"]
COPY ["Exchangerat.Identity.Services/Exchangerat.Identity.Services.csproj", "Exchangerat.Identity.Services/"]
COPY ["Exchangerat.Identity.Models/Exchangerat.Identity.Models.csproj", "Exchangerat.Identity.Models/"]
COPY ["Exchangerat.Identity.Common/Exchangerat.Identity.Common.csproj", "Exchangerat.Identity.Common/"]
COPY ["Exchangerat/Exchangerat.csproj", "Exchangerat/"]
RUN dotnet restore "Exchangerat.Identity/Exchangerat.Identity.csproj"
COPY . .
WORKDIR "/src/Exchangerat.Identity"
RUN dotnet build "Exchangerat.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exchangerat.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchangerat.Identity.dll"]