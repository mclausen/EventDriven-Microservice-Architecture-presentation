FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Api.Gateway/Api.Gateway.csproj Api.Gateway/
RUN dotnet restore Api.Gateway/Api.Gateway.csproj
COPY . .
WORKDIR /src/Api.Gateway
RUN dotnet build Api.Gateway.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Api.Gateway.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Api.Gateway.dll"]