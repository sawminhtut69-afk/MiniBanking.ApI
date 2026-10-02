FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["MiniBanking.API.csproj", "."]
RUN dotnet restore "MiniBanking.API.csproj"

COPY . .
RUN dotnet publish "MiniBanking.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "MiniBanking.API.dll"]