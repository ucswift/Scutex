using System.ComponentModel;

namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// Represents the base type of all business objects.
	/// </summary>
	public class BaseObject : INotifyPropertyChanged
	{
		private bool isModified;
		protected bool Notifying { get; private set; }
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (!Notifying)
			{
				//                Notifying = true;
				try
				{
					//Debug.WriteLine(string.Format("> Notify on {0}.{1}:  {2}", GetType().Name, Name, propertyName));
					//Debug.Indent();
					IsModified = true;
					if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
					//Debug.Unindent();
				}
				finally
				{
					Notifying = false;
				}
			}
		}

		public void RaisePropertyChanged(string propertyName)
		{
			OnPropertyChanged(propertyName);
		}

		/// <summary>
		/// Gets whether data is modified.
		/// </summary>
		public bool IsModified
		{
			get { return isModified; }
			set
			{
				if (isModified != value)
				{
					isModified = value;
					OnModifiedChanged();
					if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsModified"));
					isModified = value;
				}
			}
		}

		protected virtual void OnModifiedChanged()
		{
		}
	}
}