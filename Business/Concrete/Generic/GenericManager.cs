using Business.Abstract;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.Generic
{
    public class GenericManager<T> : IGenericManager<T> where T : class
    {
        private IGenericRepository<T> _repository;
        public GenericManager() { }

        public async Task<List<T>> GetAll() 
        { 
            return await _repository.GetAll();
        }
        public async Task<T> GetValue(int id)
        {
            return await _repository.GetValue(id);
        }

        public async Task Add(T generic)
        {
            await _repository.Add(generic);
        }
        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }
        public async Task Update(T generic)
        {
            await _repository.Update(generic);
        }
    }
}
