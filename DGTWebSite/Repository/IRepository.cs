using System.Collections.Generic;

namespace DGTWebSite.Repository
{
    public interface IRepository<T, K>
    {
        void Add(T entity);
        void Save(T entity);
        T Get(K id);
        void Remove(T entity);
        IEnumerable<T> GetAll();
        void RemoveAll();
    }
}