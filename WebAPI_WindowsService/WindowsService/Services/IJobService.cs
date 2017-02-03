using System.Collections.Generic;
using System.ServiceModel;

namespace WindowsService.Services
{
    public enum ToolType
    {
        XAF,
        XAS
    }

    public enum TaskType
    {
        Validate,
        Transform
    }

    [ServiceContract]
    public interface IJobService
    {
        [OperationContract]
        void StartJob(string workDir, TaskType taskType, ToolType toolType, Dictionary<string, string> parameters);
    }
}