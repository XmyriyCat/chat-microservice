FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000:80

env ConnectionStrings:Db_Connection_String "host=postgres_db;port=5432;database=chatDb;username=admin;password=password"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Chat.WebApi/Chat.WebApi.csproj", "Chat.WebApi/"]
COPY ["Chat.Application/Chat.Application.csproj", "Chat.Application/"]
COPY ["Chat.Data/Chat.Data.csproj", "Chat.Data/"]
	
RUN dotnet restore "./Chat.WebApi/Chat.WebApi.csproj"
COPY . .
WORKDIR "/src/Chat.WebApi/."
RUN dotnet build "Chat.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chat.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.WebApi.dll"]