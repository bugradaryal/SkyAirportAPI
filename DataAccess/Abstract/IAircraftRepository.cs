using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DataAccess.Abstract
{
    public interface IAircraftRepository
    {
        Task<List<Aircraft>> GetAll();
        Task<List<Aircraft>> GetAllById(int id);
    }
}
