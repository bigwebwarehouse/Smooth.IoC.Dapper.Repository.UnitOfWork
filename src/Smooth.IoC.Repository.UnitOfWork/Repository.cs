using Dapper.FastCrud.Mappings;
using Smooth.IoC.Repository.UnitOfWork.Containers;
//using Smooth.IoC.Repository.UnitOfWork.Helpers;
using Smooth.IoC.UnitOfWork.Abstractions;
using Smooth.IoC.UnitOfWork.Exceptions;
using Smooth.IoC.UnitOfWork.Helpers;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Smooth.IoC.Repository.UnitOfWork;

public abstract partial class Repository<TEntity, TPk> : RepositoryBase, IRepository<TEntity, TPk>
    where TEntity : class
    where TPk : IComparable
{
    private readonly RepositoryContainer _container = RepositoryContainer.Instance;
    //private readonly SqlDialectHelper _helper;

    //protected SqlInstance Sql { get; } = SqlInstance.Instance;

    protected Repository(IDbFactory factory) : base(factory)
    {
        //_helper = new SqlDialectHelper();
    }

    protected bool TryAllKeysDefault(TEntity entity)
    {
        if (_container.IsIEntity<TEntity, TPk>())
        {
            if (entity is IEntity<TPk> entityInterface)
            {
                return entityInterface.Id.CompareTo(default(TPk)) == 0;
            }
        }

        PropertyMapping[] keys = _container.GetKeys<TEntity>();
        IEnumerable<PropertyInfo> properties = _container.GetProperties<TEntity>(keys);
        if (keys == null || properties == null)
        {
            throw new NoPkException(
                "There is no keys for this entity, please create your logic or add a key attribute to the entity");
        }

        return properties.Select(property => property.GetValue(entity))
            .All(value => value == null || value.Equals(default(TPk)));
    }

    protected TPk GetPrimaryKeyValue(TEntity entity)
    {
        if (_container.IsIEntity<TEntity, TPk>())
            if (entity is IEntity<TPk> entityInterface)
                return entityInterface.Id;

        PropertyInfo primaryKeyValue = GetPrimaryKeyPropertyInfo();
        return (TPk)primaryKeyValue.GetValue(entity);
    }

    protected void SetPrimaryKeyValue(TEntity entity, TPk value)
    {
        if (_container.IsIEntity<TEntity, TPk>())
            if (entity is IEntity<TPk> entityInterface)
            {
                entityInterface.Id = value;
                return;
            }

        PropertyInfo primaryKeyValue = GetPrimaryKeyPropertyInfo();
        primaryKeyValue.SetValue(entity, value);
    }

    private PropertyInfo GetPrimaryKeyPropertyInfo()
    {
        PropertyMapping[] keys = _container.GetKeys<TEntity>();
        string primaryKeyName = keys.FirstOrDefault(key => key.IsPrimaryKey)?.PropertyName;
        IEnumerable<PropertyInfo> properties = _container.GetProperties<TEntity>(keys);
        if (keys == null || primaryKeyName == null || properties == null)
        {
            throw new NoPkException(
                "There is no primary ket for this entity, please create your logic or add a key attribute to the entity");
        }

        PropertyInfo primaryKeyValue =
            properties.FirstOrDefault(property => property.Name.Equals(primaryKeyName, StringComparison.Ordinal));
        return primaryKeyValue;
    }

    protected TEntity CreateEntityAndSetKeyValue(TPk key)
    {
        TEntity entity = CreateInstanceHelper.Resolve<TEntity>();
        SetPrimaryKeyValue(entity, key);
        return entity;
    }

    //protected void SetDialogueOnce<T>(IUnitOfWork uow) where T : class
    //{
    //    _helper.SetDialogueIfNeeded<T>(uow.SqlDialect);
    //}

    //protected void SetDialogueOnce<T>(ISession session) where T : class
    //{
    //    _helper.SetDialogueIfNeeded<T>(session.SqlDialect);
    //}
}