using System;
using System.IO.Pipes;
using System.Threading;
using GpioService.Common.IPC;
using GpioService.Interface;
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
				var response = new GpioResponse();
				
				using (new UserContext(ipcServer.GetImpersonationUserName()))
				{
					#if DEBUG
					Console.WriteLine(JsonConvert.SerializeObject(request));
					#endif
					
					var gpioManager = new GpioManager(); // TODO: Get from DI
					switch (request.Action)
					{
						case RequestAction.Export:
							response.Result = gpioManager.TryExport(request.PinNumber, out IGpioPin pin);
							response.Payload = (pin as GpioPin)?.Path;
							break;
						case RequestAction.Unexport:
							response.Result = gpioManager.TryUnexport(new GpioPin(request.PinNumber));
							break;
						default:
							response.Result = false;
							break;
					}
				}
				
				ipcServer.Disconnect();
			}
			
			ipcServer.Dispose();
		}

		private static void TestClient()
		{
			while (true)
			{
				Console.ReadLine();
				var request = new GpioRequest{ PinNumber = 7, Action = RequestAction.Export};
				
				var commStream2 = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
				commStream2.Connect(); 
				commStream2.SendMessage(request);
				commStream2.Dispose();
			}
		}
	}
}