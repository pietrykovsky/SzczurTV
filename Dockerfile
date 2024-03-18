FROM mcr.microsoft.com/dotnet/sdk as build
COPY . ./src
WORKDIR /src
RUN dotnet build "SzczurTv/SzczurTv.sln" -o /app
RUN dotnet publish "SzczurTv/SzczurTv.sln" -o /publish
 
FROM mcr.microsoft.com/dotnet/aspnet as base
COPY --from=build  /publish /app
WORKDIR /app
EXPOSE 8080
ENTRYPOINT ["dotnet", "SzczurApp.dll"]

