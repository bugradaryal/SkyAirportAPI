using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public GenericRepository() { }

        public async Task<List<T>> GetAll()
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
        }
        public async Task<T> GetValue(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                return await _dbContext.Set<T>().FirstOrDefaultAsync(x => EF.Property<int>(x, "id") == id);
            }
        }
        public async Task Add(T generic)
        {
            using (var _dbContext = new DataDbContext())
            {
                _dbContext.Set<T>().Add(generic);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task Delete(int id)
        {
            using (var _dbContext = new DataDbContext())
            {
                var value = await _dbContext.Set<T>().FirstOrDefaultAsync(x => EF.Property<int>(x, "id") == id);
                _dbContext.Set<T>().Remove(value);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task Update(T generic)
        {
            using (var _dbContext = new DataDbContext())
            {
                _dbContext.Set<T>().Update(generic);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
