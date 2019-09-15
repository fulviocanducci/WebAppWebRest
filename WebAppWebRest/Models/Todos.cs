using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace WebAppWebRest.Models
{
    public class Todos
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool Done { get; set; }
    }

    public class TodosConfiguration : IEntityTypeConfiguration<Todos>
    {
        public void Configure(EntityTypeBuilder<Todos> builder)
        {
            builder.ToTable("todos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Done)
                .HasColumnName("done")                
                .IsRequired();
        }
    }
}
