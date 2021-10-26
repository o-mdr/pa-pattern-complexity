using System;
using System.Collections.Generic;

namespace PatternPA.Core.Interfaces
{
    public interface IRepository<T>
    {
        T FindBy(long id);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindAll(Func<T, bool> exp);
        void Delete(T value);
        void Save(T value);
        void Update(T value);
    }
}