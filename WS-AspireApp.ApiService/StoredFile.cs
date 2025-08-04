namespace WS_AspireApp.ApiService;

public class StoredFile
{ 
    public StoredFile(string name, long? sizeInBytes)
    {
        Name = name;
        SizeInBytes = sizeInBytes;
    }

    public string Name { get; }
    public long? SizeInBytes { get; }
}
