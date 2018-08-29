FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY MarsToday.Web/MarsToday.Web.csproj MarsToday.Web/
RUN dotnet restore MarsToday.Web/MarsToday.Web.csproj
COPY . .
WORKDIR /src/MarsToday.Web
RUN dotnet build MarsToday.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MarsToday.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MarsToday.Web.dll"]
