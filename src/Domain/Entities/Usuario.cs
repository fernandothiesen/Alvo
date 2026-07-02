using System;
using System.Text.RegularExpressions;
namespace Entities
{
    public class Usuario
    {
        public int Id_usuario {get; private set;}
        public string Nome {get; private set;}
        public string Email {get; private set;}
        public string Senha_hash {get; private set;}
        public bool Ativo {get; private set;}
        public DateTime Data_Criacao {get; private set;}
        public DateTime? Ultimo_login {get; private set;}


        protected Usuario(){}


        public Usuario(string nome, string email, string senhaHash)
        {
            if(string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obirgatório");
            if(string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email é obirgatório");

            
            Nome = nome;
            Email = email;
            Senha_hash = senhaHash;
            Ativo = true;
            Data_Criacao = DateTime.UtcNow;
        }

        public void AtualizarUltimoLogin()
        {
            Ultimo_login = DateTime.UtcNow;
        }

        public void Desativar()
        {
            Ativo = false;
        }

        public void Ativar()
        {
            Ativo = true;
        }


        public bool ValidarEmail(string email)
        {
            string email_padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, email_padrao);
        }
    }
}