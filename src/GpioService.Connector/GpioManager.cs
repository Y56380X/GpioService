using GpioService.Interface;

namespace GpioService.Connector
{
	public class GpioManager : IGpioManager
	{
		public bool TryExport(int pinNumber, out IGpioPin pin)
		{
			throw new System.NotImplementedException();
		}

		public bool TryUnexport(IGpioPin pin)
		{
			throw new System.NotImplementedException();
		}
	}
}