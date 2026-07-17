using Application.DTOs.Usuario;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Application.DTOs.Role;
using Application.DTOs.Permissao;



namespace Application.Services;



public class UsuarioService : IUsuarioService
{
    
    private readonly IUsuario _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;  
    public UsuarioService(IUsuario usuarioRepository, IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResponseResult> CriarUsuarioAsync(CriarUsuarioDto dto)
    {
        try
        {
            if(await _usuarioRepository.EmailExisteAsync(dto.Email))
                return ResponseResult.Erro("Ja existe um Usuario com este email");

            //(usando a interface, mas infrastructure vai usar BCrypt)
            var senhaHash = _passwordHasher.HashPassword(dto.Senha);

            var usuario = new Usuario(dto.Nome, dto.Email, senhaHash);

            await _usuarioRepository.AdicionarAsync(usuario);


            var todasAsRoles = new List<int>();


            if(dto.IdRole.HasValue)
            {
                todasAsRoles.Add(dto.IdRole.Value);
            }

            todasAsRoles.AddRange(dto.IdsRoles);
            todasAsRoles = todasAsRoles.Distinct().ToList();


            if(todasAsRoles.Any())
            {
                var novasRoles = todasAsRoles
                    .Select(idRole => new UsuarioRole(usuario.IdUsuario, idRole))
                    .ToList();

                usuario.DefinirRole(novasRoles);
                await _usuarioRepository.AtualizarRolesAsync(usuario, novasRoles);
            }

            return ResponseResult.Sucesso("Usuario criado com sucesso!", new {Id = usuario.IdUsuario});
        }catch(Exception ex)
        {
            return ResponseResult.Erro($"Erro ao criar usuario: {ex.Message}");
        }
    }


    public async Task<ResponseResult> AtualizarAsync(int id, CriarUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if(usuario == null)
            {
                return ResponseResult.Erro("Usuario nao encontrado");
            }


            //att dados basicos 

            usuario.AtualizarNome(dto.Nome);
            usuario.AtualizarEmail(dto.Email);
            
            if(!string.IsNullOrEmpty(dto.Senha))
            {
                var senhaNovaHash = _passwordHasher.HashPassword(dto.Senha);
                usuario.AtualizarSenha(senhaNovaHash);
            }


            await _usuarioRepository.AtualizarAsync(usuario);


            var todasAsRoles = new List<int>();

            if(dto.IdRole.HasValue)
            {
                todasAsRoles.Add(dto.IdRole.Value);
            }

            todasAsRoles.AddRange(dto.IdsRoles);
            todasAsRoles = todasAsRoles.Distinct().ToList();

            var novasRoles = todasAsRoles
                .Select(idRole => new UsuarioRole(usuario.IdUsuario, idRole))
                .ToList();

            usuario.DefinirRole(novasRoles);
            await _usuarioRepository.AtualizarRolesAsync(usuario, novasRoles);

            return ResponseResult.Sucesso("Usuario atualizado com sucesso!");
        }catch(DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
    }


    public async Task<ResponseResult> DesativarAsync(int id)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id);

        if(usuario == null)
            return ResponseResult.Erro("Esse usuario nao existe");


        usuario.Desativar();
        await _usuarioRepository.AtualizarAsync(usuario);

        return ResponseResult.Sucesso("Usuario desativado com sucesso");
    }



    public async Task<UsuarioDto?> ObterPorIdAsync(int id)
    {
        var usuario = await _usuarioRepository.ObterPorIdComRolesAsync(id);   // trocado de ObterPorIdAsync

        if(usuario == null)
            return null;

        var permissoes = await _usuarioRepository.ObterPermissoesPorUsuarioAsync(id);

        return new UsuarioDto
        {
            IdUsuario = usuario.IdUsuario,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Ativo = usuario.Ativo,
            DataCriacao = usuario.DataCriacao,
            UltimoLogin = usuario.UltimoLogin,
            Roles = usuario.Roles.Select(r => r.Role!.NomeRole).ToList(),
            Permissoes = permissoes.ToList()
        };
    }


    public async Task<IEnumerable<UsuarioDto>> ObterTodosAsync()
    {
        var usuarios = await _usuarioRepository.ObterTodosAsync();

        var resultado = new List<UsuarioDto>();
        foreach (var u in usuarios)
        {
            var permissoes = await _usuarioRepository.ObterPermissoesPorUsuarioAsync(u.IdUsuario);
            resultado.Add(new UsuarioDto
            {
                IdUsuario = u.IdUsuario,
                Nome = u.Nome,
                Email = u.Email,
                Ativo = u.Ativo,
                DataCriacao = u.DataCriacao,
                UltimoLogin = u.UltimoLogin,
                Roles = u.Roles.Select(r => r.Role!.NomeRole).ToList(),
                Permissoes = permissoes.ToList()
            });
        }

        return resultado;
    }


    public async Task<ResponseResult> AtualizarRolesAsync(int idUsuario, AtualizarRoleDto dto)
    {
        try
        {
            var usuario = await _usuarioRepository.ObterPorIdComRolesAsync(idUsuario);
            if(usuario == null)
                return ResponseResult.Erro("Usuario nao encontrado");

            var novasRoles = new List<UsuarioRole>();

            foreach(var idRole in dto.IdsRoles)   // dto.IdsRoles já vem inicializado com new(), não precisa checar null
            {
                novasRoles.Add(new UsuarioRole(idUsuario, idRole));   
            }

            usuario.DefinirRole(novasRoles);
            await _usuarioRepository.AtualizarRolesAsync(usuario, novasRoles);

            return ResponseResult.Sucesso("Roles do usuario atualizada com sucesso");
        }
        catch(DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch(Exception ex)
        {
            return ResponseResult.Erro($"Erro ao atualizar roles: {ex.Message}");
        }
    }


    public async Task<ResponseResult> AtualizarPermissoesAsync(int idUsuario, AtualizarPermissaoDto dto)
    {
        try
        {
            var usuario = await _usuarioRepository.ObterPorIdComRolesAsync(idUsuario);

            if(usuario == null)
            {
                return ResponseResult.Erro("Usuario nao encontrado");
            }


            var novasPermissoes = dto.IdsPermissoes
                .Select(idPermissao => new UsuarioPermissao(idUsuario, idPermissao))
                .ToList();

            usuario.DefinirPermissao(novasPermissoes);
            await _usuarioRepository.AtualizarPermissoesAsync(usuario, novasPermissoes);

            return ResponseResult.Sucesso("Permissoes do usuario atualizadas com sucesso");
        }catch(DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
    }

}