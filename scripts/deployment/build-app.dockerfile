FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /agile-retro/app

# Copy source and restore 
ADD ./app /agile-retro/app
ADD ./Retrospective.Data /agile-retro/Retrospective.Data 
ADD ./Retrospective.Data.Model /agile-retro/Retrospective.Data.Model
ADD ./Retrospective.Domain.Model /agile-retro/Retrospective.Domain.Model
ADD ./Retrospective.Domain /agile-retro/Retrospective.Domain 

RUN dotnet restore

# build
#RUN dotnet publish -c Release -o out -r alpine-x64
RUN dotnet publish -c Release -o out

# Build runtime image
#FROM nginx:alpine
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /agile-retro/app/out .
ENTRYPOINT ["dotnet", "app.dll"]
EXPOSE 5000
