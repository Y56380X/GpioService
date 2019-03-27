using System;
using System.Collections.Generic;
using System.Threading;

namespace GpioService.Service.Common
{
	internal class UserContext : IDisposable
	{
		private static Dictionary<Thread, UserContext> userContexts = new Dictionary<Thread, UserContext>();

		public static UserContext Current => userContexts[Thread.CurrentThread];

		public string Username { get; }
		
		public UserContext(string username)
		{
			userContexts.Add(Thread.CurrentThread, this);
			Username = username;
		}
		
		public void Dispose()
		{
			userContexts.Remove(Thread.CurrentThread);
		}
	}
}