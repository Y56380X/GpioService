using System.IO;
using GpioService.Interface;

namespace GpioService.Service.Common
{
	internal class GpioManager : IGpioManager
	{
		public bool TryExport(int pinNumber, out IGpioPin pin)
		{
			using (var export = File.OpenWrite("/sys/..."))
				export.WriteByte((byte)pinNumber);
			UserContext.Current
		}

		public bool TryUnexport(IGpioPin pin)
		{
			throw new System.NotImplementedException();
		}
	}
}