FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ContractsAPI/ContractsAPI.csproj", "ContractsAPI/"]
RUN dotnet restore "ContractsAPI/ContractsAPI.csproj"
COPY . .
WORKDIR "/src/ContractsAPI"
RUN dotnet build "ContractsAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContractsAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContractsAPI.dll"]
