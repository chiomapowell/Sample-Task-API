using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskAPI.Context;
using TaskAPI.Enums;
using TaskAPI.Models;
using TaskAPI.Services;
using Xunit;

namespace TaskAPITest
{
    public class TaskTest
    {
        private DbContextOptions<TaskContext> _dbOptions;
        private ILogger<TaskService> _logger;
        private TaskContext _context;
        public TaskTest()
        {
            var dbName = $"TaskTestdb_{DateTime.Now}";
            _dbOptions = new DbContextOptionsBuilder<TaskContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _context = new TaskContext(_dbOptions);
            Seed(_context);
        }

        private static void Seed(TaskContext context)
        {
            List<TaskModel> taskList = new();

            for (int i = 0; i < 110; i++)
            {
                taskList.Add(new TaskModel
                {
                    Description = $"Unit Test {i}",
                    DueDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date,
                    StartDate = DateTime.Now.Date,
                    Name = $"Task Test {i}",
                    Priority = Priority.High,
                    Status = Status.New
                });
            }

            context.Tasks.AddRange(taskList);
            context.SaveChanges();

        }

        /// <summary>
        /// This test evaluates adding a Task successfully
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InsertTaskPositive()
        {
            var service = CreateTaskSerice();

            var result = Task.Run(() => service.AddTask(new TaskModel()
            {
                Description = "Unit Test",
                DueDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date,
                StartDate = DateTime.Now.Date,
                Name = "Task Test",
                Priority = Priority.High,
                Status = Status.Finished
            }));

            Assert.NotNull(result);
            Assert.True(condition: result.Result > 0);
        }

        /// <summary>
        /// This test  evaluates if a task is being added that does not meet the requirments provided 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InsertTaskNegative()
        {
            var service = CreateTaskSerice();

            var result = Task.Run(() => service.AddTask(new TaskModel()
            {
                Description = "Unit Test",
                DueDate = DateTime.Now,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now,
                Name = "Task Test",
                Priority = Priority.High,
                Status = Status.New
            }));

            Assert.NotNull(result);
            Assert.True(condition: result.Result == 0);
        }

        [Fact]
        public async Task UpdateTask()
        {
            var timeStamp = DateTime.Now;
            var service = CreateTaskSerice();

            var task = await _context.Tasks.FirstOrDefaultAsync();

            var timeStampname = $"Update Task_{timeStamp}";

            task.Name = timeStampname;
                
            var taskId = task.Id;

            var result = await service.UpdateTask(task);
            //retrive updated task
            var dbTask = await service.RetrieveTaskById(result);

            Assert.NotNull(result);
            Assert.True(condition: dbTask.Name == timeStampname);
        }



        private TaskService CreateTaskSerice()
        {
            return new TaskService(_context, _logger);
        }
    }
}