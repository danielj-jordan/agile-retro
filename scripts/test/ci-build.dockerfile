

#
#
#  https://hub.docker.com/r/microsoft/dotnet/
# FROM microsoft/dotnet
FROM mcr.microsoft.com/dotnet/core/sdk:2.2

#
# install utilities
#
RUN apt-get -qq update
RUN apt-get -qq -y install curl
RUN apt-get -qq -y install unzip

#
# install node
#
RUN apt-get -qq update
RUN apt-get install -y sudo && rm -rf /var/lib/apt/lists/*
RUN apt-get -qq update

RUN curl -sL https://deb.nodesource.com/setup_10.x | sudo -E bash -
RUN apt-get install -y nodejs

RUN npm -g config set user root
RUN npm install -g webpack --unsafe
RUN npm install -g @angular/cli --unsafe

#
# Install Google Chrome
#
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable  main" >> /etc/apt/sources.list.d/google.list'
RUN apt-get update && apt-get install -y google-chrome-beta


#
# install mongo
#
RUN curl -OsL https://fastdl.mongodb.org/linux/mongodb-linux-x86_64-4.0.10.tgz
RUN tar -zxvf mongodb-linux-x86_64-4.0.10.tgz
RUN mkdir mongodb
RUN cp -R mongodb-linux-x86_64-4.0.10/*  /mongodb
RUN rm -R /mongodb-linux-x86_64-4.0.10


ENV PATH=$PATH:/mongodb/bin
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DB_CONNECTIONSTRING=mongodb://localhost:27017
ENV DB_NAME=e2e_test

#
#  install the project source code
#
ADD . /agile-retro


# Expose ports for Mongo.
#   - 27017: process
#   - 28017: http
#EXPOSE 27017
#EXPOSE 28017

#KARMA test runner port
EXPOSE 9876

#Node and .NET core port
EXPOSE 4200
EXPOSE 5000
