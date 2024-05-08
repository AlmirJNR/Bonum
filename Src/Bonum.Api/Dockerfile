FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["Bonum.Api/Bonum.Api.csproj", "Bonum.Api/"]
COPY ["Bonum.Shared/Bonum.Shared.csproj", "Bonum.Shared/"]
COPY ["Bonum.Contracts/Bonum.Contracts.csproj", "Bonum.Contracts/"]
RUN dotnet restore "Bonum.Api/Bonum.Api.csproj"
COPY . .
WORKDIR "/src/Bonum.Api"
RUN dotnet build "Bonum.Api.csproj" -c Release -o /app/build
RUN dotnet publish "Bonum.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bonum.Api.dll"]