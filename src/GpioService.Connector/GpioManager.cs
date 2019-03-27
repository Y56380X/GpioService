using GpioService.Interface;

namespace GpioService.Connector
{
	public class GpioManager : IGpioManager
	{
		public bool TryExport(out IGpioPin pin)
		{
			// Request
			string filename = "";

			pin = new GpioPin(filename);

			return true;
		}

		public bool TryUnexport(IGpioPin pin)
		{
			throw new System.NotImplementedException();
		}
	}
}