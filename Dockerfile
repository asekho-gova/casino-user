# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/Casino.User.Api/Casino.User.Api.csproj", "src/Casino.User.Api/"]
RUN dotnet restore "src/Casino.User.Api/Casino.User.Api.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/src/Casino.User.Api"

# Build the application
RUN dotnet build "Casino.User.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Casino.User.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose the port the app will run on
EXPOSE 8080

# Set the environment variable for ASP.NET to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Run the application
ENTRYPOINT ["dotnet", "Casino.User.Api.dll"]
