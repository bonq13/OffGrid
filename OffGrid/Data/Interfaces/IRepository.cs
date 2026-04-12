namespace OffGrid.Data.Interfaces;

public interface IRepository<T>
{
    void Save(IEnumerable<T> data, string fileName);
    List<T> Load(string fileName);
}