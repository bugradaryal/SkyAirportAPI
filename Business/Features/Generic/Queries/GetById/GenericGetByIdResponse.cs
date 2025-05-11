using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Utilitys.ResponseHandler;

namespace Business.Features.Generic.Queries.GetById
{
    public class GenericGetByIdResponse<TEntity>
    {
        public TEntity? entity { get; set; }
        public ResponseModel? response { get; set; }
        public bool error { get; set; } = true;
    }
}
