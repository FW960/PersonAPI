FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AuthorizationAPI/AuthorizationAPI.csproj", "AuthorizationAPI/"]
RUN dotnet restore "AuthorizationAPI/AuthorizationAPI.csproj"
COPY . .
WORKDIR "/src/AuthorizationAPI"
RUN dotnet build "AuthorizationAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthorizationAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizationAPI.dll"]
