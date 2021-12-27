using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Notlarim101.Core.DataAccess;
using Notlarim101.DataAccessLayer.EntityFramework;

namespace Notlarim101.BusinessLayer.Abstract
{
    public abstract class ManagerBase<T> : IDataAccess<T> where T : class//abstract classlarla oluşturulan metodlar virtualla oluşturulur daha sonrasında ezebilmek için
    {
        private Repository<T> repo = new Repository<T>();//repositorynin arasında aracı katman
        public virtual int Delete(T obj)
        {
            return repo.Delete(obj);
        }

        public virtual T Find(Expression<Func<T, bool>> find)
        {
            return repo.Find(find);
        }

        public virtual int Insert(T obj)
        {
            return repo.Insert(obj);
        }

        public virtual List<T> List()
        {
            return repo.List();
        }

        public virtual List<T> List(Expression<Func<T, bool>> where)
        {
            return repo.List(where);
        }

        public virtual IQueryable<T> QList()
        {
            return repo.QList();
        }

        public virtual int Save()
        {
            return repo.Save();
        }

        public virtual int Update(T obj)
        {
            return repo.Update(obj);
        }
    }
}
