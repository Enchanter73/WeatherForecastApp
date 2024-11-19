# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file
COPY WeatherForecastApp.sln ./

# Copy each project file
COPY Application/Application.csproj ./Application/
COPY Domain/Domain.csproj ./Domain/
COPY Infastructure/Infastructure.csproj ./Infastructure/
COPY WeatherForecastApp/WeatherForecastApp.csproj ./WeatherForecastApp/

# Restore dependencies
RUN dotnet restore "WeatherForecastApp.sln"

# Copy the rest of the source code
COPY . ./

# Build the project and explicitly define a clean output directory
WORKDIR "/src/WeatherForecastApp"
RUN dotnet build "WeatherForecastApp.csproj" -c Release -o /app/build_output

FROM build AS publish
# Publish the project to a separate output directory
RUN dotnet publish "WeatherForecastApp.csproj" -c Release -o /app/publish_output

FROM base AS final
WORKDIR /app
# Copy the final build output
COPY --from=publish /app/publish_output .
ENTRYPOINT ["dotnet", "WeatherForecastApp.dll"]






