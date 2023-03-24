#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["csharp_it/csharp_it.csproj", "csharp_it/"]
RUN dotnet restore "csharp_it/csharp_it.csproj"
COPY . .
WORKDIR "/src/csharp_it"
RUN dotnet build "csharp_it.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "csharp_it.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "csharp_it.dll"]
