using System;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Persistence.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository Repository();
        IDatabaseTransaction BeginTransaction();
        Task ExecuteWithTransactionAsync(Func<Task> action);
        Task SaveChanges();
    }
}
