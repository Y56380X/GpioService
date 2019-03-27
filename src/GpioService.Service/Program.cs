using System;
using System.IO.Pipes;
using System.Threading;
using GpioService.Common.IPC;
using GpioService.Service.Common;
using Newtonsoft.Json;
using static GpioService.Common.IPC.Constants;

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
	}
}