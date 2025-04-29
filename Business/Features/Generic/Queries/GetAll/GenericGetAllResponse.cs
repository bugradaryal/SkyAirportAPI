using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;

namespace Business.Features.Generic.Queries.GetAll
{
    public class GenericGetAllResponse<TEntity>
    {
        public List<TEntity> data { get; set; }
        public CustomException exception { get; set; }
        public bool error { get; set; } = true;
    }
}
