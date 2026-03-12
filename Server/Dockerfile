FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/ClamShellServer/ClamShellServer.csproj ./
RUN dotnet restore

COPY src/ClamShellServer/ ./
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

RUN apk add --no-cache krb5-libs

COPY --from=build /publish .

EXPOSE 5000
EXPOSE 9000

ENTRYPOINT ["dotnet", "ClamShellServer.dll"]