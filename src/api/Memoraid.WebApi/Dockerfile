FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Memoraid.WebApi/Memoraid.WebApi.csproj", "Memoraid.WebApi/"]
RUN dotnet restore "Memoraid.WebApi/Memoraid.WebApi.csproj"
COPY . .
RUN dotnet build "Memoraid.WebApi/Memoraid.WebApi.csproj" -c Release -o /app/build
RUN dotnet test "Memoraid.Tests.Unit/Memoraid.Tests.Unit.csproj"
RUN dotnet publish "Memoraid.WebApi/Memoraid.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /app/publish .

# Change user to non-root
RUN adduser --disabled-password --gecos '' appuser
USER appuser

ENTRYPOINT ["dotnet", "DockerCourseApi.dll"]