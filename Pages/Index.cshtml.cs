using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Notes.Controller;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net;
using System.Text.RegularExpressions;
using Notes.Models;
using System.Collections;

namespace Notes.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        Scanner scanner = new Scanner();
        const string searchTemplate = @"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b";

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public IList<CameraView> Cameras { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            scanner.ScannerEvent += new EventHandler<ScannerEventArgs>(scanner_ScannerEvent);

        }

        public async Task OnPostAsync()
        {
            Regex regex = new Regex(searchTemplate);
            if (String.IsNullOrEmpty(SearchString))
            {
                return;
            }

            MatchCollection matches = regex.Matches(SearchString);
            List<string> ipList = new List<string>();
            if (matches.Count > 0)
            {
                foreach (Match match in matches.Cast<Match>())
                {
                    ipList.Add(match.Value);
                }
            }
            else
            {
                Cameras = new List<CameraView>();
                return;
            }

            await scanner.ScanParallel(ipList.ToArray());
            Cameras = Cameras.OrderBy(x => x.IsOnline).ToList();

        }

        //result of ping
        void scanner_ScannerEvent(object sender, ScannerEventArgs e)
        {
            DisplayIP(e.Address, e.Status);
        }

        private void DisplayIP(IPAddress address, IPStatus status)
        {
            string ip = address.ToString();
            Cameras ??= new List<CameraView>();
            lock (Cameras)
            {
                if (Cameras.Any(x => x.Address == ip))
                {
                    var cam = Cameras.First(x => x.Address == ip);
                    cam.IsOnline = status == IPStatus.Success;
                }
                else
                {
                    Cameras.Add(new CameraView(ip, status.ToString()));
                }
            }
        }
    }
}