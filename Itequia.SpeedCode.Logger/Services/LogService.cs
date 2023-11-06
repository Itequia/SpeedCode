using Itequia.SpeedCode.Logger.Models;
using Itequia.SpeedCode.Persistence.Interfaces.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Logger.Services
{
    public class LogService: ILogService
    {
        private readonly IMongoRepository<Logs> _mongoRepository;

        public LogService(IMongoRepository<Logs> mongoRepository)
        {
            this._mongoRepository = mongoRepository;
        }

        public async Task<List<Logs>> GetLogs(Expression<Func<Logs, bool>> predicate)
        {
            return  (await this._mongoRepository.Search(predicate)).ToList();           
        }
    }
}
