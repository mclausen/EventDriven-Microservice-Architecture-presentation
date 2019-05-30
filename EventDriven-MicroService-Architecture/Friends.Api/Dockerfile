FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Friends.Api/Friends.Api.csproj Friends.Api/
RUN dotnet restore Friends.Api/Friends.Api.csproj
COPY . .
WORKDIR /src/Friends.Api
RUN dotnet build Friends.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Friends.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Friends.Api.dll"]