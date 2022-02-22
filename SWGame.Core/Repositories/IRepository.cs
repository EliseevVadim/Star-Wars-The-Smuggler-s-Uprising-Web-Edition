using System.Collections.Generic;

namespace SWGame.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        public List<T> LoadAll();
        public T LoadById(int id);
    }
}
