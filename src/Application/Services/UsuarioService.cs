using Application.DTOs.Usuario;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;



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
            if(await _usuarioRepository.EmailExistenteAsync(dto.Email))
                return ResponseResult.Erro("Ja existe um Usuario com este email");

            //(usando a interface, mas infrastructure vai usar BCrypt)
            var senhaHash = _passwordHasher.HashPassword(dto.Senha);

            var usuario = new Usuario(dto.Nome, dto.Email, senhaHash);

            await _usuarioRepository.AdicionarAsync(usuario);

            return ResponseResult.Sucesso("Usuario criado com sucesso!", new {Id = usuario.Id_usuario});
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
            usuario.At



        }


    }


}