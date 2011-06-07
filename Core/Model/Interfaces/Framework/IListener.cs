namespace WaveTech.Scutex.Model.Interfaces.Framework
{
	public interface IListener<T>
	{
		void Handle(T message);
	}
}