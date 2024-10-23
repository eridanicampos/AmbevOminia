using Microsoft.AspNetCore.Http;
using ProjectTest.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class CurrentUserInfo : ICurrentUserInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserInfo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            }
        }

        public string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "name")?.Value; 
            }
        }
    }
}
