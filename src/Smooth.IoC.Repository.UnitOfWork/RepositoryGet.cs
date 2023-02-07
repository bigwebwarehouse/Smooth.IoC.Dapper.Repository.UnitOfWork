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
    public virtual TEntity GetKey(TPk key, ISession session)
    {
        if (_container.IsIEntity<TEntity, TPk>())
        {
            return session.QuerySingleOrDefault<TEntity>(
                $"SELECT * FROM {Sql.Table<TEntity>(session.SqlDialect)} WHERE Id = @Id",
                new { Id = key });
        }

        TEntity entity = CreateEntityAndSetKeyValue(key);
        return session.Get(entity);
    }

    public virtual TEntity GetKey<TSession>(TPk key) 
        where TSession : class, ISession
    {
        using TSession session = Factory.Create<TSession>();
        return GetKey(key, session);
    }

    public virtual TEntity GetKey(TPk key, IUnitOfWork uow)
    {
        if (_container.IsIEntity<TEntity, TPk>())
        {
            return uow.Connection.QuerySingleOrDefault<TEntity>(
                $"SELECT * FROM {Sql.Table<TEntity>(uow.SqlDialect)} WHERE Id = @Id",
                new { Id = key }, uow.Transaction);
        }

        TEntity entity = CreateEntityAndSetKeyValue(key);
        return uow.Get(entity);
    }

    public virtual async Task<TEntity> GetKeyAsync(TPk key, ISession session)
    {
        if (_container.IsIEntity<TEntity, TPk>())
        {
            return await session.QuerySingleOrDefaultAsync<TEntity>(
                $"SELECT * FROM {Sql.Table<TEntity>(session.SqlDialect)} WHERE Id = @Id",
                new { Id = key });
        }

        TEntity entity = CreateEntityAndSetKeyValue(key);
        return await GetAsync(entity, session);
    }

    public virtual async Task<TEntity> GetKeyAsync<TSession>(TPk key)
        where TSession : class, ISession
    {
        using TSession session = Factory.Create<TSession>();
        return await GetKeyAsync(key, session);
    }

    public virtual async Task<TEntity> GetKeyAsync(TPk key, IUnitOfWork uow)
    {
        if (_container.IsIEntity<TEntity, TPk>())
        {
            return await uow.Connection.QuerySingleOrDefaultAsync<TEntity>(
                $"SELECT * FROM {Sql.Table<TEntity>(uow.SqlDialect)} WHERE Id = @Id",
                new { Id = key }, uow.Transaction);
        }

        TEntity entity = CreateEntityAndSetKeyValue(key);
        return await uow.GetAsync(entity);
    }

    public virtual TEntity Get(TEntity entity, ISession session)
    {
        return _container.IsIEntity<TEntity, TPk>()
            ? session.QuerySingleOrDefault<TEntity>(
                $"SELECT * FROM {Sql.Table<TEntity>(session.SqlDialect)} WHERE Id = @Id",
                new { ((IEntity<TPk>)entity).Id })
            : session.Get(entity);
    }

    public virtual TEntity Get<TSession>(TEntity entity)
        where TSession : class, ISession
    {
        using TSession session = Factory.Create<TSession>();
        return Get(entity, session);
    }

    public virtual TEntity Get(TEntity entity, IUnitOfWork uow)
    {
        return _container.IsIEntity<TEntity, TPk>()
            ? uow.Connection.QuerySingleOrDefault<TEntity>(
                $"SELECT * FROM {Sql.Table<TEntity>(uow.SqlDialect)} WHERE Id = @Id",
                new { ((IEntity<TPk>)entity).Id }, uow.Transaction)
            : uow.Get(entity);
    }

    public virtual async Task<TEntity> GetAsync(TEntity entity, ISession session)
    {
        return _container.IsIEntity<TEntity, TPk>()
            ? await session.QuerySingleOrDefaultAsync<TEntity>(
                $"SELECT * FROM {Sql.Table<TEntity>(session.SqlDialect)} WHERE Id = @Id",
                new { ((IEntity<TPk>)entity).Id })
            : await session.GetAsync(entity);
    }

    public virtual async Task<TEntity> GetAsync(TEntity entity, IUnitOfWork uow)
    {
        return _container.IsIEntity<TEntity, TPk>()
            ? await uow.Connection.QuerySingleOrDefaultAsync<TEntity>(
                $"SELECT * FROM {Sql.Table<TEntity>(uow.SqlDialect)} WHERE Id = @Id",
                new { ((IEntity<TPk>)entity).Id }, uow.Transaction)
            : await uow.GetAsync(entity);
    }

    public virtual async Task<TEntity> GetAsync<TSession>(TEntity entity)
        where TSession : class, ISession
    {
        using TSession session = Factory.Create<TSession>();
        return await GetAsync(entity, session);
    }
}