FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LeadersWeb/LeadersWeb.csproj", "LeadersWeb/"]
RUN dotnet restore "LeadersWeb/LeadersWeb.csproj"
COPY . .
WORKDIR "/src/LeadersWeb"
RUN dotnet build "LeadersWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LeadersWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeadersWeb.dll"]
