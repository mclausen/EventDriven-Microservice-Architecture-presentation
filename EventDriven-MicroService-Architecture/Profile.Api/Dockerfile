FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Profile.Api/Profile.Api.csproj Profile.Api/
RUN dotnet restore Profile.Api/Profile.Api.csproj
COPY . .
WORKDIR /src/Profile.Api
RUN dotnet build Profile.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Profile.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Profile.Api.dll"]