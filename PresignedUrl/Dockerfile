## Use the official .NET SDK image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Use the official .NET SDK image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PresignedUrl/PresignedUrl.csproj", "PresignedUrl/"]
RUN dotnet restore "PresignedUrl/PresignedUrl.csproj"
COPY . .
WORKDIR "/src/PresignedUrl"
RUN dotnet build "PresignedUrl.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PresignedUrl.csproj" -c Release -r linux-x64 -o /app/publish

# Set the base image for the final stage
FROM base AS final
WORKDIR /var/task

CMD ["PresignedUrl::PresignedUrl.LambdaEntryPoint::FunctionHandlerAsync"]
COPY --from=publish /app/publish .