using System.Net;
using System.Net.NetworkInformation;

namespace Notes.Models
{
    public class Camera
    {
        public bool IsOnline { get; set; }
        public string? Address { get; set; }

        public Camera(string ip)    
        {
            Address = ip;
            IsOnline = false;
        }

        public Camera(string ip, string status)
        {
            Address = ip;
            IsOnline = status == IPStatus.Success.ToString();
        }
    }
}
