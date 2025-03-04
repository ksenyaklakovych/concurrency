namespace Concurrency
{
	internal class AsyncAwaitExample
	{
		public void ReadFilesSync(List<string> filePaths)
		{
			foreach (var path in filePaths)
			{
				var content = File.ReadAllText(path);
				Console.WriteLine($"Read {path}: {content.Length} bytes");
			}
		}

		public async Task ReadFilesAsync(List<string> filePaths)
		{
			var tasks = filePaths.Select(async path =>
			{
				var content = await File.ReadAllTextAsync(path);
				Console.WriteLine($"Read {path}: {content.Length} bytes");
			});

			await Task.WhenAll(tasks);
		}
	}
}
