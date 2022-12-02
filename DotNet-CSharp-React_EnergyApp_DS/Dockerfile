FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5024

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DotNet-CSharp-React_EnergyApp_DS/DotNet-CSharp-React_EnergyApp_DS.csproj", "DotNet-CSharp-React_EnergyApp_DS/"]
RUN dotnet restore "DotNet-CSharp-React_EnergyApp_DS/DotNet-CSharp-React_EnergyApp_DS.csproj"
COPY . .
WORKDIR "/src/DotNet-CSharp-React_EnergyApp_DS"
RUN dotnet build "DotNet-CSharp-React_EnergyApp_DS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNet-CSharp-React_EnergyApp_DS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNet-CSharp-React_EnergyApp_DS.dll"]
