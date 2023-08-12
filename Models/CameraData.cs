using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace Notes.Models
{
    public class CameraView: Camera
    {
        private readonly Color colorOnline = Color.Green;
        private readonly Color colorOffline = Color.Red;
        private const string textRUOnline = "Доступен";
        private const string textRUOffline = "Недоступен";
        public string? StatusRU { get; set; }    
       
        public Color Highlight { get; set; }
        private bool _isOnline;
        public new bool IsOnline { 
            get { return _isOnline; }
            set 
            { 
                Highlight = value ? colorOnline : colorOffline;
                StatusRU = value ? textRUOnline : textRUOffline;
                _isOnline = value;
            } 
        }
        public CameraView(string ip):base(ip)
        {
            StatusRU = textRUOffline;
        }

        public CameraView(string ip, string status) : base(ip, status)
        {
            //StatusRU = status == IPStatus.Success.ToString() ? textRUOnline : textRUOffline;
            IsOnline = status == IPStatus.Success.ToString();
        }
    }
}
