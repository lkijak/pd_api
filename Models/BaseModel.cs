using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int UserCreateId { get; set; }
        public int UserModifyId { get; set; }
    }
}
