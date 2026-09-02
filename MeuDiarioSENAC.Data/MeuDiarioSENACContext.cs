using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using MeuDiarioSENAC.Model;

namespace MeuDiarioSENAC.Data;

public class MeuDiarioSENACContext : DbContext
{
    public DbSet<Nota> Notas { get; set; }

    public string ConnectionString { get; } = "Server=localhost;Database=anotacoes;Uid=root;Pwd=1234;";

    public void GarantirBancoETabela()
    {
        try
        {
            using (var connection = new MySqlConnection("Server=localhost;Uid=root;Pwd=1234;"))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "CREATE DATABASE IF NOT EXISTS anotacoes;";
                    cmd.ExecuteNonQuery();
                }
            }

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS nota (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Titulo VARCHAR(200) NOT NULL,
                            Data DATETIME NOT NULL,
                            Conteudo TEXT NOT NULL
                        );";

                    cmd.ExecuteNonQuery();
                }
            }

            Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao preparar o banco de dados: {ex.Message}");
            throw;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Nota>(entity =>
        {
            entity.ToTable("nota");
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(n => n.Conteudo).IsRequired();
            entity.Property(n => n.Data).IsRequired();
        });
    }
}