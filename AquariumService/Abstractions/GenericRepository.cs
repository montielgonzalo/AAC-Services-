using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AquariumService.Abstractions
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal DbContext _context;
        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        [Description("Add a new  entity from DB")]
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        [Description("Add a new range of entities from DB")]
        public void AddRange(IEnumerable<T> entity)
        {
            _context.Set<T>().AddRange(entity);
        }

        [Description("Delete an entity from DB")]
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        [Description("Delete a range of entities from BD")]
        public void DeleteRange(IEnumerable<T> entity)
        {
            _context.Set<T>().RemoveRange(entity);
        }

        [Description("Detach an entity")]
        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;

        }

        [Description("Edit an entity from DB")]
        public void Edit(T entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
        }

        [Description("Edit a range of entities from DB")]
        public void EditRange(IEnumerable<T> entity)
        {
            _context.Set<T>().UpdateRange(entity);
        }

        [Description("Find a specified entity from DB")]
        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            return query;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        [Description("Get all data from a table")]
        public IQueryable<T> GetAll()
        {
            IQueryable<T> query = _context.Set<T>();
            return query;
        }

        [Description("Save changes made in the context to DB")]
        public void Save()
        {
            _context.SaveChanges();
        }

        [Description("Asynchronously save changes made in the context to DB")]
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
