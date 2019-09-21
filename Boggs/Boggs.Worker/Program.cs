using Topshelf;

namespace Boggs.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(configure =>
            {
                configure.Service<BoggsWorkerService>(service =>
                {
                    service.ConstructUsing(s => new BoggsWorkerService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.
                configure.RunAsLocalSystem();
                configure.SetServiceName("BoggsWorkerService");
                configure.SetDisplayName("BoggsWorkerService");
                configure.SetDescription("BoggsWorkerService");
            });
        }
    }
}