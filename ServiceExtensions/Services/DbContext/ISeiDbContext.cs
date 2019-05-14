using Extensions.EntityFrameWorkCore.MsSql;
using Extensions.EntityFrameWorkCore.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.DbContext
{
    public interface IPNoteServicesDbContext : IDbContextMsSql
    {
    }
}
