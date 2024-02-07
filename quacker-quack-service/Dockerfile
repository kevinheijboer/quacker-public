FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5021

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["QuackService.Api/QuackService.Api.csproj", ""]
RUN dotnet restore 
COPY . .
WORKDIR "/src/QuackService.Api/."
RUN dotnet build "QuackService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuackService.Api.csproj" -c Release -o /app/publish

FROM base AS final
ARG ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=$ENVIRONMENT
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuackService.Api.dll"]