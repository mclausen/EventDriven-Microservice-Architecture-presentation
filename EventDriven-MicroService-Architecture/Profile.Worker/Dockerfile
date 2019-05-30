FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Profile.Worker/Profile.Worker.csproj Profile.Worker/
RUN dotnet restore Profile.Worker/Profile.Worker.csproj
COPY . .
WORKDIR /src/Profile.Worker
RUN dotnet build Profile.Worker.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Profile.Worker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Profile.Worker.dll"]