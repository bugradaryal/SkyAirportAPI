using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums
{
    public enum Operation_Type
    {
        Create,
        Update,
        Delete,
        Login,
        Logout,
        Access,
        FailedLogin,
        PasswordChange,
        PermissionChange,
        RoleAssignment,
        AccountLock,
        AccountUnlock,
        DataImport,
        DataExport,
        SystemConfigChange,
        Audit,
        NotificationSent,
        BatchProcessStart,
        BatchProcessEnd,
        BackupInitiated,
        FileUpload,
        FileDownload,
        SystemError,
        DataValidation,
        ApiAccess
    }
}
