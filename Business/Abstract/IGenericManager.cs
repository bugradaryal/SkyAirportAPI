using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGenericManager<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetValue(int id);
        Task Add(T generic);
        Task Delete(int id);
        Task Update(T generic);
    }
}
