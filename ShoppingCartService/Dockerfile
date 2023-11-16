## [ build stage ]  
# Use the official .NET SDK version 7.0 as the base image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container to /src
WORKDIR /src

# Copy only the project file(s) needed for restoring dependencies to the container
COPY ["ShoppingCartService.csproj", "ShoppingCartService/"]

# Restore the dependencies using dotnet restore based on the provided project file
RUN dotnet restore "ShoppingCartService/ShoppingCartService.csproj"

# Change the working directory to the location of the project file
WORKDIR "/src/ShoppingCartService"

# Copy the entire content of the local directory to the container's working directory (/src)
COPY . .

# Build the application using dotnet build, specifying Release configuration and output directory
RUN dotnet build "ShoppingCartService.csproj" -c Release -o /app/build

## [ publish stage ] 
# Create a new build stage named 'publish' based on the 'build' stage
FROM build AS publish

# Publish the application using dotnet publish, specifying Release configuration, and output directory
RUN dotnet publish "ShoppingCartService.csproj" -c Release -o /app/publish 

## [ final stage ]
# Create a new build stage named 'final' based on the 'base' stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS final

# Set the working directory inside the container to /app
WORKDIR /app

# Expose the port and copy the published application from the 'publish' stage to the current stage
EXPOSE 80 
COPY --from=publish /app/publish .

# Set the entry point for the container to run the .NET application
ENTRYPOINT ["dotnet", "ShoppingCartService.dll"]
