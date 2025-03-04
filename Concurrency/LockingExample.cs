namespace Concurrency
{
	internal static class LockingExample
	{
		public class Counter
		{
			private int _count = 0;

			public void Increment()
			{
				_count++;
			}

			public int GetValue() => _count;
		}

		public static void Run()
		{
			var counter = new Counter();
			var tasks = new List<Task>();

			for (int i = 0; i < 1000; i++)
			{
				tasks.Add(Task.Run(counter.Increment));
			}

			Task.WaitAll(tasks.ToArray());
			Console.WriteLine($"Final count: {counter.GetValue()}");
		}

		// Using the lock
		public class CounterWithLock
		{
			private int _count = 0;
			private readonly object _lock = new object();

			public void Increment()
			{
				lock (_lock)
				{
					_count++;
				}
			}

			public int GetValue() => _count;
		}

		public static void RunWithLock()
		{
			var counter = new CounterWithLock();
			var tasks = new List<Task>();

			for (int i = 0; i < 1000; i++)
			{
				tasks.Add(Task.Run(counter.Increment));
			}

			Task.WaitAll(tasks.ToArray());
			Console.WriteLine($"Final count: {counter.GetValue()}");
		}
	}
}
