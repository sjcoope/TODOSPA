using SJCNet.Todo.Data.Repository.Contract;
using SJCNet.Todo.Model.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Data.Repository
{
    public class EFRepository<T> : IRepository<T> where T : class, IEntity
    {
        #region Constructors

        public EFRepository(DbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException("dbContext");
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        #endregion

        #region Properties

        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        protected virtual IQueryable<T> Query { get; set; }

        #endregion

        #region Eager Loading Methods

        /// <summary>
        /// Used to specify the list of properties to be eager loaded so we have full control over the size of
        /// requests made.  If the eager load list is not specified then the default object will be loaded.
        /// </summary>
        /// <param name="eagerLoadList">List of properties to eager load in the query.</param>
        public void EagerLoad(params Expression<Func<T, object>>[] eagerLoadList)
        {
            IQueryable<T> query = DbContext.Set<T>().AsQueryable();
            foreach (var load in eagerLoadList) query = query.Include(load);
            Query = query;
        }

        public void EagerLoad(params string[] eagerLoadList)
        {
            IQueryable<T> query = DbContext.Set<T>().AsQueryable();
            foreach (var load in eagerLoadList) query = query.Include(load);
            Query = query;
        }

        /// <summary>
        /// Controls the eager loading of navigation properties in derived classes.  Should be implemented
        /// by the derived class.
        /// </summary>
        public virtual void FullEagerLoad()
        {
            // To be implemented in derived classes if required.
        }

        #endregion

        #region Data CRUD Methods

        public virtual IQueryable<T> GetAll()
        {
            return Query ?? DbSet;
        }

        public virtual T GetById(int id)
        {
            if (Query != null) return Query.SingleOrDefault(i => i.Id == id);
            else return DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Detached) dbEntityEntry.State = EntityState.Added;
            else DbSet.Add(entity);
        }

        public virtual void Add(IEnumerable<T> entities)
        {
            foreach (var entity in entities) Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached) DbSet.Attach(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities) Update(entity);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            var entitiesToDelete = entities.ToList();
            foreach (var entity in entitiesToDelete)
            {
                Delete(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted) dbEntityEntry.State = EntityState.Deleted;
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        #endregion
    }
}
