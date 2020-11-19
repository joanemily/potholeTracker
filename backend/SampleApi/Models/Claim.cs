using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.Models
{
    public class Claim
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Cost { get; set; }
        public DateTime DateOfAccident { get; set; }
        public string Phone { get; set; }
        public int PotholeId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime DateFiled { get; set; }

    }
}
