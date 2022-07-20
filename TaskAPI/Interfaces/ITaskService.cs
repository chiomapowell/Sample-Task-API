using ThreadTask = System.Threading.Tasks;
using AppTask = TaskAPI.Models.TaskModel;
using TaskAPI.Models;

namespace TaskAPI.Interfaces
{
    public interface ITaskService
    {
        public Task<int> AddTask(AppTask task);
        public Task<int> UpdateTask(AppTask task);
    }
}
