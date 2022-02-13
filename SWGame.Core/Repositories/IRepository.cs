using System;
using System.Collections.Generic;
using SWGame.Core.Models;

namespace SWGame.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        public List<T> LoadAll();
        public T LoadById(int id);
    }
}
