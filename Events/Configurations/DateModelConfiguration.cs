using Events.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Models.Configurations
{
    public class DateModelConfiguration : IEntityTypeConfiguration<Date>
    {
        public void Configure(EntityTypeBuilder<Date> builder)
        {
            builder.ToTable("Dates");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.EventId).IsRequired();
            builder.Property(x => x.DateBegin).IsRequired();
            builder.Property(x => x.DateEnd);

            builder.HasOne(x => x.Event).WithMany(x => x.Dates).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(x => x.User).WithMany(x => x.Dates).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
