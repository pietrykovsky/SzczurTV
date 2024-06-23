# SzczurTV
A streaming application based on Twitch


## Setup

To get started with the SzczurTV project, follow these steps:

1. Clone the repository to your local machine using the following command:
`git clone <repository-url>`

2. Navigate to the directory containing the `compose.yml`.

3. Copy the `.env.example` file to `.env` and update the environment variables as needed.

4. Build and start the containers with the following command:
`docker compose up --build`
This command builds the images if they do not exist and starts the containers.

5. When you're done working and want to stop the containers, use:
`docker compose down`
This command stops and removes the containers, networks, volumes, and images created by `up`.

## Running the Application

After successfully starting the containers using `docker compose up --build`, you can access the SzczurTV application by opening your web browser and navigating to:
`http://localhost:8082`

## Code Formatting

Before making any commit, ensure your C# code is properly formatted to pass the formatting checks with the following command:
`dotnet csharpier .`

## Deployment

To deploy the SzczurTV application you need a reverse proxy server to route incoming requests to the SzczurTV application.

e.g. Nginx configuration:
```nginx
server {
    server_name your-domain.com;
    server_tokens off;

    location / {
        proxy_pass http://szczurtv:8080;  # Internal app port
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    location /hls {
        proxy_pass http://nginx_rtmp:8080/hls;  # Internal RTMP server for HLS
        proxy_set_header Host $host;
    }
}
```

Keep in mind that the reverse proxy must be connected to the same network as the SzczurTV application.

```yaml
version: '3.9'
services:
nginx:
    restart: always
    image: nginx:latest
    container_name: nginx
    ports:
    - "80:80"
    volumes:
    - ./nginx.conf:/etc/nginx/conf.d/default.conf
    networks:
    - szczurtv_app-network
    restart: always

networks:
szczurtv_app-network:
    external:
    name: szczurtv_app-network
```

Update the `appsettings.json` file with the correct URL for the reverse proxy server:
```json
{
    "DomainUrl": "http://your-domain.com",
    "StreamingUrl": "http://your-domain.com/hls"
}
```

Remember to change the environment variables in the `.env` file to match the production environment.