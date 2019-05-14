using Extensions.EntityFrameWorkCore.MsSql;
using Extensions.Security;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.DbContext
{
    public class PNoteServicesDbContext : MsSqlDbContextBase, IPNoteServicesDbContext
    {
        public PNoteServicesDbContext(DbContextOptions<PNoteServicesDbContext> options, 
            ICurrentUserAccessor currentUserAccessor) : base(options, currentUserAccessor)
        {

        }
    }
}
