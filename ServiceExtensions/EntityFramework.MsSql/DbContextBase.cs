using Dapper;
using Extensions.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Extensions.EntityFrameWorkCore.MsSql
{
    /// <summary>
    /// dbcontext for mssql connection which implements methods in IDbContextMsSql
    /// </summary>
    public abstract class MsSqlDbContextBase : DbContext, IDbContextMsSql
    {
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        protected MsSqlDbContextBase(DbContextOptions options,ICurrentUserAccessor currentUserAccessor)
            :base(options)
        {
            _currentUserAccessor = currentUserAccessor;
        } 

        public Task<int> ExecuteNonQueryProcCommandAsync(string proc, params SqlParameter[] parameters)
        {
            return PrepareProcCommand(proc, parameters).ExecuteNonQueryAsync();

        }

        public Task<IEnumerable<TResult>> ExecuteReaderListProcCommandAsync<TResult>(string proc, object parameters)
        {
            return GetConnection().QueryAsync<TResult>(proc, parameters,commandType:CommandType.StoredProcedure);
        }

        public Task<DbDataReader> ExecuteReaderProcCommandAsync(string proc, params SqlParameter[] parameters)
        {
            return PrepareProcCommand(proc, parameters).ExecuteReaderAsync();
        }

        public Task<TResult> ExecuteReaderSingleProcCommandAsync<TResult>(string proc, object parameters)
        {
            return GetConnection().QuerySingleAsync<TResult>(proc, parameters, commandType: CommandType.StoredProcedure);
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommandAsync(sql, CancellationToken.None, parameters);
        }

        public IQueryable<TEntity> GetAsQueryable<TEntity>() where TEntity : class
        {
            return Set<TEntity>().AsNoTracking().AsQueryable();
        }

        public void Update (object entity)
        {
            Attach(entity);
            base.Update(entity); 
        }

        public void PartialUpdate(object entity, params string[] properties)
        {
            Attach(entity);

            foreach(var property in properties)
            {
                base.Entry(entity).Property(property).IsModified = true;
            }
        }

        public void PartialUpdateRange(params (object Entity, string[] Properties)[] entities)
        {
            foreach (var item in entities)
            {
                PartialUpdate(item);
            }
        }

        internal DbCommand PrepareProcCommand(string proc,params SqlParameter[] parameters)
        {
            var dbConnection = GetConnection();
            var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = proc;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddRange(parameters);

            return dbCommand;
        }

        private DbConnection GetConnection()
        {
            var dbConnection = Database.GetDbConnection();

            if (dbConnection.State == ConnectionState.Closed || dbConnection.State == ConnectionState.Broken)
                dbConnection.Open();

            return dbConnection;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken token=default(CancellationToken))
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(token);
        }

        private void UpdateAuditFields()
        {
           var modifiedEntries = ChangeTracker.Entries()
                .Where(x=> x.Entity is IAuditEntity && 
                    (x.State == EntityState.Added || x.State == EntityState.Modified));

            var identityName = _currentUserAccessor?.GetCurrentUser()?.UserLoginId ?? string.Empty;

            DateTime now = DateTime.Now;

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as IAuditEntity;

                if(entry.State == EntityState.Added)
                {
                    entity.CreatedBy = identityName;
                    entity.CreatedDate = now; 
                }

                entity.UpdatedBy = identityName;
                entity.UpdatedDate = now; 
            }

        }

        protected virtual Assembly OnAssemblyModelCreating()
        {
            return GetType().GetTypeInfo().Assembly;
        }

        protected virtual Func<Type,bool> OnEntityMappingModelCreatingPredicate()
        {
            return x=>x.GetTypeInfo().BaseType != null && x.GetTypeInfo().BaseType.IsGenericType 
            && typeof(DbEntityMapping<>).MakeGenericType
            (x.GetTypeInfo().BaseType.GenericTypeArguments[0]) == x.GetTypeInfo().BaseType;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var currentAssembly = OnAssemblyModelCreating();
            
            var entityMappingTypes = currentAssembly.GetTypes()
                                     .Where(OnEntityMappingModelCreatingPredicate());

            var modelBuilderEntityMethod = typeof(ModelBuilder).GetTypeInfo()
                                            .GetMethods()
                                            .Single
                                            (x=>
                                            x.Name == "Entity" &&
                                            x.IsGenericMethod  &&
                                            x.ReturnType.Name == "EntityTypeBuilder`1" 
                                             );
            
            foreach(var entityMappingType in entityMappingTypes)
            {
                var entityModelType = entityMappingType.GetTypeInfo().BaseType.GetGenericArguments().Single();

                var modelBuilderGenericEntityMethod = modelBuilderEntityMethod.MakeGenericMethod(entityModelType);

                var entityBuilder = modelBuilderGenericEntityMethod.Invoke(modelBuilder, null);

                var entityMappingInstance = Activator.CreateInstance(entityMappingType);

                var entityMappingConfigureMethod = entityMappingType.GetTypeInfo()
                                                    .GetMethod(nameof(DbEntityMapping<object>.Configure));

                entityMappingConfigureMethod.Invoke(entityMappingInstance, new[] { entityBuilder });
            }
            base.OnModelCreating(modelBuilder);
        } 
    }
}
