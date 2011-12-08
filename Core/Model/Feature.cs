using System;

namespace WaveTech.Scutex.Model
{
	public class Feature: BaseObject
	{
		#region Private Members
		private int _featureId;
		private int _productId;
		private string _name;
		private string _description;
		private Guid _uniquePad;
		#endregion Private Members

		#region Constructor
		public Feature()
		{
			// Initialize the unique pad to ensure it's always there
			_uniquePad = Guid.NewGuid();
		}
		#endregion Constructor

		#region Public Properties
		public virtual int FeatureId
		{
			get
			{
				return _featureId;
			}

			set
			{
				if (value != _featureId)
				{
					_featureId = value;
					OnPropertyChanged("FeatureId");
				}
			}
		}

		public virtual int ProductId
		{
			get
			{
				return _productId;
			}

			set
			{
				if (value != _productId)
				{
					_productId = value;
					OnPropertyChanged("ProductId");
				}
			}
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}

			set
			{
				if (value != _name)
				{
					_name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		public virtual string Description
		{
			get
			{
				return _description;
			}

			set
			{
				if (value != _description)
				{
					_description = value;
					OnPropertyChanged("Description");
				}
			}
		}

		public virtual Guid UniquePad
		{
			get
			{
				return _uniquePad;
			}

			set
			{
				if (value != _uniquePad)
				{
					_uniquePad = value;
					OnPropertyChanged("UniquePad");
				}
			}
		}
		#endregion Public Properties
	}
}