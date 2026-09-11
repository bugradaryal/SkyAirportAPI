using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Features.Generic.Queries.GetAll
{
    public class GenericGetAllResponse<TEntity>
    {
        public List<TEntity>? entity { get; set; }
    }
}
