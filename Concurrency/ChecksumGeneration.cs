namespace Concurrency
{
	class ChecksumGeneration
	{
		private const ulong Crc64Polynomial = 0x42F0E1EBA9EA3693;
		private static readonly ulong[] Crc64Table = GenerateCrc64Table();
		public static void GenerateChecksum()
		{
			string filePath = @"C:\Users\kklak\Downloads\new file.txt"; 

			try
			{
				ulong checksum = ComputeCRC64NVME(filePath);
				Console.WriteLine($"CRC64-NVME Checksum (Hex): {checksum:X16}");

				// Convert to Base64 (AWS format)
				byte[] checksumBytes = BitConverter.GetBytes(checksum);
				string base64Checksum = Convert.ToBase64String(checksumBytes);
				Console.WriteLine($"CRC64-NVME Checksum (Base64): {base64Checksum}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}

		public static byte[] HexStringToBytes(string hex)
		{
			if (hex == null)
				throw new ArgumentNullException(nameof(hex));

			if (hex.Length % 2 != 0)
				throw new ArgumentException("Hex string must have an even length");

			byte[] bytes = new byte[hex.Length / 2];
			for (int i = 0; i < hex.Length; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			}
			return bytes;
		}

		static ulong ComputeCRC64NVME(string filePath)
		{
			ulong crc = 0;

			using (FileStream fs = File.OpenRead(filePath))
			{
				byte[] buffer = new byte[8192];
				int bytesRead;
				while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
				{
					for (int i = 0; i < bytesRead; i++)
					{
						byte index = (byte)((crc ^ buffer[i]) & 0xFF);
						crc = (crc >> 8) ^ Crc64Table[index];
					}
				}
			}

			return crc ^ 0xFFFFFFFFFFFFFFFF;
		}

		static ulong[] GenerateCrc64Table()
		{
			ulong[] table = new ulong[256];

			for (ulong i = 0; i < 256; i++)
			{
				ulong crc = i;
				for (int j = 0; j < 8; j++)
				{
					if ((crc & 1) != 0)
						crc = (crc >> 1) ^ Crc64Polynomial;
					else
						crc >>= 1;
				}
				table[i] = crc;
			}

			return table;
		}
	}
}
