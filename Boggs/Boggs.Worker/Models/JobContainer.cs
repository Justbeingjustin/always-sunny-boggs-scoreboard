using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boggs.Worker.Models
{
    public class JobContainer
    {
        public string status { get; set; }
        public List<Job> data { get; set; }
        public string message { get; set; }
    }
}