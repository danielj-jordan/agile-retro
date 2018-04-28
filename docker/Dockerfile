

#
#
#  https://hub.docker.com/r/microsoft/dotnet/
from microsoft/dotnet

#
# install node
#
RUN apt-get -qq update
RUN apt-get update && apt-get install -y sudo && rm -rf /var/lib/apt/lists/*
RUN apt-get -qq update
RUN apt-get -qq -y install curl
RUN curl -sL https://deb.nodesource.com/setup_8.x | sudo -E bash -
RUN apt-get install -y nodejs
#RUN npm install -g @angular/cli
#RUN npm install -g webpack

#
# Install Google Chrome
#
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable  main" >> /etc/apt/sources.list.d/google.list'
RUN apt-get update && apt-get install -y google-chrome-beta


#
# install mongo
# https://github.com/dockerfile/mongodb
#
#RUN  apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv 7F0CEB10
#RUN  echo 'deb http://downloads-distro.mongodb.org/repo/ubuntu-upstart dist 10gen' > /etc/apt/sources.list.d/mongodb.list
#RUN  apt-get --allow-unauthenticated update
#RUN  apt-get --allow-unauthenticated install -y mongodb-org
#RUN  rm -rf /var/lib/apt/lists/*
RUN curl -O https://fastdl.mongodb.org/linux/mongodb-linux-x86_64-3.6.3.tgz
RUN tar -zxvf mongodb-linux-x86_64-3.6.3.tgz
RUN mkdir mongodb
RUN cp -R -n mongodb-linux-x86_64-3.6.3/ mongodb



# Define mountable directories.
#VOLUME ["/data/db"]

# Define working directory.
#WORKDIR /data

# Define default command.
#CMD ["mongod"]

# Expose ports for Mongo.
#   - 27017: process
#   - 28017: http
EXPOSE 27017
EXPOSE 28017

#KARMA test runner port
EXPOSE 9876

#Node and .NET core port
EXPOSE 4200
EXPOSE 5000
