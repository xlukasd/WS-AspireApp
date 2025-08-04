using Azure.Storage.Blobs;

namespace WS_AspireApp.ApiService.Endpoints;

internal static class StoredFilesEndpoints
{
    internal static void AddEndpoints(this WebApplication app)
    {
        app.MapGet("/storedfiles", async (BlobServiceClient blobServiceClient, DummyServiceHttpClient dummyServiceHttpClient) =>
        {
            var dummyData = await dummyServiceHttpClient.GetDummyDataAsync();

            Console.WriteLine($"/storedfiles endpoint. Dummy data retrieved: {dummyData}");

            var containerClient = blobServiceClient.GetBlobContainerClient("files");

            var containerExists = await containerClient.ExistsAsync();

            if (!containerExists)
            {
                return new List<StoredFile>();
            }

            var blobs = new List<StoredFile>();
            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                blobs.Add(new StoredFile(blobItem.Name, blobItem.Properties.ContentLength));
            }

            return blobs;
        })
        .WithName("GetStoredFiles");

        app.MapGet("/buggystoredfiles", async (BlobServiceClient blobServiceClient) =>
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("nonexistingfiles");

            var containerExists = await containerClient.ExistsAsync();

            if (!containerExists)
            {
                return Results.NotFound("Container not found");
            }

            var blobs = new List<StoredFile>();
            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                blobs.Add(new StoredFile(blobItem.Name, blobItem.Properties.ContentLength));
            }

            return Results.Ok(blobs);
        })
        .WithName("GetStoredFilesBuggy");
    }
}
