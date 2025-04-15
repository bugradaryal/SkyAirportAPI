using Business.Abstract;
using Business.ExceptionHandler;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.Generic
{
    public class GenericManager<T> : IGenericServices<T> where T : class
    {
        private IGenericRepository<T> _repository;
        public GenericManager() { }

        public async Task<List<T>> GetAll() 
        {
            try
            {
                            return await _repository.GetAll();
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
        public async Task<T> GetValue(int id)
        {
            try
            {
                return await _repository.GetValue(id);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task Add(T generic)
        {
            try
            {
                await _repository.Add(generic);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
        public async Task Delete(int id)
        {
            try
            {
                await _repository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }      
        }
        public async Task Update(T generic)
        {
            try
            {
                await _repository.Update(generic);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
