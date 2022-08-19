FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TimeSheets/TimeSheets.csproj", "TimeSheets/"]
RUN dotnet restore "TimeSheets/TimeSheets.csproj"
COPY . .
WORKDIR "/src/TimeSheets"
RUN dotnet build "TimeSheets.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeSheets.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeSheets.dll"]
