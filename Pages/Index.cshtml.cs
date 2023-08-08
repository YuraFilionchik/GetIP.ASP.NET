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
        const string formName = "IP checker";
        const string statusOnline = " = Online";
        const string statusOffline = " = Offline";
        const string textNotFound = "No matches found";
        Color colorOnline = Color.Green;
        Color colorOffline = Color.Red;
        const string searchTemplate = @"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b";
        int counter = 1;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public IList<Camera> Cameras { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            scanner.ScannerEvent += new EventHandler<ScannerEventArgs>(scanner_ScannerEvent);

        }

        public async Task OnPostAsync()
        {
            counter = 1;
            Regex regex = new Regex(searchTemplate);
            if (String.IsNullOrEmpty(SearchString))
            {
                return;
            }

            MatchCollection matches = regex.Matches(SearchString);
            List<string> ipList = new List<string>();
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    ipList.Add(match.Value);
                }
            }
            else
            {
                //textNotFound;
            }
            if (ipList.Count == 0) return;
            
            scanner.StopPings();
            await scanner.ScanParallel(ipList.ToArray());
            Cameras = Cameras.OrderBy(x => x.IsOnline).ToList();

        }

        private async Task WaitPinging(Scanner scanner)
        {
            Thread.Sleep(300);
            
        }

        
        //result of ping
        void scanner_ScannerEvent(object sender, ScannerEventArgs e)
        {
                DisplayIP(e.Address, e.Status);
        }

        private void DisplayIP(IPAddress address, IPStatus status)
        {
            string ip = address.ToString();
            string addText;
            Color highlight;
            if (status == IPStatus.Success)
            {
                addText = statusOnline;
                highlight = colorOnline;
            }
            else
            {
                addText = statusOffline;
                highlight = colorOffline;
            }
            if (Cameras == null) Cameras = new List<Camera>();
            if (Cameras.Count( x => x.Address == ip) > 0)
            {
                Cameras.First(x=>x.Address == ip).IsOnline = status == IPStatus.Success;
            }
            else
            {
                Cameras.Add(new Camera(ip, status.ToString())) ;
            }
            //for (int i = 0; i < tbOutput.Lines.Count(); i++)
            //{
            //    if (tbOutput.Lines[i].Split(' ')[0] == ip)
            //    {
            //        int lineStartIndex = tbOutput.GetFirstCharIndexFromLine(i);
            //        int lineLength = tbOutput.Lines[i].Length;
            //        tbOutput.SelectionStart = lineStartIndex;
            //        tbOutput.SelectionLength = lineLength;
            //        string oldText = tbOutput.Lines[i].ToString();

            //        if (!oldText.Contains(addText))
            //        {
            //            string newText = oldText + addText;
            //            tbOutput.SelectedText = newText;
            //            tbOutput.Find(newText);
            //        }

            //        tbOutput.SelectionBackColor = highlight;
            //        tbOutput.DeselectAll();
            //    }
            //}

        }

        //public void OnGet()
        //{
            
        //}


    }
}