using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO
{
    public class EntityDTO
    {
        public string CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string UpdatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteAt { get; set; }
        public string DeletedByUserId { get; set; }
    }
}
