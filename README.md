This repository contains three parts of the FloodCitiSense project:
 * the asp.net based main API server
 * an Angular based web app referred to as admin-app,
 * the Xamarin based mobile app. 
 
 The other consituent parts developed by IIASA are a React JS based [web app]() aimed at the general public, and an accompanying http proxy.

# Deployment
The admin-app and api are intended to be deployed inside docker images, and the Makefile contains the necessary commands to build and push to the registry.

`make build-admin-app` or `make build-api`


The docker-compose file also contains the routing instructions (in the form of traefik labels) for the  stack when fully deployed:
* app.floodcitisense.eu - the main public site
* app.floodcitisense.eu/graphql - the proxy between the public site and api (and the seperate rainfall API)
* api.floodicitsense.eu - main api, called by the mobile app directly and the proxy
* admin.floodcitisense.eu - the admin app

Alternative subdomains are also provided for convenience: brussels.floodcitisense.eu, birmingham.floodcitisense.eu and rotterdam.floodcitisense.eu all point to the public app.


# Android 

**Build status**

[![Build status](https://build.appcenter.ms/v0.1/apps/eb2146a1-b63a-409a-9fe5-1c4e426d1ad5/branches/master/badge)](https://appcenter.ms)

# IOS 
**Build status**

[![Build status](https://build.appcenter.ms/v0.1/apps/509b3bd8-e2a8-4519-9b13-5484d43b46e0/branches/master/badge)](https://appcenter.ms)

