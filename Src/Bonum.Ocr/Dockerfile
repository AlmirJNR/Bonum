FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["Bonum.Ocr/Bonum.Ocr.csproj", "Bonum.Ocr/"]
COPY ["Bonum.Shared/Bonum.Shared.csproj", "Bonum.Shared/"]
COPY ["Bonum.Contracts/Bonum.Contracts.csproj", "Bonum.Contracts/"]
RUN dotnet restore "Bonum.Ocr/Bonum.Ocr.csproj"
COPY . .
WORKDIR "/src/Bonum.Ocr"
RUN dotnet build "Bonum.Ocr.csproj" -c Release -o /app/build
RUN dotnet publish "Bonum.Ocr.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine
RUN apk add tesseract-ocr tesseract-ocr-data-eng tesseract-ocr-data-por
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bonum.Ocr.dll"]