namespace WS_AspireApp.Web;

public class StoredFilesApiClient(HttpClient httpClient)
{
    public Task<StoredFile[]> GetStoredFiles(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        return GetStoredFilesInternal("/storedfiles", 10, cancellationToken);
    }

    public Task<StoredFile[]> GetNonExistingStoredFiles(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        return GetStoredFilesInternal("/nonexistingstoredfiles", 10, cancellationToken);
    }

    public Task<StoredFile[]> GetBuggyStoredFiles(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        return GetStoredFilesInternal("/buggystoredfiles", 10, cancellationToken);
    }

    public async Task<StoredFile[]> GetStoredFilesInternal(string route, int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<StoredFile>? storedFiles = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<StoredFile>(route, cancellationToken))
        {
            if (storedFiles?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                storedFiles ??= [];
                storedFiles.Add(forecast);
            }
        }

        return storedFiles?.ToArray() ?? [];
    }
}

public record StoredFile(string Name, long? SizeInBytes)
{
    public string Name { get; } = Name;
    public long? SizeInBytes { get; } = SizeInBytes;
}
