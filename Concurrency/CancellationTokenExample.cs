namespace Concurrency
{
	internal class CancellationTokenExample
	{
		public async Task DownloadFileAsync(string url, CancellationToken token)
		{
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(url, token);
				response.EnsureSuccessStatusCode();
				string content = await response.Content.ReadAsStringAsync();
				Console.WriteLine("File downloaded.");
			}
		}
		public async Task Run()
		{
			CancellationTokenSource cts = new CancellationTokenSource();
			try
			{
				Task downloadTask = DownloadFileAsync("https://example.com/file", cts.Token);
				// Simulate user cancellation
				cts.CancelAfter(100);
				await downloadTask;
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Download cancelled.");
			}
		}
	}
}
