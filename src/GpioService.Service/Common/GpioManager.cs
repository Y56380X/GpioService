using System.IO;
using GpioService.Interface;

namespace GpioService.Service.Common
{
	internal class GpioManager : IGpioManager
	{
		public bool TryExport(int pinNumber, out IGpioPin pin)
		{
			// Export gpio pin
			try
			{
				using (var export = File.OpenWrite("/sys/class/gpio/export"))
					export.WriteByte((byte) pinNumber);
			}
			catch
			{
				
			}
			
			pin = new GpioPin(pinNumber, $"/sys/class/gpio/gpio{pinNumber}");
			return true;
		}

		public bool TryUnexport(IGpioPin pin)
		{
			using (var export = File.OpenWrite("/sys/class/gpio/unexport"))
				export.WriteByte((byte)pin.Number);
			
			throw new System.NotImplementedException();
		}
	}
}