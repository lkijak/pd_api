using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Models.Survey
{
    public class AnswerModel : BaseModel
    {
        [Required]
        public string Text { get; set; }
    }
}
