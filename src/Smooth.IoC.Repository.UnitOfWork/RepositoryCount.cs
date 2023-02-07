using Dapper;
using Smooth.IoC.Repository.UnitOfWork.Extensions;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Threading.Tasks;

namespace Smooth.IoC.Repository.UnitOfWork;

public abstract partial class Repository<TEntity, TPk>
    where TEntity : class
    where TPk : IComparable
{
    public virtual int Count(ISession session)
    {
        return _container.IsIEntity<TEntity, TPk>()
            ? session.QuerySingleOrDefault<int>(
                $"SELECT count(*) FROM {Sql.Table<TEntity>(session.SqlDialect)}")
            : session.Count<TEntity>();
    }

    public virtual int Count(IUnitOfWork uow)
    {
        return _container.IsIEntity<TEntity, TPk>()
            ? uow.Connection.QuerySingleOrDefault<int>(
                $"SELECT count(*) FROM {Sql.Table<TEntity>(uow.SqlDialect)}", uow.Transaction)
            : uow.Count<TEntity>();
    }

    public virtual int Count<TSession>() where TSession : class, ISession
    {
        using IUnitOfWork uow = Factory.Create<IUnitOfWork, TSession>();
        return Count(uow);
    }

    public virtual async Task<int> CountAsync(ISession session)
    {
        return _container.IsIEntity<TEntity, TPk>()
            ? await session.QuerySingleOrDefaultAsync<int>(
                $"SELECT count(*) FROM {Sql.Table<TEntity>(session.SqlDialect)}")
            : await session.CountAsync<TEntity>();
    }

    public virtual async Task<int> CountAsync(IUnitOfWork uow)
    {
        return _container.IsIEntity<TEntity, TPk>()
            ? await uow.Connection.QuerySingleOrDefaultAsync<int>(
                $"SELECT count(*) FROM {Sql.Table<TEntity>(uow.SqlDialect)}")
            : await uow.CountAsync<TEntity>();
    }

    public virtual async Task<int> CountAsync<TSession>() where TSession : class, ISession
    {
        using IUnitOfWork uow = Factory.Create<IUnitOfWork, TSession>();
        return await CountAsync(uow);
    }
}