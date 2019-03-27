using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GpioService.Interface.IPC;
using GpioService.Service.Common;
using Newtonsoft.Json;
using static GpioService.Interface.IPC.Constants;

namespace GpioService.Service
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			using (var ipcSocket = new Socket(SocketType.Stream, ProtocolType.Tcp))
			{
				ipcSocket.Bind(new IPEndPoint(IPAddress.IPv6Loopback, IpcPort));
				ipcSocket.Listen(1);

				while (Thread.CurrentThread.ThreadState == ThreadState.Running)
				{
					var acceptTask = ipcSocket.AcceptAsync();
					if (acceptTask.Wait(TimeSpan.FromSeconds(10)))
					{
						var connectionSocket = acceptTask.Result;
						var buffer = new byte[connectionSocket.Available];
						connectionSocket.Receive(buffer);
						var request = JsonConvert.DeserializeObject<GpioRequest>(System.Text.Encoding.Default.GetString(buffer));
						GpioResponse response;
						using (var userContext = new UserContext(request.User))
						{
							new GpioManager().TryExport(out var pin);
							response = new GpioResponse
							{
								Result = true, 
								Payload = (pin as GpioPin).Path
							};
						}

						connectionSocket.Send(
							System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)));
						connectionSocket.Close();
					}
				}
			}
		}
	}
}