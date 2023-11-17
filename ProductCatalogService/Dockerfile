FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ProductCatalogService.csproj", "ProductCatalogService/"]
RUN dotnet restore "ProductCatalogService/ProductCatalogService.csproj"
WORKDIR "/src/ProductCatalogService"
COPY . .
RUN dotnet build "ProductCatalogService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductCatalogService.csproj" -c Release -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalogService.dll"]
