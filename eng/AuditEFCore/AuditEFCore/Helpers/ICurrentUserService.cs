using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditEFCore.Helpers
{
    public interface ICurrentUserService
    {
        string GetCurrentUsername();
    }
}
