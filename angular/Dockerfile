#############
### build ###
#############

# base image
FROM node:12.2.0 as build

# set working directory
WORKDIR /app

# add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

# install and cache app dependencies
COPY package.json /app/package.json
COPY nginx.conf /app/nginx.conf
RUN npm install
RUN npm install -g @angular/cli@7.3.9

# add app
COPY . /app

# generate build
RUN ng build --output-path=dist --env=prod

############
### prod ###
############

# base image
FROM nginx:1.16.0-alpine

# copy artifact build from the 'build environment'
COPY --from=build /app/dist /usr/share/nginx/html
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf
# expose port 80
EXPOSE 80

# run nginx
CMD ["nginx", "-g", "daemon off;"]
