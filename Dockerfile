FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY src/TcpApi/TcpApi.csproj ./
RUN dotnet restore
COPY src/TcpApi/ ./
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "TcpApi.dll"]