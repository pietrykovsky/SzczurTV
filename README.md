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
`http://localhost:8080`


To start streaming run OBS go to File->Settings->Transmission and then: change service for `Custom...`, set Server for `rtmp://localhost:1935/stream`, set key for `hello`.

## Code Formatting

Before making any commit, ensure your C# code is properly formatted to pass the formatting checks with the following command:
`dotnet csharpier .`

