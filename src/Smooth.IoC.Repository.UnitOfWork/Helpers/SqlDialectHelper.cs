using Dapper.FastCrud;
using Dapper.FastCrud.Mappings;
using Smooth.IoC.Repository.UnitOfWork.Containers;
using Smooth.IoC.UnitOfWork.Helpers;

namespace Smooth.IoC.Repository.UnitOfWork.Helpers;

public sealed class SqlDialectHelper
{
    private readonly object _lockSqlDialectUpdate = new();
    private readonly SqlDialectContainer _container = SqlDialectContainer.Instance;

    public void SetDialogueIfNeeded<TEntity>(IoC.UnitOfWork.SqlDialect sqlDialect) where TEntity : class
    {
        SetDialogueIfNeeded<TEntity>(EnumHelper.ConvertEnumToEnum<SqlDialect>(sqlDialect));
    }

    public void SetDialogueIfNeeded<TEntity>(SqlDialect sqlDialect) where TEntity : class
    {
        if (_container.TryEntityIsFrozenOrDialogueIsCorrect<TEntity>())
        {
            return;
        }

        EntityMapping<TEntity> mapping = OrmConfiguration.GetDefaultEntityMapping<TEntity>();
        if (!mapping.IsFrozen && mapping.Dialect != sqlDialect)
        {
            lock (_lockSqlDialectUpdate)
            {
                mapping = OrmConfiguration.GetDefaultEntityMapping<TEntity>(); //reload to be sure
                if (mapping.IsFrozen || mapping.Dialect == sqlDialect) 
                    return;

                mapping.SetDialect(sqlDialect);
            }
        }

        _container.AddEntityFrozenOrDialogueState<TEntity>(mapping.IsFrozen || mapping.Dialect == sqlDialect);
    }

    public bool? GetEntityState<TEntity>() where TEntity : class
    {
        return _container.GetState<TEntity>();
    }

    public void Reset()
    {
        _container.Clear();
    }
}