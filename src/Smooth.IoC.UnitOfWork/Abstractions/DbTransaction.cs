using System;
using System.Data;
using System.Data.Common;
using Smooth.IoC.UnitOfWork.Interfaces;

#pragma warning disable 618

namespace Smooth.IoC.UnitOfWork.Abstractions
{
    public abstract class DbTransaction : IDisposable
    {
        private readonly IDbFactory _factory;
        protected bool Disposed;
        protected ISession Session;
        private bool _hasRolledBack;
        private bool _hasCommitted;
        public IDbTransaction Transaction { get; set; }
        public IDbConnection Connection => Transaction?.Connection;
        public IsolationLevel IsolationLevel => Transaction?.IsolationLevel ?? IsolationLevel.Unspecified;

        protected DbTransaction(IDbFactory factory)
        {
            _factory = factory;
        }
        
        public void Commit()
        {
            if (Connection?.State != ConnectionState.Open || TransactionCompleted)
                return;

            try
            {
                Transaction?.Commit();
                _hasCommitted = true;
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                Transaction?.Dispose();
            }
        }

        public void Rollback()
        {
            if (Connection?.State != ConnectionState.Open || TransactionCompleted) 
                return;

            Transaction?.Rollback();
            _hasRolledBack = true;
        }

        ~DbTransaction()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Disposed) 
                return;

            Disposed = true;

            if (!disposing)
                return;

            DisposeTransaction();
            DisposeSessionIfSessionIsNotNull();
        }

        private void DisposeTransaction()
        {
            if (Transaction?.Connection == null) 
                return;

            Rollback(); // Added to ensure uncommitted transactions are rolled back
            Transaction?.Dispose();
            Transaction = null;
            _factory.Release(this);
        }

        private void DisposeSessionIfSessionIsNotNull()
        {
            Session?.Dispose();
            Session = null;
        }

        private bool TransactionCompleted => _hasCommitted || _hasRolledBack;
    }
}
