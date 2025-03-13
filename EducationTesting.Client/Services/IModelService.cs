namespace EducationTesting.Client.Services
{
    public interface IModelService<T>
    {
        T Get(string id);
        void Create(T item);
        void Update(T item);
        void Delete(string id);
    }
}