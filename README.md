# SzczurTV
A streaming application based on Twitch


## Setup

To get started with the SzczurTV project, follow these steps:

1. Clone the repository to your local machine using the following command:
`git clone <repository-url>`

2. Navigate to the directory containing the `docker-compose.yml` and `Dockerfile`.

3. Build and start the containers with the following command:
`docker-compose up --build`
This command builds the images if they do not exist and starts the containers.

4. When you're done working and want to stop the containers, use:
`docker-compose down`
This command stops and removes the containers, networks, volumes, and images created by `up`.

## Running the Application

After successfully starting the containers using `docker-compose up --build`, you can access the SzczurTV application by opening your web browser and navigating to:
`http://localhost:8080`

## Code Formatting

Before making any commit, ensure your C# code is properly formatted to pass the formatting checks with the following command:
`dotnet csharpier .`

