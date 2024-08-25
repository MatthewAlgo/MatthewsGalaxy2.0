# Stage 1: Build the application
FROM node:22-bullseye AS build

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application source code
COPY . .

# Build the application
RUN npm run build

# Stage 2: Setup NGINX for serving the app
FROM ubuntu:22.04

# Install NGINX and clean up temporary files
RUN apt-get update && \
    apt-get install -y nginx && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

# Copy build output from the previous stage
COPY --from=build /app/dist/matthewsgalaxy.client/browser /usr/share/nginx/html

# Copy custom NGINX configuration file
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Remove the default site configuration
RUN rm /etc/nginx/sites-enabled/default

# Adjust NGINX user permissions for security
RUN chown -R www-data:www-data /usr/share/nginx/html

# Expose the port NGINX will listen on
EXPOSE 80

# Environment variable to specify NGINX log level
ENV NGINX_LOG_LEVEL=info

# Start NGINX in the foreground
CMD ["nginx", "-g", "daemon off;"]

