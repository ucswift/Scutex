using System;
using System.Windows;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for UpdateProductWindow.xaml
	/// </summary>
	public partial class UpdateProductWindow : Window
	{
		private Product _product;

		public UpdateProductWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public UpdateProductWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}

		public UpdateProductWindow(Window parent, Product product)
			: this(parent)
		{
			_product = product;

			txtProductName.Text = _product.Name;
			txtProductDescription.Text = _product.Description;
		}

		private bool IsProductFormValid()
		{
			if (String.IsNullOrEmpty(txtProductName.Text))
				return false;

			if (String.IsNullOrEmpty(txtProductDescription.Text))
				return false;

			return true;
		}

		private void btnAddProduct_Click(object sender, RoutedEventArgs e)
		{
			if (IsProductFormValid())
			{
				IProductsService productsService = ObjectLocator.GetInstance<IProductsService>();

				_product.Name = txtProductName.Text.Trim();
				_product.Description = txtProductDescription.Text;

				productsService.SaveProduct(_product);

				IEventAggregator eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
				eventAggregator.SendMessage<ProductsUpdatedEvent>();

				this.Close();
			}
			else
			{
				MessageBox.Show("Please fill in the form completely to update the product.");
			}
		}
	}
}
