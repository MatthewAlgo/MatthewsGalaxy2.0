using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthewsGalaxy.WorkerMicroservice.Models
{
    public class Subscriber
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
    }
}
