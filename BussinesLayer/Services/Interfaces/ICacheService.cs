using BussinesLayer.Models.Search;

namespace BussinesLayer.Services
{
    public interface ICacheService<T>
    {
        T Get(object key);
        T Create(object key, T item);
        void Remove(object key);
    }
}
