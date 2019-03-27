namespace GpioService.Interface
{
	public interface IGpioPin
	{
		int Number { get; }
		Mode Mode { get; set; }
		int Value { get; set; }
	}

	public enum Mode
	{
		In,
		Out
	}
}