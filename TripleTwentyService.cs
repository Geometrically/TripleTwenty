using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace tripletwenty
{
    public class TripleTwentyService : ServiceBase
    {
        public TripleTwentyService()
        {
            ServiceName = "Triple Twenty Service";
            CanStop = true;
            CanPauseAndContinue = false;
            AutoLog = false;
        }

        private BackgroundWorker _worker;
        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Service entered onStart.", EventLogEntryType.Warning);
            base.OnStart(args);

            if (_worker != null && _worker.IsBusy)
            {
                EventLog.WriteEntry("Service: Worker already running.", EventLogEntryType.Warning);
                return;
            }

            _worker = new BackgroundWorker();
            _worker.DoWork += (sender, eventArgs) =>
            {
                EventLog.WriteEntry("Worker do work entered", EventLogEntryType.Information);

                var i = 0;
                while (!_worker.CancellationPending)
                {
                    EventLog.WriteEntry("Worker working " + i++, EventLogEntryType.Information);
                    Thread.Sleep(3000);
                }

                EventLog.WriteEntry("Worker exit.", EventLogEntryType.Information);
            };

            _worker.WorkerSupportsCancellation = true;
            _worker.WorkerReportsProgress = false;
            _worker.RunWorkerAsync();
            EventLog.WriteEntry("Service: Worker started", EventLogEntryType.Information);

        }

        protected override void OnStop()
        {
            base.OnStop();

            if (!_worker.IsBusy) return;

            EventLog.WriteEntry("Service: Worker cancelling ...", EventLogEntryType.Information);
            _worker.CancelAsync();
            Thread.Sleep(3000);

            EventLog.WriteEntry("Service: Worker dismissed ...", EventLogEntryType.Information);
            _worker = null;
        }
    }
}