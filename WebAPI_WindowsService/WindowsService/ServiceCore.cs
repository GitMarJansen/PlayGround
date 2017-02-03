using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using WindowsService.Properties;
using WindowsService.Services;

namespace WindowsService
{
    public partial class ServiceCore : ServiceBase
    {
        private ServiceHost _serviceHost;

        public ServiceCore()
        {
            InitializeComponent();
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists("ServiceCore"))
            {
                EventLog.CreateEventSource("ServiceCore", "ServiceLog");
            }
            eventLog1.Source = "ServiceCore";
            eventLog1.Log = "ServiceLog";
        }

        internal void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("WCF host is started, press <ENTER> key to stop");
            Console.ReadLine();
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            WriteToEventLog("ServiceCore is starting");
            try
            {
                Uri baseAddress = new Uri(Settings.Default.ServiceUri);
                _serviceHost = new ServiceHost(typeof(JobService), baseAddress);
                _serviceHost.AddServiceEndpoint(typeof(IJobService), new NetNamedPipeBinding(), Settings.Default.ServiceAddress);
                _serviceHost.Open();
                WriteToEventLog("ServiceCore is started");
            }
            catch (Exception ex)
            {
                WriteToEventLog($"ServiceCore failed started {ex.Message}");
            }
        }

        protected override void OnStop()
        {
            try
            {
                WriteToEventLog("ServiceCore is stopping");
                _serviceHost.Close();
                _serviceHost = null;
                WriteToEventLog("ServiceCore is stopped");
            }
            catch (Exception ex)
            {
                WriteToEventLog(ex);
            }
        }

        private void WriteToEventLog(string message)
        {
            eventLog1.WriteEntry(message);
        }

        private void WriteToEventLog(Exception ex)
        {
            eventLog1.WriteEntry($"Error while stopping the service: {ex.Message}", EventLogEntryType.Error);
        }
    }
}