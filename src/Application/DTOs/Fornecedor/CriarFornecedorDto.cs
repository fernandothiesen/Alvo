using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Fornecedor;


public class CriarFornecedorDTO
{
    [Required(ErrorMessage = "O nome do fornecedor eh obrigatorio")]
    [MaxLength(150, ErrorMessage = "O nome deve ter no maximo 150 caracteres")]
    [MinLength(3, ErrorMessage = "O nome deve ter no minimo 3 caracteres")]
    public string Nome {get; set;} = string.Empty;


    public int? IdCidade {get; set;}


}