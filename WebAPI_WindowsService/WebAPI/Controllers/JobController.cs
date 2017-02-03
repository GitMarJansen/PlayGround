using System;
using System.ServiceModel;
using System.Threading;
using System.Web.Http;
using WebAPI.JobService;
using WebAPI.Properties;

namespace WebAPI.Controllers
{
    [RoutePrefix("ABBA/jobs")]
    public class JobController : ApiController
    {
        [HttpPost]
        [Route("Work")]
        public IHttpActionResult DoWork()
        {
            try
            {
                var jobService = new JobServiceClient(new NetNamedPipeBinding(), new EndpointAddress(Settings.Default.ServiceUri));
                for (int i = 0; i < 10; i++)
                {
                    jobService.StartJob($@"d:\test\workdir\{DateTime.Now.Ticks}", TaskType.Transform, ToolType.XAF, null);
                    Thread.Sleep(500);
                }
                return Ok();
            }
            catch (EndpointNotFoundException ex)
            {
                // Failed to queue work
                return InternalServerError(ex);
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}