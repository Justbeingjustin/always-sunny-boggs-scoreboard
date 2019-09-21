using System;

namespace Boggs.Worker.Models
{
    public class Job
    {
        public int jobid { get; set; }
        public string paramaters { get; set; }
        public int status { get; set; }
        public string statusmessage { get; set; }
        public DateTime createddate { get; set; }
    }
}