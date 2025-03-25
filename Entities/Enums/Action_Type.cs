using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums
{
    public enum Action_Type
    {
        Create,
        Read,
        Update,
        Delete,
        Login,
        Logout,
        FileUpload,
        FileDownload,
        APIRequest,
        APIResponse,
        SystemError,
        PerformanceIssue
    }
}
