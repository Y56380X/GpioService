using System;
using GpioService.Interface;

namespace GpioService.Connector
{
	public class GpioPin : IGpioPin
	{
		public int Number { get; }
		public Mode Mode { get; set; }
		public int Value { get; set; }
		
		internal GpioPin(string filename)
		{
			throw new NotImplementedException();
		}
	}
}