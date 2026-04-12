using System.Text.Json;
using OffGrid.Data.Interfaces;

namespace OffGrid.Data;

public class JsonRepository<T> : IRepository<T>
{
    private readonly string _dataFolder = "data";

    public void Save(IEnumerable<T> data, string fileName)
    {
        Directory.CreateDirectory(_dataFolder);
        var dane = JsonSerializer.Serialize(data);
        var filePath = Path.Combine(_dataFolder, fileName + ".json");
        File.WriteAllText(filePath, dane); 
    }

    public List<T> Load(string fileName)
    {
        var filePath = Path.Combine(_dataFolder, fileName + ".json");
        if(!File.Exists(filePath)) return new List<T>();
        var odczyt= File.ReadAllText(filePath);  
        return JsonSerializer.Deserialize<List<T>>(odczyt) ?? new List<T>();
    }
    
}