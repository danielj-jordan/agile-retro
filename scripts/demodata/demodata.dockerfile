FROM mcr.microsoft.com/dotnet/core/sdk

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DB_CONNECTIONSTRING=mongodb://localhost:27017
ENV DB_NAME=e2e_test

#
#  install the project source code
#
ADD . /agile-retro

