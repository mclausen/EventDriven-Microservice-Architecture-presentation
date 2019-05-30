FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Friends.Worker/Friends.Worker.csproj Friends.Worker/
RUN dotnet restore Friends.Worker/Friends.Worker.csproj
COPY . .
WORKDIR /src/Friends.Worker
RUN dotnet build Friends.Worker.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Friends.Worker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Friends.Worker.dll"]