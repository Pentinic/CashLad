using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashLad.Data.Services
{
    public interface IBaseService<T> where T : class
    {
        public T GetById(int id);
        public Task<T> GetByIdAsync(int id);
        public List<T> GetAllPaged(int page, int pageSize = int.MaxValue);
        public Task<List<T>> GetAllPagedAsync(int page, int pageSize = int.MaxValue);
        public Task<int> GetAllCountAsync();
        public List<T> GetAll();
        public void Add(T entity);
        public void Add(IEnumerable<T> entities);
        public void Update(T entity);
        public void Update(IList<T> entities);
        public void Remove(T entity);
        public void Remove(IEnumerable<T> entities);
    }


    public class BaseService<T> where T : BaseEntity
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<T> _repository;

        public BaseService(DatabaseContext context)
        {
            _context = context;
            _repository = _context.Set<T>();
        }

        public virtual T GetById(int id)
        {
            var query = _repository.Where(x => x.Id == id);
            return query.FirstOrDefault();
        }
        public virtual async Task<T> GetByIdAsync(int id)
        {
            var query = _repository.Where(x => x.Id == id);
            return await query.FirstOrDefaultAsync();
        }
        
        public List<T> GetAllPaged(int page, int pageSize = int.MaxValue)
        {
            var skip = (page - 1) * pageSize;
            var query = _repository.Skip(skip).Take(pageSize);
            
            
            return query.ToList();
        }
        public async Task<List<T>> GetAllPagedAsync(int page, int pageSize = int.MaxValue)
        {
            var skip = page * pageSize;
            var query = _repository.Skip(skip).Take(pageSize);


            return await query.ToListAsync();
        }
        public async Task<int> GetAllCountAsync()
        {
            return await _repository.CountAsync();
        }

        public List<T> GetAll()
        {
            return _repository.ToList();
        }

        public void Add(T entity)
        {
            _repository.Add(entity);
            _context.SaveChanges();
        }
        public void Add(IEnumerable<T> entities)
        {
            _repository.AddRange(entities);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
            _context.SaveChanges();
        }
        public void Update(IList<T> entities)
        {
            _repository.UpdateRange(entities);
            _context.SaveChanges();
        }
    
        public void Remove(T entity)
        {
            _repository.Remove(entity);
            _context.SaveChanges();
        }
        public void Remove(IEnumerable<T> entities)
        {
            _repository.RemoveRange(entities);
            _context.SaveChanges();
        }
    }
}
