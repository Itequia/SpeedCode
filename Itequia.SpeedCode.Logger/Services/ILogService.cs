using Itequia.SpeedCode.Logger.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Logger.Services
{
    public interface ILogService
    {
        Task<List<Logs>> GetLogs(Expression<Func<Logs, bool>> predicate);
    }
}
