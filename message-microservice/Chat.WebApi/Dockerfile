FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Chat.WebApi.csproj", "."]
RUN dotnet restore "./Chat.WebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Chat.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chat.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.WebApi.dll"]