using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Extensions.EntityFrameWorkCore.MySql
{
    /// <summary>
    /// this class is implemented by domain to map to database properties
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DbEntityMapping<TEntity> where TEntity : class
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            //model builder needs to be updated if we have anything
        }
    }
}
