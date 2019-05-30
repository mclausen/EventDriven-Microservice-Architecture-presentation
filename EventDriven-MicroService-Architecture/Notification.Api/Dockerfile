FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Notification.Api/Notification.Api.csproj Notification.Api/
RUN dotnet restore Notification.Api/Notification.Api.csproj
COPY . .
WORKDIR /src/Notification.Api
RUN dotnet build Notification.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Notification.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Notification.Api.dll"]