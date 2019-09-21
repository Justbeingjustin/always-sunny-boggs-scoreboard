namespace Boggs.Worker.Models
{
    public class UpdateJobDTO
    {
        public long jobId { get; set; }
        public int status { get; set; }
        public string statusMessage { get; set; }
    }
}