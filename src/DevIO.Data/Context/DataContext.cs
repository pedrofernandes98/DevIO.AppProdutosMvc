using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DevIO.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Fornecedor> Fornecedores { get; set; }

        public DbSet<Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configura para caso uma propriedade do tipo string não seja manualmente configura receba o valor de varchar(100) ao invés de nvarchar(max) que é o valor configurado por padrão no Migration
            //EF Core (3x ou superior) property.Relational().ColumnType = "VARCHAR(100)"; => property.SetColumnType("VARCHAR(100)");
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                                        .SelectMany(e => e.GetProperties()
                                        .Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("VARCHAR(100)");
            }

            //Avalia os DbSets do assembly, no caso AppContext e as respectivas entidades que compõe estes DbSets
            //Procura no projeto uma classe que implemente a interface IEntityTypeConfiguration<T> para a entidade configurada no DbSet
            //Aplica as configurações definidas na classe encontrada
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            //Desabilita o delete CASCADE para todas as entidades do Banco
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}
