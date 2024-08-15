FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

# copy ALL the projects
COPY Domain/*.csproj ./Domain/
COPY DAL.EF/*.csproj ./DAL.EF/
COPY Base/*.csproj ./Base/
COPY DAL/*.csproj ./DAL/
COPY BLL/*.csproj ./BLL/
COPY DTO/*.csproj ./DTO/
COPY Resources/*.csproj ./Resources/
COPY Test/*.csproj ./Test/
COPY WebApp/*.csproj ./WebApp/

RUN dotnet restore

# copy everything else and build app
# copy all the projects
COPY Domain/. ./Domain/
COPY DAL.EF/. ./DAL.EF/
COPY Base/. ./Base/
COPY DAL/. ./DAL/
COPY BLL/. ./BLL/
COPY DTO/. ./DTO/
COPY Resources/. ./Resources/
COPY Test/. ./Test/
COPY WebApp/. ./WebApp/


# build output files
WORKDIR /app/WebApp
RUN dotnet publish -c Release -o out

# switch to runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
EXPOSE 80
EXPOSE 8080
WORKDIR /app
COPY --from=build /app/WebApp/out ./
ENTRYPOINT ["dotnet", "WebApp.dll"]
