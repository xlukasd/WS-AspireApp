using System.Collections;

namespace WS_AspireApp.ApiService;

public class Helpers
{
    public static void WriteAllEnvironmentVariables()
    {
        string fileName = @"C:\Users\lukas.durovsky\Desktop\temp\_logs\env_vars.txt";
        using (StreamWriter writer = new StreamWriter(fileName, append: false))
        {
            foreach (var envVar in Environment.GetEnvironmentVariables().Cast<DictionaryEntry>())
            {
                writer.WriteLine($"{envVar.Key}: {envVar.Value}");
            }
        }
    }
}
