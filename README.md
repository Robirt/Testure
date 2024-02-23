# Testure

## Sample application for testing Azure Function and Azure Storage

### ProcessQueueMessage Function _(Queue Trigger)_

ProcessQueueMessage Function is responsible for getting item from Azure Storage Account Queue and,
if item is string in valid JSON format, saves the content to the file on Blob Container with directory pattern: `Year/Month/Day/Hour/Minute/{Guid}.json`.

### ListBlobs Function _(Http Trigger)_

ListBlobs Function is responsible for getting names of saved by ProcessQueueMessage Function files in Blob Container
and returns them as simple HTTP response with list of strings.
