using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public abstract class Entity
    {
        public const bool IS_VALID = true;
        public const bool IS_NOT_VALID = false;
        public const bool ERROR_BUSINESS = false;
        public List<string> MessagesToReturn = new List<string>();

        public string CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string UpdatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteAt { get; set; }
        public string DeletedByUserId { get; set; }

        public abstract Task<(bool isValid, List<string> messages)> Validate();
    }
}
