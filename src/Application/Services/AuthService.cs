using Application.DTOs.Auth;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.DTOs.Usuario;
using Domain.Entities;
using Domain.Interfaces;



public class AuthService : IAuthService
{
    private readonly IUsuario _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;


    public AuthService(IUsuario usuarioRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseResult> LoginAsync(LoginDto loginDto)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(loginDto.Email);
        if(usuario == null)
            return ResponseResult.Erro("Email ou senha invalidos");
        
        if(!usuario.Ativo)
            return ResponseResult.Erro("Usuario desativado. Entre em contato com o administrador");

        if(!_passwordHasher.VerifyPassword(loginDto.Senha, usuario.SenhaHash))
            return ResponseResult.Erro("Email ou senha invalidos");

        usuario.RegistrarLogin();
        await _usuarioRepository.AtualizarAsync(usuario);

        //roles e permissoes para o token 

        var roles = usuario.Roles.Select(r => r.Role!.NomeRole).ToList();


        //Simplificando roles por enquanto, depois adicionar metodo no repository para pegar permissoes

        var permissoes = new List<string>();
        foreach(var role in usuario.Roles)
        {
            if(role.Role != null)
            {
                permissoes.AddRange(role.Role.Permissoes.Select(p => p.Permissao!.NomePermissao));
            }
        }

        //gera token jwt 
        var token = _tokenGenerator.GenerateToken(usuario, roles, permissoes.Distinct().ToList());

        var response = new LoginResponseDto
        {
            Token = token,
            Expiracao = DateTime.UtcNow.AddHours(2),
            Usuario = new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Roles = roles
            }
        };

        return ResponseResult.Sucesso("Login Realizado com sucesso!", response);

    }


}