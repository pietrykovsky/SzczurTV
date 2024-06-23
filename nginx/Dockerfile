FROM tiangolo/nginx-rtmp:latest

# Create the HLS directory and set permissions
RUN mkdir -p /tmp/hls && \
    chmod -R 755 /tmp/hls

# Copy the Nginx configuration file
COPY nginx.conf /etc/nginx/nginx.conf

# Expose necessary ports
EXPOSE 1935
EXPOSE 8080
