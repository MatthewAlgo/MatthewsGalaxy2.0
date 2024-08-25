using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthewsGalaxy.WorkerMicroservice.Models
{
    public class EmailRequest
    {
        public string DestinationEmail { get; set; }
        public string? SourceEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
