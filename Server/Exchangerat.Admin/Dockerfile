#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Exchangerat.Admin/Exchangerat.Admin.csproj", "Exchangerat.Admin/"]
COPY ["Exchangerat.Admin.Data/Exchangerat.Admin.Data.csproj", "Exchangerat.Admin.Data/"]
COPY ["Exchangerat/Exchangerat.csproj", "Exchangerat/"]
COPY ["Exchangerat.Admin.Common/Exchangerat.Admin.Common.csproj", "Exchangerat.Admin.Common/"]
COPY ["Exchangerat.Admin.Services/Exchangerat.Admin.Services.csproj", "Exchangerat.Admin.Services/"]
COPY ["Exchangerat.Admin.Models/Exchangerat.Admin.Models.csproj", "Exchangerat.Admin.Models/"]
RUN dotnet restore "Exchangerat.Admin/Exchangerat.Admin.csproj"
COPY . .
WORKDIR "/src/Exchangerat.Admin"
RUN dotnet build "Exchangerat.Admin.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Exchangerat.Admin.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchangerat.Admin.dll"]