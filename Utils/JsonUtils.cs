namespace Utils;
using System.Text.Json;

public static class JsonUtils
{

    public static void SaveDataToJson<T>(List<T> data, string fileName)
    {
        try 
        {

            string localPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../Data"));


            string dataPath = Environment.GetEnvironmentVariable("DATA_PATH") ?? localPath;
            Console.Write(dataPath);

            string fullFileName = Path.Combine(dataPath, fileName);
            
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
            //ruta local
            string localPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../Data"));


            string dataPath = Environment.GetEnvironmentVariable("DATA_PATH") ?? localPath;

            string fullFileName = Path.Combine(dataPath, fileName);

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



