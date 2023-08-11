using System.Net.NetworkInformation;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Controller
{
     public class Scanner
    {
        public Task? Task { get; set; }
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;
        public event EventHandler<ScannerEventArgs> ScannerEvent;

        public Task ScanParallel(string[] addresses)
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
            //task = Task.Factory.StartNew(() => {
            //    ParallelOptions options = new ParallelOptions()
            //    {
            //        MaxDegreeOfParallelism = addresses.Length
            //    };
            //    Parallel.ForEach(addresses, options, address => {
            //        try
            //        {
            //            Ping ping = new Ping();
            //            PingReply reply = ping.Send(address, 1300);
            //            OnScannerEvent(new ScannerEventArgs(reply.Address, reply.Status));
            //        }
            //        catch (Exception) { }
            //    });
            //}, token);
            List<Task> tasks = new List<Task>();
            foreach(string address in addresses)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    Ping ping = new Ping();
                    PingReply reply = ping.Send(address, 1300);
                    OnScannerEvent(new ScannerEventArgs(reply.Address, reply.Status));
                }, token));
                
            }
            return Task.WhenAll(tasks);
        }

        public bool InProcess()
        {
            if (Task == null || Task.IsCompleted)  
            {
                    return false;
            }

            return true; 
        }
        public void StopPings()
        {
            if (Task != null)
            {
                cancelTokenSource.Cancel();
                cancelTokenSource.Dispose();
            }
        }

        protected virtual void OnScannerEvent(ScannerEventArgs e)
        {
            if (ScannerEvent != null)
            {
                ScannerEvent(this, e);
            }
        }
    }
    public class ScannerEventArgs : EventArgs
    {
        public readonly IPAddress Address;
        public readonly IPStatus Status;
        public ScannerEventArgs(IPAddress address, IPStatus status)
        {
            Address = address;
            Status = status;
        }
    }
}
