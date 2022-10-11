# Instructions

## AWS Access

- in ~/.aws/credentials add:
    ```
    [dogapi]
    aws_access_key_id = 
    aws_secret_access_key = 
    ```

## DB Container

- run `docker-compose up` from project root
- install dotnet-ef with `dotnet tool install --global dotnet-ef` if not installed
- to apply ef-core migrations, run `dotnet ef database update --project src/dog-api.csproj`