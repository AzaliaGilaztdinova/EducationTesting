namespace EducationTesting.Client.Repositories
{
    public interface IRepository<T>
    {
        T Get(string id);
        void Create(T item);
        void Update(T item);
        void Delete(string id);
    }
}