FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Quiz.Api/Quiz.Api.csproj", "src/Quiz.Api/"]
COPY ["src/Quiz.Application/Quiz.Application.csproj", "src/Quiz.Application/"]
COPY ["src/Quiz.Core/Quiz.Core.csproj", "src/Quiz.Core/"]
COPY ["src/Quiz.Persistence/Quiz.Persistence.csproj", "src/Quiz.Persistence/"]
RUN dotnet restore "src/Quiz.Api/Quiz.Api.csproj"
COPY . .
WORKDIR "/src/src/Quiz.Api"
RUN dotnet build "Quiz.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Quiz.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quiz.Api.dll"]
