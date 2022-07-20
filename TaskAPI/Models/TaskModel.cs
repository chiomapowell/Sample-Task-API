using System.ComponentModel.DataAnnotations;
using TaskAPI.Enums;
using TaskAPI.Interfaces;

namespace TaskAPI.Models
{
    public class TaskModel : ITask
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }

    }
}
