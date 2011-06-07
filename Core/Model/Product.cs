using System;
using System.Collections.Generic;

namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// Describes a unique product in the Scutex system. Products are unique and distinct within a company
	/// and are used to tie licenses to specific products and companies.
	/// </summary>
	public class Product: BaseObject
	{
		#region Private Members
		private int _productId;
		private string _name;
		private string _description;
		private string _uniquePad;
		private List<Feature> _features;
		#endregion Private Members

		#region Constructor
		public Product()
		{
			// Initialize the unique pad to ensure it's always there
			_uniquePad = Guid.NewGuid().ToString();
		}
		#endregion Constructor

		#region Public Properties
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

		public virtual string UniquePad
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

		public virtual List<Feature> Features
		{
			get
			{
				return _features;
			}

			set
			{
				if (value != _features)
				{
					_features = value;
					OnPropertyChanged("Features");
				}
			}
		}
		#endregion Public Properties

		#region Public Methods
		/// <summary>
		/// Formats the product Id into a Hex value that can be consumed
		/// for the license key generation system.
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public string GetFormattedProductId(int length)
		{
			string data = ProductId.ToString("X");
			data = data.PadLeft(length, char.Parse("0"));

			return data;
		}
		#endregion Public Methods

		public bool Equals(Product other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other._uniquePad, _uniquePad) && Equals(other._description, _description) && Equals(other._name, _name) && other._productId == _productId;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (Product)) return false;
			return Equals((Product) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = (_uniquePad != null ? _uniquePad.GetHashCode() : 0);
				result = (result*397) ^ (_description != null ? _description.GetHashCode() : 0);
				result = (result*397) ^ (_name != null ? _name.GetHashCode() : 0);
				result = (result*397) ^ _productId;
				return result;
			}
		}

		public static bool operator ==(Product left, Product right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Product left, Product right)
		{
			return !Equals(left, right);
		}
	}
}