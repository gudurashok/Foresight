using System.Collections.Generic;
using Scalable.Shared.DataAccess;
using System;

namespace Scalable.Shared.Repositories
{
    public abstract class Repository : IRepository, IDisposable
    {
        private readonly IList<dynamic> _innerList;
        protected SqlDbManager db;

        protected Repository()
        {
            _innerList = null; //createTestData();
        }

        public IEnumerator<dynamic> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}


//public abstract class Repository<T> : IDisposable where T : class, IIdentifiable
//{
//    protected DbContext DbContext;

//    public Repository(DbContext dataContext)
//    {
//        DbContext = dataContext;
//    }

//    //public void Add(T entity)
//    //{
//    //    _db.GetTable<T>().InsertOnSubmit(entity);
//    //    _db.SubmitChanges();
//    //}

//    //public void Remove(T entity)
//    //{
//    //    _db.GetTable<T>().DeleteOnSubmit(entity);
//    //    _db.SubmitChanges();
//    //}

//    //public T GetById(int id)
//    //{
//    //    T entity = _db.GetTable<T>().SingleOrDefault(e => e.Id.Equals(id));
//    //    return entity;
//    //}

//    //public IQueryable<T> GetByFilter(Expression<Func<T, bool>> filter)
//    //{
//    //    return _db.GetTable<T>().Where(filter);
//    //}

//    public void Dispose()
//    {
//        DbContext.Dispose();
//    }
//}

//public interface IRepository<T> where T : class
//{
//    void Add(T entity);
//    long Count();
//    long Count(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition);
//    void Delete(T entity);
//    void DeleteAll();
//    System.Collections.Generic.IList<T> GetAll();
//    System.Collections.Generic.IList<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition);
//    System.Linq.IQueryable<T> GetQueryable();
//    T GetSingle(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition);
//    void Update(T entity);
//}

//public sealed class RocketRepository<T> : IRepository<T> where T : EntityObject
//{
//    static readonly RocketRepository<T> instance = new RocketRepository<T>(new ObjectContext()); //(new Rocket_CoreEntities());
//    private static readonly Object _lockObject = new Object();
//    // Explicit static constructor to tell C# compiler
//    // not to mark type as beforefieldinit
//    static RocketRepository()
//    {
//        if (_lockObject == null)
//            _lockObject = new Object();
//    }

//    public static RocketRepository<T> Instance
//    {
//        get
//        {
//            //return new RocketRepository<T>(new RocketCoreEntities());
//            return instance;
//        }
//    }

//    RocketRepository(ObjectContext repositoryContext)
//    {
//        _repositoryContext = repositoryContext ?? new Rocket_CoreEntities();
//        _objectSet = _repositoryContext.CreateObjectSet<T>();
//    }

//    public IRepository<T> GetRepository()
//    {
//        return Instance;
//    }

//    private static ObjectContext _repositoryContext;
//    private ObjectSet<T> _objectSet;
//    public ObjectSet<T> ObjectSet
//    {
//        get
//        {
//            return _objectSet;
//        }
//    }

//    #region IRepository Members

//    public void Add(T entity)
//    {
//        lock (_lockObject)
//        {
//            this._objectSet.AddObject(entity);
//            _repositoryContext.SaveChanges();
//            _repositoryContext.AcceptAllChanges();
//        }
//    }

//    public void Update(T entity)
//    {
//        lock (_lockObject)
//        {
//            _repositoryContext.ApplyOriginalValues(((IEntityWithKey)entity)
//                .EntityKey.EntitySetName, entity);
//            _repositoryContext.Refresh(RefreshMode.ClientWins, _objectSet);
//            _repositoryContext.SaveChanges();
//            _repositoryContext.AcceptAllChanges();
//        }
//    }

//    public void Delete(T entity)
//    {
//        lock (_lockObject)
//        {
//            this._objectSet.DeleteObject(entity);
//            _repositoryContext.Refresh(RefreshMode.ClientWins, _objectSet);
//            _repositoryContext.SaveChanges();
//            _repositoryContext.AcceptAllChanges();
//        }
//    }

//    public void DeleteAll()
//    {
//        _repositoryContext
//            .ExecuteStoreCommand("DELETE " + _objectSet.EntitySet.ElementType.Name);
//    }

//    public IList<T> GetAll()
//    {
//        lock (_lockObject)
//        {
//            return this._objectSet.ToList<T>();
//        }
//    }

//    public IList<T> GetAll(Expression<Func<T, bool>> whereCondition)
//    {
//        lock (_lockObject)
//        {
//            return this._objectSet.Where(whereCondition).ToList<T>();
//        }
//    }

//    public T GetSingle(Expression<Func<T, bool>> whereCondition)
//    {
//        lock (_lockObject)
//        {
//            return this._objectSet.Where(whereCondition).FirstOrDefault<T>();
//        }
//    }

//    public IQueryable<T> GetQueryable()
//    {
//        lock (_lockObject)
//        {
//            return this._objectSet.AsQueryable<T>();
//        }
//    }

//    public long Count()
//    {
//        return this._objectSet.LongCount<T>();
//    }

//    public long Count(Expression<Func<T, bool>> whereCondition)
//    {
//        return this._objectSet.Where(whereCondition).LongCount<T>();
//    }

//    #endregion
//}
