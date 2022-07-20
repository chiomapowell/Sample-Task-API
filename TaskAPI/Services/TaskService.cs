using System.Collections;
using TaskAPI.Context;
using TaskAPI.Enums;
using TaskAPI.Interfaces;
using TaskAPI.Models;

namespace TaskAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskContext _context;
        private readonly ILogger<TaskService> _logger;   
        public TaskService(TaskContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddTask(TaskModel task)
        {
            try
            {
                var taskId = 0;

                await using (_context)
                {
                    //get conditional task count
                    var count = _context.Tasks.Where(t => t.Status != Status.Finished &&
                                                          t.Priority == Priority.High &&
                                                          t.DueDate == task.DueDate.Date).ToList().Count();

                    if (count >= 100) return taskId;
                    await _context.AddAsync(task);
                    await _context.SaveChangesAsync();

                    return task.Id;
                }
            }
            catch (Exception e)
            {
                //log
                _logger.LogError($"AddTask error: {e.StackTrace}");
                throw;
            }
        }

        public async Task<int> UpdateTask(TaskModel task)
        {
            var taskId = 0;

            try
            {
                await using (_context)
                {
                    _context.Update(task);
                   taskId = await _context.SaveChangesAsync();

                   return taskId;
                }

            }
            catch (Exception e)
            {
                //log
                _logger.LogError($"UpdateTask error on TaskId {task.Id}: {e.StackTrace}");
                throw;
            }
        }

        public async Task<TaskModel> RetrieveTaskById(int taskId)
        {
            //var taskId = 0;

            try
            {
                await using (_context)
                {
                    return _context.Tasks.Where(x => x.Id == taskId).FirstOrDefault();
                }

            }
            catch (Exception e)
            {
                //log
                _logger.LogError($"Select task by Id error on TaskId {taskId}: {e.StackTrace}");
                throw;
            }
        }
    }
}
