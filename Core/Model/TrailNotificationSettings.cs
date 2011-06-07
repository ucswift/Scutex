namespace WaveTech.Scutex.Model
{
	public class TrailNotificationSettings: BaseObject
	{
		#region Private Members
		private int _tryButtonDelay;
		#endregion Private Members

		#region Public Properties
		public virtual int TryButtonDelay
		{
			get
			{
				return _tryButtonDelay;
			}

			set
			{
				if (value != _tryButtonDelay)
				{
					_tryButtonDelay = value;
					OnPropertyChanged("TryButtonDelay");
				}
			}
		}
		#endregion Public Properties
	}
}