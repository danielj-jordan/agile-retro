# The builder from node image
FROM node:alpine as builder

# build-time variables 
# prod|sandbox its value will be come from outside 
# ARG env=prod

RUN apk update && apk add --no-cache make git

# Move our files into directory name "app"
WORKDIR /agile-retro/web
# COPY ./client/package.json package-lock.json  /app/
RUN npm install -g @angular/cli --unsafe
RUN mkdir /app
COPY ./web/client  /app
RUN cd /app && npm install

# Build with $env variable from outside
RUN cd /app && npm run build

# Build a small nginx image with static website
FROM nginx:alpine
RUN mkdir -p /run/nginx
RUN apk add --no-cache nginx-mod-http-lua
RUN rm -rf /usr/share/nginx/html/*
COPY ./web/nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=builder /app/dist /usr/share/nginx/html
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]