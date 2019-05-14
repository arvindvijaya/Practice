using Extensions.EntityFrameWorkCore.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleApplication.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Mapping
{
    public class ApplicationUserMap : DbEntityMapping<ApplicationUser>
    {
        public override void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("application_user");

            builder.HasKey(x => x.User_Id);

            base.Configure(builder);
        }
    }
}
