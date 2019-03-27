namespace GpioService.Interface
{
	public interface IGpioManager
	{
		bool TryExport(int pinNumber, out IGpioPin pin);
		bool TryUnexport(IGpioPin pin);
	}
}