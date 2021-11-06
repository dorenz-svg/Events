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
    public class VisitorModelConfiguration : IEntityTypeConfiguration<Visitor>
    {
        public void Configure(EntityTypeBuilder<Visitor> builder)
        {
            builder.ToTable("Visitors");
            builder.HasKey(x=>x.Id);

            builder.Property(x => x.Id);
            builder.Property(x => x.EventId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.HasOne(x => x.User).WithMany(x=>x.Visitors).HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Event).WithMany(x => x.Visitors).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
