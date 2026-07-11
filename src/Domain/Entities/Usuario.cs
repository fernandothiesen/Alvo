using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Domain.Exceptions;


namespace Domain.Entities
{
    public class Usuario
    {

        private readonly List<UsuarioRole> _roles = new();
        private readonly List<UsuarioPermissao> _permissoes = new();
        private readonly List<UsuarioContato> _contatos = new();
        public int IdUsuario {get; private set;}
        public string Nome {get; private set;}
        public string Email {get; private set;}
        public string SenhaHash {get; private set;}
        public bool Ativo {get; private set;}
        public DateTime DataCriacao {get; private set;}
        public DateTime? UltimoLogin {get; private set;}



        public IReadOnlyCollection<UsuarioRole> Roles => _roles.AsReadOnly();
        public IReadOnlyCollection<UsuarioPermissao> Permissoes => _permissoes.AsReadOnly();
        public IReadOnlyCollection<UsuarioContato> Contatos => _contatos.AsReadOnly();

        //construtor protegido para efCore
        protected Usuario(){}


        public Usuario(string nome, string email, string senhaHash)
        {
            ValidarNome(nome);
            ValidarEmail(email);
            ValidarEmail(senhaHash);


            
            Nome = nome.Trim();
            Email = email.Trim().ToLower();
            SenhaHash = senhaHash;
            Ativo = true;
            DataCriacao = DateTime.UtcNow;
        }

        public void AtualizarNome(string novoNome)
        {
            ValidarNome(novoNome);
            Nome = novoNome.Trim();
        }

        public void AtualizarSenha(string novaSenhaHash)
        {
            ValidarSenhaHash(novaSenhaHash);
            SenhaHash = novaSenhaHash;            
        }

        public void AtualizarEmail(string email)
        {
            ValidarEmail(email);
            Email = email.Trim();
        }


        public void Ativar()
        {
            Ativo = true;
        }

        public void Desativar()
        {
            Ativo = false;
        }

        public void RegistrarLogin()
        {
            UltimoLogin = DateTime.UtcNow;
        }


        //metodos de relacionamento 

        public void AdicionarRoleUsuario(UsuarioRole usuarioRole)
        {
            if(_roles.Any(r => r.IdRole == usuarioRole.IdRole))
                throw new DomainException("Usuario ja possui role");

            _roles.Add(usuarioRole);
        }

        public void RemoverRole(int idRole)
        {
            var role = _roles.FirstOrDefault(r => r.IdRole == idRole);

            if(role == null)
            {
                throw new DomainException("Role nao encontrada para este usuario");
            }

            _roles.Remove(role);
        }


        public void AdicionarPermissao(UsuarioPermissao usuarioPermissao)
        {
            if(_permissoes.Any(p => p.IdPermissao == usuarioPermissao.IdPermissao))
                throw new DomainException("Usuario ja possui essa permissao");

            _permissoes.Add(usuarioPermissao);
        }


        public void RemoverPermissao(int idPermissao)
        {
            var permissao = _permissoes.FirstOrDefault(p => p.IdPermissao == idPermissao);
            if(permissao == null)
                throw new DomainException("permisaao nao encontrada para esse usuario");

            _permissoes.Remove(permissao);
        }

        public void AdicionarContato(UsuarioContato usuarioContato)
        {
            _contatos.Add(usuarioContato);
        }

        public void RemoverContato(int idContato)
        {
            var contato = _contatos.FirstOrDefault(c => c.IdContato == idContato);
            if(contato == null)
                throw new DomainException("Esse contato nao existe");

            _contatos.Remove(contato);
        }

        private void ValidarNome(string nome)
        {
            if(string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome e obrigatorio");

            if(nome.Trim().Length < 3)
            {
                throw new DomainException("Nome deve ter no minimo 3 caracteres");
            }

            if(nome.Trim().Length > 100)
                throw new DomainException("Nome deve ter no maximo 100 caracteres");
        }


        private void ValidarEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email e obirgatorio");

            if(!email.Contains("@") || !email.Contains("."))
                throw new DomainException("Email invalido");

            if(email.Trim().Length > 150)
                throw new DomainException("Email deve ter no maximo 150m caracteres");
        }

        private void ValidarSenhaHash(string senha)
        {
            if(string.IsNullOrWhiteSpace(senha))
                throw new DomainException("Senha e obrigatorio");
        }
    }
}