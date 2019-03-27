namespace GpioService.Common.IPC
{
	public class GpioRequest
	{
		public int PinNumber { get; set; }
		public RequestAction Action { get; set; }
	}

	public enum RequestAction
	{
		Export,
		Unexport
	}
}