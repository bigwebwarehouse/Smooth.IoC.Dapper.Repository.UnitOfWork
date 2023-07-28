using Dapper.FastCrud;
using Dapper.FastCrud.Configuration.StatementOptions.Builders;
//using Smooth.IoC.Repository.UnitOfWork.Helpers;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smooth.IoC.Repository.UnitOfWork.Extensions;

public static class UnitOfWorkExtensions
{
    //private static readonly SqlDialectHelper DialogueHelper = new();

    public static int BulkDelete<TEntity>(
        this IUnitOfWork uow,
        Action<IConditionalBulkSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.BulkDelete<TEntity>(statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.BulkDelete(statementOptions);
    }

    public static int BulkDelete<TEntity>(
        this IUnitOfWork uow,
        FormattableString whereClause,
        object parameters)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);
        return uow.Connection.BulkDelete<TEntity>(statement => statement
            .AttachToTransaction(uow.Transaction)
            .Where(whereClause)
            .WithParameters(parameters));
    }

    public static Task<int> BulkDeleteAsync<TEntity>(
        this IUnitOfWork uow,
        Action<IConditionalBulkSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.BulkDeleteAsync<TEntity>(statement =>
                statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.BulkDeleteAsync(statementOptions);
    }

    public static Task<int> BulkDeleteAsync<TEntity>(
        this IUnitOfWork uow,
        FormattableString whereClause,
        object parameters)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);
        return uow.Connection.BulkDeleteAsync<TEntity>(statement => statement
            .AttachToTransaction(uow.Transaction)
            .Where(whereClause)
            .WithParameters(parameters));
    }

    public static int BulkUpdate<TEntity>(
        this IUnitOfWork uow,
        TEntity updateData,
        Action<IConditionalBulkSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.BulkUpdate(updateData,
                statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.BulkUpdate(updateData, statementOptions);
    }

    public static Task<int> BulkUpdateAsync<TEntity>(
        this IUnitOfWork uow,
        TEntity updateData,
        Action<IConditionalBulkSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.BulkUpdateAsync(updateData,
                statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.BulkUpdateAsync(updateData, statementOptions);
    }

    public static int Count<TEntity>(
        this IUnitOfWork uow,
        Action<IConditionalSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.Count<TEntity>(statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.Count(statementOptions);
    }

    public static Task<int> CountAsync<TEntity>(
        this IUnitOfWork uow,
        Action<IConditionalSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.CountAsync<TEntity>(statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.CountAsync(statementOptions);
    }

    public static bool Delete<TEntity>(
        this IUnitOfWork uow,
        TEntity entityToDelete,
        Action<IStandardSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.Delete(entityToDelete,
                statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.Delete(entityToDelete, statementOptions);
    }

    public static Task<bool> DeleteAsync<TEntity>(
        this IUnitOfWork uow,
        TEntity entityToDelete,
        Action<IStandardSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.DeleteAsync(entityToDelete,
                statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.DeleteAsync(entityToDelete, statementOptions);
    }

    public static IEnumerable<TEntity> Find<TEntity>(
        this IUnitOfWork uow,
        Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.Find<TEntity>(statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.Find(statementOptions);
    }

    public static Task<IEnumerable<TEntity>> FindAsync<TEntity>(
        this IUnitOfWork uow,
        Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.FindAsync<TEntity>(statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.FindAsync(statementOptions);
    }

    public static TEntity Get<TEntity>(
        this IUnitOfWork uow,
        TEntity entityKeys,
        Action<ISelectSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.Get(entityKeys, statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.Get(entityKeys, statementOptions);
    }

    public static Task<TEntity> GetAsync<TEntity>(
        this IUnitOfWork uow,
        TEntity entityKeys,
        Action<ISelectSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.GetAsync(entityKeys, statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.GetAsync(entityKeys, statementOptions);
    }

    public static void Insert<TEntity>(
        this IUnitOfWork uow,
        TEntity entityToInsert,
        Action<IStandardSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
        {
            uow.Connection.Insert(entityToInsert, statement => statement.AttachToTransaction(uow.Transaction));
        }
        else
        {
            statementOptions += x => x.AttachToTransaction(uow.Transaction);
            uow.Connection.Insert(entityToInsert, statementOptions);
        }
    }

    public static Task InsertAsync<TEntity>(this IUnitOfWork uow,
        TEntity entityToInsert,
        Action<IStandardSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.InsertAsync(entityToInsert,
                statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.InsertAsync(entityToInsert, statementOptions);
    }

    public static bool Update<TEntity>(
        this IUnitOfWork uow,
        TEntity entityToUpdate,
        Action<IStandardSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.Update(entityToUpdate,
                statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.Update(entityToUpdate, statementOptions);
    }


    /// <summary>Updates a record in the database.</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="uow">Database connection.</param>
    /// <param name="entityToUpdate">
    /// The entity you wish to update.
    /// For partial updates use an entity mapping override.
    /// </param>
    /// <param name="statementOptions">Optional statement options (usage: statement =&gt; statement.SetTimeout().AttachToTransaction()...)</param>
    /// <returns>True if the item was updated.</returns>
    public static Task<bool> UpdateAsync<TEntity>(
        this IUnitOfWork uow,
        TEntity entityToUpdate,
        Action<IStandardSqlStatementOptionsBuilder<TEntity>> statementOptions = null)
        where TEntity : class
    {
        //DialogueHelper.SetDialogueIfNeeded<TEntity>(uow.SqlDialect);

        if (statementOptions is null)
            return uow.Connection.UpdateAsync(entityToUpdate,
                statement => statement.AttachToTransaction(uow.Transaction));

        statementOptions += x => x.AttachToTransaction(uow.Transaction);
        return uow.Connection.UpdateAsync(entityToUpdate, statementOptions);
    }
}