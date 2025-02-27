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
            

            string fullFileName = Path.Combine(dataPath, fileName);
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
            //ruta local
            string localPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../Data"));


            string dataPath = Environment.GetEnvironmentVariable("DATA_PATH") ?? localPath;

            string fullFileName = Path.Combine(dataPath, fileName);
            Console.WriteLine(fullFileName);

            //Comprueba si existe fichero sino
            if (!File.Exists(fullFileName) || new FileInfo(fullFileName).Length == 0)
            {
            Console.WriteLine("El archivo no existe o está vacío. Se inicializa una lista vacía.");
            return new List<T>();
            }

            string jsonContent = File.ReadAllText(fullFileName);
            
            return JsonSerializer.Deserialize<List<T>>(jsonContent) ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer el archivo JSON: {ex.Message}");
            return new List<T>();
        }
    }
        
}



