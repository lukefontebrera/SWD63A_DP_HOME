# Use the .NET SDK 6.0 image as the base image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory
WORKDIR /WebApp

# Copy the .csproj file and restore dependencies
COPY /WebApp/WebApp.csproj .
RUN dotnet restore "WebApp.csproj"

# Copy the remaining project files and build the application
COPY /WebApp .
RUN dotnet build "WebApp.csproj" -c Release -o /build

# Publish the application to the /publish directory
FROM build-env AS publish
RUN dotnet publish "WebApp.csproj" -c Release -o /publish

# Use an Nginx image as the base image for the runtime stage
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy the published application to the Nginx html directory
COPY --from=publish /publish/wwwroot /usr/local/webapp/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
