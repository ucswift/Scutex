using System;
using System.Windows;
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
	/// Interaction logic for NewProductWindow.xaml
	/// </summary>
	public partial class NewProductWindow : XamRibbonWindow
	{
		public NewProductWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public NewProductWindow(Window parent)
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

				this.Close();
			}
			else
			{
				MessageBox.Show("Please fill in the form completely to add a new product.");
			}
		}
	}
}
