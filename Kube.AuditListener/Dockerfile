FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
EXPOSE 5253
WORKDIR /app

# Copy everything else and build
COPY . ./
RUN dotnet restore Kube.AuditListener.sln --verbosity minimal
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Kube.AuditListener.dll"]