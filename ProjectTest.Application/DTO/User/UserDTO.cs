using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO.User
{
    public class UserDTO : EntityPKDTO
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string tipo_usuario { get; set; }
    }
}
