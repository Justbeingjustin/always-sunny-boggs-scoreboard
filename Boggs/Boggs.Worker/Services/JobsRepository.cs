using Boggs.Worker.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace Boggs.Worker.Services
{
    public class JobsRepository
    {
        private readonly string _baseUrl;

        public JobsRepository(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public JobContainer GetJobs()
        {
            var client = new RestClient(_baseUrl + "/api/jobs");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<JobContainer>(response.Content);
        }

        public void UpdateJob(UpdateJobDTO updateJobDto)
        {
            var client = new RestClient(_baseUrl + "/api/job");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(updateJobDto), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
    }
}