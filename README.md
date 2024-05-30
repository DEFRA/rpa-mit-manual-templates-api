# rpa-mit-manual-templates-api

rpa-mit-manual-templates-api Dotnet service.

## Prerequisites

- Docker
- Docker Compose

Optional:

- Kubernetes
- Helm
- Access to an instance of an [Azure Service Bus](https://docs.microsoft.com/en-us/azure/service-bus-messaging/)

## Environment variables

The following environment variables are required by the application container. Values for development are set in the Docker Compose configuration. Default values for production-like deployments are set in the Helm chart and may be overridden by build and release pipelines.

| Name                         | Description                     | Required | Default | Valid | Notes                                                                                                                             |
| ---------------------------- | ------------------------------- | -------- | ------- | ----- | --------------------------------------------------------------------------------------------------------------------------------- |
| APPINSIGHTS_CONNECTIONSTRING | App Insights connection string  | no       |         |       | will log to Azure Application Insights if set in local development. During deployment to AKS it is automatically set by platform. |
| APPINSIGHTS_CLOUDROLE        | Role used for filtering metrics | no       |         |       | Set to `rpa-mit-manual-templates-api-local` in docker compose files                                                                |

## Test structure

The tests have been structured into subfolders of `./test` as per the [Microservice test approach and repository structure](https://eaflood.atlassian.net/wiki/spaces/FPS/pages/1845396477/Microservice+test+approach+and+repository+structure)

### Running tests

A convenience script is provided to run automated tests in a containerised environment. This will rebuild images before running tests via docker-compose, using a combination of `docker-compose.yaml` and `docker-compose.test.yaml`.

Examples:

```
# Run all tests
scripts/test

# Run tests with file watch
scripts/test -w
```

### docker-compose.test.yaml

This file runs all tests and exits the container. If any tests fails the error will be output. Use the docker-compose `-p` flag to avoid conflicting with a running app instance:

`docker-compose -p rpa-mit-manual-templates-api-test -f docker-compose.yaml -f docker-compose.test.yaml up`

### docker-compose.test.watch.yaml

This file is intended to be an override file for `docker-compose.test.yaml`. The container will not exit following test run, instead it will watch for code changes in the application or tests and rerun on occurrence.

`docker-compose -p rpa-mit-manual-templates-api-test -f docker-compose.yaml -f docker-compose.test.watch.yaml up`

## Running the application

The application is designed to run in containerised environments, using Docker Compose in development and Kubernetes in production.

- A Helm chart is provided for production deployments to Kubernetes.

### Build container image

Container images are built using Docker Compose, with the same images used to run the service with either Docker Compose or Kubernetes.

By default, the start script will build (or rebuild) images so there will rarely be a need to build images manually. However, this can be achieved through the Docker Compose [build](https://docs.docker.com/compose/reference/build/) command:

```
# Build container images
docker-compose build
```

### Start and stop the service

Use Docker Compose to run service locally.

```
# Start the service in development
docker-compose up
```

### docker-compose.override.yaml

The default `docker-compose.yaml` and `docker-compose.override.yaml` provide the following features to aid local development:

- map port 3007 from the host to the app container
- bind-mount application code into the app container
- run the application behind a file watcher, automatically reloading the app on change
- run a database and message queue alongside the application

Additional Docker Compose files are provided for scenarios such as linking to other running services and running automated tests.

### Deploy to Kubernetes

For production deployments, a helm chart is included in the `.\helm` folder.

#### Probes

The service has both an Http readiness probe and an Http liveness probe configured to receive at the below end points.

Readiness: `/healthy`
Liveness: `/healthz`

## CI & CD Pipeline

This service uses the [ADP Common Pipelines](https://github.com/DEFRA/adp-pipeline-common) for Builds and Deployments.

### AppConfig - KeyVault References

If the application uses `keyvault references` in `appConfig.env.yaml`, please make sure the variable to be added to keyvault is created in ADO Library variable groups and the reference for the variable groups and variables are provided in `build.yaml` like below.

```
variableGroups:
    - rpa-mit-manual-templates-api-snd1
    - rpa-mit-manual-templates-api-snd2
    - rpa-mit-manual-templates-api-snd3
variables:
    - rpa-mit-manual-templates-api-APPLICATION-SECRET
```

## Licence

THIS INFORMATION IS LICENSED UNDER THE CONDITIONS OF THE OPEN GOVERNMENT LICENCE found at:

<http://www.nationalarchives.gov.uk/doc/open-government-licence/version/3>

The following attribution statement MUST be cited in your products and applications when using this information.

> Contains public sector information licensed under the Open Government license v3

### About the licence

The Open Government Licence (OGL) was developed by the Controller of Her Majesty's Stationery Office (HMSO) to enable information providers in the public sector to license the use and re-use of their information under a common open licence.

It is designed to encourage use and re-use of information freely and flexibly, with only a few conditions.
