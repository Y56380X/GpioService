using GpioService.Interface;

namespace GpioService.Service.Common
{
	internal class GpioPin : IGpioPin
	{
		public int Number { get; }
		public Mode Mode { get; set; }
		public int Value { get; set; }
		public string Path { get; }

		public GpioPin(int number, string path = null)
		{
			Number = number;
			Path = path;
		}
	}
}