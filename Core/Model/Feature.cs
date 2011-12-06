using System;

namespace WaveTech.Scutex.Model
{
	public class Feature: BaseObject
	{
		#region Private Members
		private int _productFeatureId;
		private int _productId;
		private string _name;
		private string _description;
		private Guid _uniquePad;
		#endregion Private Members

		#region Public Properties
		public virtual int ProductFeatureId
		{
			get
			{
				return _productFeatureId;
			}

			set
			{
				if (value != _productFeatureId)
				{
					_productFeatureId = value;
					OnPropertyChanged("ProductFeatureId");
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