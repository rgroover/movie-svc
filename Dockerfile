# Stage 1: Build the .NET Core Web API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the .csproj file and restore any dependencies
COPY /src/movie.api/*.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Build the app
RUN dotnet publish -c Release -o /publish

# Stage 2: Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the build artifacts from the previous stage
COPY --from=build /publish .

# Expose the port your application runs on
EXPOSE 8080

# Set the entry point to run the Web API
ENTRYPOINT ["dotnet", "movie-svc.dll"]
