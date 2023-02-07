using System;
using System.Collections.Concurrent;

namespace Smooth.IoC.Repository.UnitOfWork.Containers;

internal sealed class SqlDialectContainer
{
    private readonly ConcurrentDictionary<Type, bool> _entityIsFrozenOrDialogueCorrect = new();

    private static volatile SqlDialectContainer _instance;
    private static readonly object SyncRoot = new();

    private SqlDialectContainer()
    {
    }

    internal static SqlDialectContainer Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            lock (SyncRoot)
            {
                _instance ??= new SqlDialectContainer();
            }

            return _instance;
        }
    }

    internal bool TryEntityIsFrozenOrDialogueIsCorrect<TEntity>()
        where TEntity : class
    {
        return _entityIsFrozenOrDialogueCorrect.TryGetValue(typeof(TEntity), out bool isFrozen) && isFrozen;
    }

    internal void AddEntityFrozenOrDialogueState<TEntity>(bool state)
        where TEntity : class
    {
        _entityIsFrozenOrDialogueCorrect.AddOrUpdate(typeof(TEntity), state, (key, oldValue) => state);
    }

    internal bool? GetState<TEntity>() where TEntity : class
    {
        return !_entityIsFrozenOrDialogueCorrect.ContainsKey(typeof(TEntity))
            ? null
            : TryEntityIsFrozenOrDialogueIsCorrect<TEntity>();
    }

    internal void Clear()
    {
        _entityIsFrozenOrDialogueCorrect.Clear();
    }
}