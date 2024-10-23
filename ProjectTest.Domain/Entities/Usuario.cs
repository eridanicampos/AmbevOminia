using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class Usuario : EntityGuid
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public virtual ICollection<AcessoUsuario>? AcessosUsuarios { get; set; }

        public override async Task<(bool isValid, List<string> messages)> Validate()
        {
            var isNameEmpty = string.IsNullOrEmpty(Nome);
            if (isNameEmpty)
            {
                MessagesToReturn.Add("Nome não pode ser vazio.");
                return new(false, MessagesToReturn);
            }
            var isEmailEmpty = string.IsNullOrEmpty(Email);
            if (isEmailEmpty)
            {
                MessagesToReturn.Add("E-mail não pode ser vazio.");
                return new(false, MessagesToReturn);
            }
            var isSenhaEmpty = string.IsNullOrEmpty(Senha);
            if (isSenhaEmpty)
            {
                MessagesToReturn.Add("Senha não pode ser vazia.");
                return new(false, MessagesToReturn);
            }


            return new(true, new());
        }
    }
}
