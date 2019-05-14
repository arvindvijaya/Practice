using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Extensions.EntityFrameWorkCore.MsSql
{
    /// <summary>
    /// mysql dbcontext interface
    /// </summary>
    public interface IDbContextMsSql: IDisposable
    {
        IQueryable<TEntity> GetAsQueryable<TEntity>() where TEntity : class;

        Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;

        EntityEntry Add(object entity);

        void Update(object entity);

        void AddRange(params object[]entities);

        EntityEntry Attach(object entity);

        void UpdateRange(params object[] entities);

        void PartialUpdate(object entity, params string[] properties);

        void PartialUpdateRange(params (object Entity, string [] Properties)[] entities);

        EntityEntry Remove(object entity);

        void RemoveRange(params object[] entities);

        Task<int> SaveChangesAsync(CancellationToken token = default(CancellationToken));

        Task<int> ExecuteSqlCommandAsync(string sql,params object [] parameters);

        Task<int> ExecuteNonQueryProcCommandAsync(string proc, params SqlParameter[] parameters);

        Task<DbDataReader> ExecuteReaderProcCommandAsync(string proc, params SqlParameter[] parameters);

        Task<IEnumerable<TResult>> ExecuteReaderListProcCommandAsync<TResult>(string proc, object parameters);

        Task<TResult> ExecuteReaderSingleProcCommandAsync<TResult>(string proc, object parameters);
    }
}
