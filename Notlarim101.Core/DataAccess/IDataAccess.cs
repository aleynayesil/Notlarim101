using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.Core.DataAccess
{
    public interface IDataAccess<T>
    {
        List<T> List();

        List<T> List(Expression<Func<T, bool>> where);

        IQueryable<T> QList(Expression<Func<T, bool>> query);

        int Insert(T obj);
        int Update(T obj);
        int Delete(T obj);

        int Save();

        T Find(Expression<Func<T, bool>> find);
    }
}
