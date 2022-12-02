echo "Stopping and removing the back-end container"

docker stop  dotnet-integration

docker rm  dotnet-integration

echo "Removing the image for the back-end container"
docker image rm dotnet-csharp-react_energyapp_ds-dotnet-integration

echo "Removing the volume for the back-end container"
docker volume rm  dotnet-csharp-react_energyapp_ds_dotnet-integration


echo "Stopping and removing the front-end container"

docker stop react-integration

docker rm react-integration

echo "Removing the image for the front-end container"
docker image rm dotnet-csharp-react_energyapp_ds-react-integration

echo "Stopping and removing the PostgreSQL container"

docker stop  postgres-integration

docker rm  postgres-integration

echo "Removing the image for the back-end container"
docker image rm postgres

echo "Removing the volume for the back-end container"
docker volume rm  dotnet-csharp-react_energyapp_ds_postgres-integration


docker-compose up --build
