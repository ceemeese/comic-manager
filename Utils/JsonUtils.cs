namespace Utils;
using System.Text.Json;

public static class JsonUtils
{

    public static void SaveDataToJson<T>(List<T> data, string fileName)
    {
        string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Data");

        string fullFileName = Path.Combine(path, fileName);
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fullFileName, json);
    }

}



