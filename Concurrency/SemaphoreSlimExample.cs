namespace Concurrency
{
	internal class NoSemaphoreSlimExample
	{
		public static async Task<Settings> GetSettings()
		{
			if (CacheExists())
			{
				return GetCachedValue();
			}

			var settings = await GetSettingsFromDB();
			CacheValue(settings);
			return settings;
		}

		private static bool CacheExists()
		{
			// logic to get cache
			return false;
		}

		private static Settings GetCachedValue()
		{
			// logic to get cache
			return new Settings();
		}

		private static async Task<Settings> GetSettingsFromDB()
		{
			// logic to get settings from DB
			await Task.Delay(100);
			return new Settings();
		}

		private static void CacheValue(Settings settings)
		{
			// logic to cache value
		}
	}

	internal class Settings { }

	internal class SemaphoreSlimExample
	{
		private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

		public static async Task<Settings> GetSettings()
		{
			if (CacheExists())
			{
				return GetCachedValue();
			}

			await semaphore.WaitAsync();
			try
			{
				// Double-check if cache was populated while waiting for semaphore
				if (CacheExists())
				{
					return GetCachedValue();
				}

				var settings = await GetSettingsFromDB();
				CacheValue(settings);
				return settings;
			}
			finally
			{
				semaphore.Release();
			}
		}

		private static bool CacheExists()
		{
			// logic to get cache
			return false;
		}

		private static Settings GetCachedValue()
		{
			// logic to get cache
			return new Settings();
		}

		private static async Task<Settings> GetSettingsFromDB()
		{
			// logic to get settings from DB
			await Task.Delay(100);
			return new Settings();
		}

		private static void CacheValue(Settings settings)
		{
			// logic to cache value
		}
	}
}
