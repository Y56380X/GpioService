namespace GpioService.Interface.IPC
{
	public class GpioRequest
	{
		public string User { get; set; }
		public int PinNumber { get; set; }
		public RequestAction Action { get; set; }
	}

	public enum RequestAction
	{
		Export,
		Unexport
	}
}