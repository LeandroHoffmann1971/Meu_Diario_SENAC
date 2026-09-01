using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeuDiarioSENAC.Data;

[Table("nota")]
public class Nota
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public DateTime Data { get; set; } = DateTime.Now;

    [Required]
    [Column(TypeName = "text")]
    public string Conteudo { get; set; } = string.Empty;

    public Nota()
    {
    }

    public Nota(string titulo, string conteudo)
    {
        Titulo = titulo;
        Conteudo = conteudo;
        Data = DateTime.Now;
    }
}
