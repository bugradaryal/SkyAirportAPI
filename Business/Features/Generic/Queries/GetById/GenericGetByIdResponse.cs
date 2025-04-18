using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;

namespace Business.Features.Generic.Queries.GetById
{
    public class GenericGetByIdResponse<TEntity>
    {
        public TEntity entity { get; set; }
        public CustomException exception { get; set; }
        public bool error { get; set; } = true;
    }
}
