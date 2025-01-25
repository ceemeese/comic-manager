namespace Utils;
using System.Text.Json;

public static class JsonUtils
{

    public static void SaveDataToJson<T>(List<T> data, string fileName)
    {
        try 
        {
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../"));

            string fullFileName = Path.Combine(path, "Data", fileName);
            Console.WriteLine(fullFileName);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fullFileName, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar datos en el archivo JSON: {ex.Message}");
        }
        
    }


    public static List<T> LoadDataJson<T>(string fileName)
    {
        try
        {
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../"));

            string fullFileName = Path.Combine(path, "Data", fileName);

            string jsonContent = File.ReadAllText(fullFileName);
            return JsonSerializer.Deserialize<List<T>>(jsonContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer el archivo JSON: {ex.Message}");
            return new List<T>();
        }
    }
        
}



