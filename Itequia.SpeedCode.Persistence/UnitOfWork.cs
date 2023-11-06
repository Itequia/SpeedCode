using Itequia.SpeedCode.Persistence.Interfaces;
using System;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository repository;
        private bool _disposed;
        public UnitOfWork(IRepository _repository)
        {
            this.repository = _repository;
        }

        public IRepository Repository()
        {
            return this.repository;
        }

        public async Task SaveChanges()
        {
            await this.repository.GetContext().SaveChangesAsync();
        }

        public IDatabaseTransaction BeginTransaction()
        {
            return new EntityDatabaseTransaction(this.repository.GetContext());
        }

        public async Task ExecuteWithTransactionAsync(Func<Task> action)

        {
            using var transaction = BeginTransaction();
            try
            {
                await action();
                await SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    repository.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
