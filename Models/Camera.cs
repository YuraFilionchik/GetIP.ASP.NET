using System.Net;
using System.Net.NetworkInformation;

namespace Notes.Models
{
    public class Camera
    {
        public bool IsOnline { get; set; }
        public string? Address { get; set; }
        public enum Status
        {
            Online = 0,
            Offline = 1
        }

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


        public override string ToString()
        {
            return IsOnline ? Status.Online.ToString() : Status.Offline.ToString();
        }

    }
}
