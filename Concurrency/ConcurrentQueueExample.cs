using System.Collections.Concurrent;

namespace Concurrency
{
	internal class ConcurrentQueueExample
	{
		public ConcurrentQueue<string> RunningJobsKeys = new ConcurrentQueue<string>();

		public void RunJobs()
		{
			while (true)
			{
				if (RunningJobsKeys.Count == 0)
				{
					var jobKey = ConsumeJob();
					RunningJobsKeys.Enqueue(jobKey);
					// do some actions
				}

				RunningJobsKeys.TryDequeue(out var value);
			}
		}

		private string ConsumeJob()
		{
			// logic to consume job key
			return "Random";
		}
	}
}
