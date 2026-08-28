FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PokeAPIMicroDex/PokeAPIMicroDex.csproj", "PokeAPIMicroDex/"]
COPY ["../DNBigHelpfulLib/DNBigHelpfulLib/DNBigHelpfulLib.csproj", "../DNBigHelpfulLib/DNBigHelpfulLib/"]
COPY ["PokeAPIAgent/PokeAPIAgent.csproj", "PokeAPIAgent/"]
RUN dotnet restore "PokeAPIMicroDex/PokeAPIMicroDex.csproj"
COPY . .
WORKDIR "/src/PokeAPIMicroDex"
RUN dotnet build "./PokeAPIMicroDex.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PokeAPIMicroDex.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokeAPIMicroDex.dll"]
