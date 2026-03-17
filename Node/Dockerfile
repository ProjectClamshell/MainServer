FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./ClamshellNode.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

RUN apk add --no-cache krb5-libs

COPY --from=build /publish .

ENTRYPOINT ["dotnet", "ClamshellNode.dll"]