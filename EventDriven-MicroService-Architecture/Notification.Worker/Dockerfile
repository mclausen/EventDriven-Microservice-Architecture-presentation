FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Notification.Worker/Notification.Worker.csproj Notification.Worker/
RUN dotnet restore Notification.Worker/Notification.Worker.csproj
COPY . .
WORKDIR /src/Notification.Worker
RUN dotnet build Notification.Worker.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Notification.Worker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Notification.Worker.dll"]