using System;
using System.Collections.Generic;

namespace pd_api.Models.DbModel
{
    public class UserTask : BaseModel
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<UserSubtask> Subtasks { get; set; }
    }

    public class UserSubtask : BaseModel
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }

        public int UserTaskId { get; set; }
        public UserTask UserTask { get; set; }
    }
}
