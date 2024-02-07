# build 
FROM node:lts-alpine as build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install 
COPY . .
ARG ENVIRONMENT
RUN npm run build -- --mode $ENVIRONMENT

# production stage
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
