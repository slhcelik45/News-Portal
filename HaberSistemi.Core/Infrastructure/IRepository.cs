using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HaberSistemi.Core.Infrastructure
{
    public interface IRepository<T > where T:class
    {
        //Tüm datamızı çekecek
        IEnumerable<T> GetAll();

        //Tek bir nesne döndürecek
        T GetById(int id);

        T Get(Expression<Func<T, bool>> expression);

        //birden fazla deger dönecek
        IQueryable<T> GetMany(Expression<Func<T, bool>> expression);

        void Insert(T obj);
        void Update(T obj);
        void Delete(int id);
        int Count();
        void Save();       
    }
}
