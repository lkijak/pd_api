using System;
using System.ComponentModel.DataAnnotations;

namespace pd_api.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        [Required]
        public int UserCreateId { get; set; }
        public int? UserModifyId { get; set; }
    }
}
