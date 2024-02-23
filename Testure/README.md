# Testure

## Configuration

Application is created for local testing purposes, so all configuration values are stored in `local.settings.json`.

`local.settings.json` stores three important values:

- **AzureStorageAccountConnectionString** - connection string to Azure Storage Account, by default targets local Azurite instance.
- **QueueName** - name of queue that will be used for actions, default name is `lots`. It will be also created by default on the startup, if not exists.
- **ContainerName** - name of container that will be used for actions, default name is `lots`. It will be also created by default on the startup, if not exists.

## Startup

There are two possible launch profiles:

- **Testure** - default one, instance of Azure Function created on the local environment.
- **Container** - second one, creates Docker Image based on `Dockerfile` and starts Docker container.

## ProcessQueueMessageFunction _(QueueTrigger)_

When new item arrives on queue `ProcessQueueMessageFunction` using mediator pattern sends Request with item string content to RequestHandler.
If request is correct JSON string RequestHandler sends data to `IContainerService` and saves new file in the blob container.
Directory structure is based on simple pattern: `Year/Month/Day/Hour/Minute/{Guid}.json`.

## ListBlobsFunction _(HttpTrigger)_

`ListBlobsFunction` handles HTTP GET requests and using mediator sends getting files names Request to RequestHandler.
RequestHandler gets values from `IContainerService`.
`IContainerService` has simple method that gets blob items using foreach loop and returns list of names.
