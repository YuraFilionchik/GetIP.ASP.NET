using System.Net;

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
    }
}
