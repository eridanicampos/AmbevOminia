using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO.User
{
    public class UserAuthenticateResponseViewModel
    {
        public UserAuthenticateResponseViewModel(UserDTO user, string token)
        {
            User = user;
            Token = token;
        }

        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
