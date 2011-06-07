using System;
using System.Windows;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Ribbon;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for ProductsWindow.xaml
	/// </summary>
	public partial class ProductsWindow : XamRibbonWindow
	{
		public ProductsWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public ProductsWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}

		private bool IsNewProductFormValid()
		{
			if (String.IsNullOrEmpty(txtProductName.Text))
				return false;

			if (String.IsNullOrEmpty(txtProductDescription.Text))
				return false;

			return true;
		}

		private void ResetNewProductForm()
		{
			txtProductName.Text = String.Empty;
			txtProductDescription.Text = String.Empty;
		}

		private void btnRemoveSelected_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (gridProducts.ActiveRecord != null)
			{
				DataRecord record = gridProducts.ActiveRecord as DataRecord;
				int productId = (int)record.Cells["ProductId"].Value;
				string productName = (string)record.Cells["Name"].Value;

				if (MessageBox.Show(string.Format("Are you sure you want to delete the {0} product?", productName), "Delete Product", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();

					if (licenseService.IsProductIdUsed(productId))
					{
						MessageBox.Show(string.Format("Cannot delete the {0} product as it is active.", productName));
						return;
					}

					IProductsService productsService = ObjectLocator.GetInstance<IProductsService>();
					productsService.DeleteProductById(productId);

					IEventAggregator eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
					eventAggregator.SendMessage<ProductsUpdatedEvent>();

					gridProducts.DataSource = productsService.GetAllProducts();
				}
			}
			else
			{
				MessageBox.Show("Please select a product to remove from the grid above.");
			}
		}

		private void btnAddProduct_Click(object sender, RoutedEventArgs e)
		{
			if (IsNewProductFormValid())
			{
				IProductsService productsService = ObjectLocator.GetInstance<IProductsService>();

				if (productsService.IsProductNameInUse(txtProductName.Text.Trim()))
				{
					MessageBox.Show(string.Format("The product name you entered [{0}] is already in use.", txtProductName.Text.Trim()));
					return;
				}

				Product p = new Product();
				p.Name = txtProductName.Text.Trim();
				p.Description = txtProductDescription.Text;

				productsService.SaveProduct(p);

				ResetNewProductForm();

				IEventAggregator eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
				eventAggregator.SendMessage<ProductsUpdatedEvent>();

				gridProducts.DataSource = productsService.GetAllProducts();
			}
			else
			{
				MessageBox.Show("Please fill in the form completely to add a new product.");
			}
		}
	}
}