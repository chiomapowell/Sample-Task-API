using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Interfaces;
using TaskAPI.Models;

namespace TaskAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private ITaskService _taskService;
    private ILogger<TaskController> _logger;

    public TaskController(ITaskService service, ILogger<TaskController> logger)
    {
        _taskService = service;
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="task"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<TaskResponse> AddTaskAsync(TaskModel task, CancellationToken token)
    {
        TaskResponse response = new();

        try
        {
            var taskId = await _taskService.AddTask(task);

            if (taskId != 0)
            {
                response.Id = task.Id;
                response.Message = "Successful";
            }
            else
            {
                response.Id = 0;
                response.Message = "Unable to add task. Please contact Admin";
            }

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            response.Message = $"Task: {task.Name} failed to add. Exception: {e.StackTrace}";
            //return response;
        }

        return response;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="task"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<TaskResponse> UpdateTaskAsync(TaskModel task, CancellationToken token)
    {
        TaskResponse response = new();

        try
        {
            var taskId = await _taskService.UpdateTask(task);

            if (taskId > 0)
            {
                response.Message = "Successful";
            }

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            response.Message = $"Task: {task.Id} failed to Update";
            //return response;
        }

        return response;
    }
}