FROM node:22 as build

WORKDIR /app
COPY package*.json ./

RUN npm install
RUN npm install -g @angular/cli

COPY . .

RUN ng build --configuration container

FROM nginx:latest

COPY --from=build app/dist/angular-frontend/browser /usr/share/nginx/html

EXPOSE 80
