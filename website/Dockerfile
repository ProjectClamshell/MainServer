FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ./ProjectClamshell/ProjectClamshell.csproj ./
RUN dotnet restore

COPY ./ProjectClamshell ./
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

RUN apk add --no-cache krb5-libs

COPY --from=build /publish .

ENTRYPOINT ["dotnet", "ProjectClamshell.dll"]