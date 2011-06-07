using System;

namespace WaveTech.Scutex.Model
{
	public class Feature: BaseObject
	{
		#region Private Members
		private int _featureId;
		private int _productId;
		private string _name;
		private Guid _uniquePad;
		#endregion Private Members

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