using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Features.Generic.Queries.GetById
{
    public class GenericGetByIdResponse<TEntity>
    {
        public TEntity? entity { get; set; }
    }
}
