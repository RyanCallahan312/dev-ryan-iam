#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["dev-ryan-iam.csproj", ""]
RUN dotnet restore "./dev-ryan-iam.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "dev-ryan-iam.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dev-ryan-iam.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dev-ryan-iam.dll"]