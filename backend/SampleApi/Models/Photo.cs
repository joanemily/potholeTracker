using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.Models
{
    public class Photo
    {

        public int Id { get; set; }
        public string Path { get; set; }
        public int PotholeId { get; set; }
        public DateTime DateUploaded { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}
