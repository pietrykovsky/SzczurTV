FROM mcr.microsoft.com/dotnet/sdk:latest as build

# Copy the entire SzczurTv solution directory
COPY SzczurTv /SzczurTv

WORKDIR /SzczurTv

# Restore NuGet packages
RUN dotnet restore

# Build the solution
RUN dotnet publish -c Debug -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:latest as runtime
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80

ENTRYPOINT ["dotnet", "SzczurApp.dll"]
