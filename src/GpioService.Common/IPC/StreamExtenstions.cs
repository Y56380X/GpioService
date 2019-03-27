using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GpioService.Common.IPC
{
	public static class StreamExtenstions
	{
		public static void SendMessage<T>(this Stream stream, T message)
		{
			var messageBuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(message));
			
			// Write message length
			var lengthBuffer = BitConverter.GetBytes(messageBuffer.Length);
			stream.Write(lengthBuffer, 0, lengthBuffer.Length);
			
			// Write message
			stream.Write(messageBuffer, 0, messageBuffer.Length);
		}

		public static T ReadMessage<T>(this Stream stream)
		{
			// Read message length
			var lengthBuffer = new byte[sizeof(int)];
			stream.Read(lengthBuffer, 0, lengthBuffer.Length);
			
			// Read message
			var messageBuffer = new byte[BitConverter.ToInt32(lengthBuffer, 0)];
			stream.Read(messageBuffer, 0, messageBuffer.Length);

			return JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(messageBuffer));
		}
	}
}