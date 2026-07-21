using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Cliente;

public class AtualizarClienteDTO
{
    [Required(ErrorMessage = "O nome do cliente é obrigatório")]
    [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres")]
    [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres")]
    public string Nome { get; set; } = string.Empty;

    public int? IdCidade { get; set; }
}