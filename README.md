# Instructions

## AWS Access

- in ~/.aws/credentials add:
    ```
    [dogapi]
    aws_access_key_id = 
    aws_secret_access_key = 
    ```

## API Project Organization

Controllers are for the endpoints of the api

Models are the data types, they are broken into three separate tiers:

- App(lication): these types are for supporting the application itself 
- Domain: these types are for specific business logic concerns 
- Infra(structure): these types are used when the application communicates with outside interfaces

## Tests

- I'll get to them I promise 

## DB Container

- run `docker-compose up` from project root to start local db container
- install dotnet-ef with `dotnet tool install --global dotnet-ef` if not installed
- to apply ef-core migrations, run `dotnet ef database update --project src/dog-api.csproj`

## Enviroment Variables + Docker Compose

- when running `docker compose up`, docker will look into the project folder for a .env file for default enviroment variable values

## Using the .Net Core Secret Manager

- secrets stored at `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`
- initialized with `dotnet user-secrets init`
- set a new secret with `dotnet user-secrets set "Movies:ServiceApiKey" "12345"`
- to set a batch of secrets you can pipe json to the set command like so `cat ./input.json | dotnet user-secrets set`

## CI/CD Pipeline

- setup using Github Actions, Docker, and AWS ECR/ECS
- on push to main, github action will trigger the CI/CD process defined in `.github/workflows/cicd.yaml`
    - action checks-out code
    - configures creds for AWS
    - logs into AWS ECR
    - builds, tags, and pushes image to Amazon ECR
    - updates Amazon ECR Task Definition with new image id
    - and finally deploys the new task definition to ECS

# What were you doing?
    - open your one favorite in the web browser
    - need to setup github repo with actions
        - lots of aws variables need to be here for the workflow to deploy correctly
    - setup the ECS task defition 
    - Might need to configure some IAM roles
    - look into hosting a postgres db somewhere (Amazon RDS?)