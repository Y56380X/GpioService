using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using GpioService.Interface.IPC;
using GpioService.Service.Common;
using Newtonsoft.Json;
using static GpioService.Interface.IPC.Constants;

namespace GpioService.Service
{
	internal static class Program
	{
		private static void Main()
		{
			// Build pipe server
			var ipcServer = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1);
			
			// TODO: TEST
			new Thread(TestClient){IsBackground = true}.Start();
			
			// Start pipe server
			while (Thread.CurrentThread.ThreadState == ThreadState.Running)
			{
				if(!ipcServer.IsConnected)
					ipcServer.WaitForConnection();

				var request = ipcServer.ReadMessage<GpioRequest>();
				
				using (var userContext = new UserContext(ipcServer.GetImpersonationUserName()))
				{
					Console.WriteLine(JsonConvert.SerializeObject(request));
					//var gpioManager = new GpioManager(); // TODO: Get from DI
				}
				
				ipcServer.Disconnect();
			}
			
			ipcServer.Dispose();
		}

		private static void TestClient()
		{
			while (true)
			{
				var input = Console.ReadLine();
				var request = new GpioRequest{ PinNumber = 190, User = input, Action = RequestAction.Export};
				
				var commStream2 = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
				commStream2.Connect(); 
				commStream2.SendMessage(request);
				commStream2.Dispose();
			}
		}
		
		private static void SendMessage<T>(this Stream stream, T message)
		{
			var messageBuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(message));
			
			// Write message length
			var lengthBuffer = BitConverter.GetBytes(messageBuffer.Length);
			stream.Write(lengthBuffer);
			
			// Write message
			stream.Write(messageBuffer);
		}

		private static T ReadMessage<T>(this Stream stream)
		{
			// Read message length
			var lengthBuffer = new byte[sizeof(int)];
			stream.Read(lengthBuffer);
			
			// Read message
			var messageBuffer = new byte[BitConverter.ToInt32(lengthBuffer)];
			stream.Read(messageBuffer);

			return JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(messageBuffer));
		}
	}
}