# Usar a imagem do SDK do .NET para construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SmartSchool.WebAPI.csproj", "./"]
RUN dotnet restore "./SmartSchool.WebAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SmartSchool.WebAPI.csproj" -c Release -o /app/build
RUN dotnet publish "SmartSchool.WebAPI.csproj" -c Release -o /app/publish

# Usar a imagem do runtime do .NET para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartSchool.WebAPI.dll"]
