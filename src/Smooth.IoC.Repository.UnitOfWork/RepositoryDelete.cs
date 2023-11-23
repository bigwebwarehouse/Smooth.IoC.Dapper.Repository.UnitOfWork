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
    //public virtual bool DeleteKey(TPk key, ISession session)
    //{
    //    TEntity entity = CreateEntityAndSetKeyValue(key);
    //    return session.Delete(entity);
    //}

    public virtual bool DeleteKey(TPk key, IUnitOfWork uow)
    {
        if (_container.IsIEntity<TEntity, TPk>())
            return uow.Connection.Execute($"DELETE FROM {Sql.Table<TEntity>(uow.SqlDialect)} WHERE Id = @Id",
                new { Id = key }, uow.Transaction) == 1;

        TEntity entity = CreateEntityAndSetKeyValue(key);
        return uow.Delete(entity);
    }

    public virtual bool DeleteKey<TSession>(TPk key)
        where TSession : class, ISession
    {
        using IUnitOfWork uow = Factory.Create<IUnitOfWork, TSession>();
        bool success = DeleteKey(key, uow);
        uow.Commit();
        return success;
    }

    //public virtual async Task<bool> DeleteKeyAsync(TPk key, ISession session)
    //{
    //    if (_container.IsIEntity<TEntity, TPk>())
    //        return await Task.Run(() => session.Execute(
    //            $"DELETE FROM {Sql.Table<TEntity>(session.SqlDialect)} WHERE Id = @Id",
    //            new { Id = key }) == 1);

    //    TEntity entity = CreateEntityAndSetKeyValue(key);
    //    return await session.DeleteAsync(entity);
    //}

    public virtual async Task<bool> DeleteKeyAsync(TPk key, IUnitOfWork uow)
    {
        if (_container.IsIEntity<TEntity, TPk>())
            return await uow.Connection.ExecuteAsync(
                $"DELETE FROM {Sql.Table<TEntity>(uow.SqlDialect)} WHERE Id = @Id",
                new { Id = key }, uow.Transaction) == 1;

        TEntity entity = CreateEntityAndSetKeyValue(key);
        return await uow.DeleteAsync(entity);
    }

    public virtual async Task<bool> DeleteKeyAsync<TSession>(TPk key)
        where TSession : class, ISession
    {
        using IUnitOfWork uow = Factory.Create<IUnitOfWork, TSession>();
        bool success = await DeleteKeyAsync(key, uow);
        uow.Commit();
        return success;
    }

    //public virtual bool Delete(TEntity entity, ISession session)
    //{
    //    if (_container.IsIEntity<TEntity, TPk>())
    //        return session.Execute($"DELETE FROM {Sql.Table<TEntity>(session.SqlDialect)} WHERE Id = @Id",
    //            new { ((IEntity<TPk>)entity).Id }) == 1;

    //    return session.Delete(entity);
    //}

    public virtual bool Delete(TEntity entity, IUnitOfWork uow)
    {
        if (_container.IsIEntity<TEntity, TPk>())
            return uow.Connection.Execute($"DELETE FROM {Sql.Table<TEntity>(uow.SqlDialect)} WHERE Id = @Id",
                new { ((IEntity<TPk>)entity).Id }, uow.Transaction) == 1;

        return uow.Delete(entity);
    }

    public virtual bool Delete<TSession>(TEntity entity)
        where TSession : class, ISession
    {
        using IUnitOfWork uow = Factory.Create<IUnitOfWork, TSession>();
        bool success;
        if (_container.IsIEntity<TEntity, TPk>())
        {
            success = uow.Connection.Execute($"DELETE FROM {Sql.Table<TEntity>(uow.SqlDialect)} WHERE Id = @Id",
                new { ((IEntity<TPk>)entity).Id }, uow.Transaction) == 1;
            uow.Commit();

            return success;
        }

        success = uow.Delete(entity);
        uow.Commit();

        return success;
    }

    //public virtual async Task<bool> DeleteAsync(TEntity entity, ISession session)
    //{
    //    return _container.IsIEntity<TEntity, TPk>()
    //        ? await Task.Run(() => session.Execute(
    //            $"DELETE FROM {Sql.Table<TEntity>(session.SqlDialect)} WHERE Id = @Id",
    //            new { ((IEntity<TPk>)entity).Id }) == 1)
    //        : await session.DeleteAsync(entity);
    //}

    public virtual Task<bool> DeleteAsync(TEntity entity, IUnitOfWork uow)
    {
        return _container.IsIEntity<TEntity, TPk>()
            ? Task.Run(() => uow.Connection.Execute(
                $"DELETE FROM {Sql.Table<TEntity>(uow.SqlDialect)} WHERE Id = @Id",
                new { ((IEntity<TPk>)entity).Id }, uow.Transaction) == 1)
            : uow.DeleteAsync(entity);
    }

    public virtual async Task<bool> DeleteAsync<TSession>(TEntity entity) where TSession : class, ISession
    {
        using IUnitOfWork uow = Factory.Create<IUnitOfWork, TSession>();
        bool success = await DeleteAsync(entity, uow);
        uow.Commit();
        return success;
    }
}