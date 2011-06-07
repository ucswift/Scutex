
namespace WaveTech.Scutex.Model
{
	public abstract class BaseServiceResult
	{
		public virtual bool WasOperationSuccessful { get; set; }
		public virtual bool WasRequestValid { get; set; }
		public virtual bool WasException { get; set; }
		public virtual string ExceptionMessage { get; set; }
	}
}