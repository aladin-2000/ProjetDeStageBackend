# Utilisez l'image SDK .NET Core officielle en tant qu'environnement de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Définissez le répertoire de travail dans le conteneur
WORKDIR /home/app

COPY *.csproj .

RUN dotnet restore

COPY . .

RUN dotnet publish -c release -o /app

# Image de runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Définissez le répertoire de travail dans le conteneur de runtime
WORKDIR /app

# Copiez l'application publiée depuis l'environnement de build
COPY --from=build /app .

# Spécifiez le point d'entrée pour le conteneur
ENTRYPOINT ["dotnet", "backEnd.dll"]
