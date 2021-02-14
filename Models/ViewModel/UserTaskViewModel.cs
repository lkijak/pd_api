using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.ViewModel
{
    public class UserTaskViewModel
    {
        public int Taskid { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [Required]
        public string UserName { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public bool IsArchiwed { get; set; }

        public IList<UserSubtaskViewModel> UserSubtasks { get; set; }
    }

    public class UserSubtaskViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsCompleted { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
    }
}
