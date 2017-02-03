using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class JobService : IJobService
    {
        public void StartJob(string workDir, TaskType taskType, ToolType toolType, Dictionary<string, string> parameters)
        {
            Task.Run(() =>
            {
                Debug.WriteLine($"Magic happens here: {workDir}");
                // Do work
                Directory.CreateDirectory(workDir);
                File.WriteAllText(Path.Combine(workDir, $"Progress {taskType} {toolType}.log"), $"Started {taskType} for {toolType} {DateTime.Now.ToShortTimeString()}");
                Thread.Sleep(30000);
                File.WriteAllText(Path.Combine(workDir, $"Progress {taskType} {toolType}.log"), $"Finished {DateTime.Now.ToShortTimeString()}");
            });
        }
    }
}