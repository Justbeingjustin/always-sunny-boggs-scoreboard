using Boggs.Worker.GifCreators;
using Boggs.Worker.Models;
using Boggs.Worker.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Timers;

namespace Boggs.Worker
{
    public class BoggsWorkerService
    {
        private Timer _timer;
        private JobsRepository _jobsRepository;

        public void Start()
        {
            this._timer = new Timer();
            _jobsRepository = new JobsRepository(ConfigurationManager.AppSettings["API"]);
            _timer.Elapsed += new ElapsedEventHandler(Fetch);
            _timer.Interval = Convert.ToDouble(1000);
            _timer.Enabled = true;
            Fetch(null, null); //fire immediately
        }

        private void Fetch(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;
                var jobContainer = _jobsRepository.GetJobs();
                foreach (var job in jobContainer.data)
                {
                    try
                    {
                        var beerPlayers = JsonConvert.DeserializeObject<List<BeerPlayer>>(job.paramaters);
                        var gifCreator = new GifCreator(beerPlayers);
                        var result = gifCreator.CreateGif();
                        _jobsRepository.UpdateJob(new UpdateJobDTO()
                        {
                            jobId = job.jobid,
                            status = 1,
                            statusMessage = result
                        });
                    }
                    catch (Exception ex)
                    {
                        SetFailureStatus(ex.ToString(), job.jobid);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _timer.Enabled = true;
            }
        }

        private void SetFailureStatus(string ex, long jobId)
        {
            try
            {
                _jobsRepository.UpdateJob(new UpdateJobDTO()
                {
                    jobId = jobId,
                    status = -1,
                    statusMessage = "failed!"
                });
            }
            catch (Exception ex2)
            {
            }
        }

        public void Stop()
        {
            _timer.Enabled = false;
        }
    }
}