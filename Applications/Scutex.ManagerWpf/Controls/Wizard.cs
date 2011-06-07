using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace WaveTech.Scutex.Manager.Controls
{
	/// <summary>
	/// Represents a wizard control.
	/// </summary>
	[TemplatePart(Name = Wizard.PART_Finish, Type = typeof(ButtonBase))]
	[TemplatePart(Name = Wizard.PART_Next, Type = typeof(ButtonBase))]
	[TemplatePart(Name = Wizard.PART_Previous, Type = typeof(ButtonBase))]
	[TemplatePart(Name = Wizard.PART_Cancel, Type = typeof(ButtonBase))]
	[TemplatePart(Name = Wizard.PART_Help, Type = typeof(ButtonBase))]
	public class Wizard : ItemsControl
	{
		#region Public Resource Keys

		public static ResourceKey HeaderPanelBorderResourceKey =
				new ComponentResourceKey(typeof(Wizard), "HeaderPanelBorderResourceKey");

		public static ResourceKey SideHeaderPanelBorderResourceKey =
				new ComponentResourceKey(typeof(Wizard), "SideHeaderPanelBorderResourceKey");

		public static ResourceKey ContentPanelBorderResourceKey =
				new ComponentResourceKey(typeof(Wizard), "ContentPanelBorderResourceKey");

		public static ResourceKey NavigationPanelBorderResourceKey =
				new ComponentResourceKey(typeof(Wizard), "NavigationPanelBorderResourceKey");

		public static ResourceKey NavigationButtonResourceKey =
				new ComponentResourceKey(typeof(Wizard), "NavigationButtonResourceKey");

		#endregion

		#region Private Constants

		private const string PART_Finish = "PART_Finish";
		private const string PART_Next = "PART_Next";
		private const string PART_Previous = "PART_Previous";
		private const string PART_Cancel = "PART_Cancel";
		private const string PART_Help = "PART_Help";

		#endregion

		#region Public Routed Events

		public event RoutedEventHandler HelpClick
		{
			add { AddHandler(HelpClickEvent, value); }
			remove { RemoveHandler(HelpClickEvent, value); }
		}

		public static readonly RoutedEvent HelpClickEvent = EventManager.RegisterRoutedEvent(
				"HelpClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Wizard));

		public event RoutedEventHandler CancelClick
		{
			add { AddHandler(CancelClickEvent, value); }
			remove { RemoveHandler(CancelClickEvent, value); }
		}

		public static readonly RoutedEvent CancelClickEvent = EventManager.RegisterRoutedEvent(
				"CancelClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Wizard));

		public event RoutedEventHandler PreviousClick
		{
			add { AddHandler(PreviousClickEvent, value); }
			remove { RemoveHandler(PreviousClickEvent, value); }
		}

		public static readonly RoutedEvent PreviousClickEvent = EventManager.RegisterRoutedEvent(
				"PreviousClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Wizard));

		public event RoutedEventHandler NextClick
		{
			add { AddHandler(NextClickEvent, value); }
			remove { RemoveHandler(NextClickEvent, value); }
		}

		public static readonly RoutedEvent NextClickEvent = EventManager.RegisterRoutedEvent(
				"NextClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Wizard));

		public event RoutedEventHandler FinishClick
		{
			add { AddHandler(FinishClickEvent, value); }
			remove { RemoveHandler(FinishClickEvent, value); }
		}

		public static readonly RoutedEvent FinishClickEvent = EventManager.RegisterRoutedEvent(
				"FinishClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Wizard));

		#endregion

		#region Static Constructor

		static Wizard()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Wizard),
					new FrameworkPropertyMetadata(typeof(Wizard)));
		}

		#endregion

		#region Public Dependency Properties

		public static readonly DependencyProperty ActivePageProperty =
				DependencyProperty.Register("ActivePage", typeof(WizardPage), typeof(Wizard), new UIPropertyMetadata(null, OnActivePageChanged, CoerceActivePage));

		/// <summary>
		/// Gets or sets active wizard page. This is a readonly dependency property.
		/// </summary>
		public WizardPage ActivePage
		{
			get { return (WizardPage)GetValue(ActivePageProperty); }
			set { SetValue(ActivePageProperty, value); }
		}

		public static readonly DependencyProperty ActivePageIndexProperty =
				DependencyProperty.Register("ActivePageIndex", typeof(int), typeof(Wizard), new UIPropertyMetadata(-1, OnActivePageIndexChanged, CoerceActivePageIndex));

		/// <summary>
		/// Gets or sets current page index. This is a readonly dependency property.
		/// </summary>
		public int ActivePageIndex
		{
			get { return (int)GetValue(ActivePageIndexProperty); }
			set { SetValue(ActivePageIndexProperty, value); }
		}

		#endregion

		#region Overrides

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			// If the wizard contains pages, setup defaults
			if (Items.Count > 0)
				ActivePageIndex = 0;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			var button = GetTemplateChild(PART_Finish) as ButtonBase;
			if (button != null)
				button.Click += (OnFinishedClicked);

			button = GetTemplateChild(PART_Next) as ButtonBase;
			if (button != null)
				button.Click += (OnNextClicked);

			button = GetTemplateChild(PART_Previous) as ButtonBase;
			if (button != null)
				button.Click += (OnPreviousClicked);

			button = GetTemplateChild(PART_Cancel) as ButtonBase;
			if (button != null)
				button.Click += (OnCancelClicked);

			button = GetTemplateChild(PART_Help) as ButtonBase;
			if (button != null)
				button.Click += (OnHelpClicked);
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new WizardPage();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is WizardPage;
		}

		#endregion

		#region Protected Helpers

		protected virtual void OnFinishedClicked(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			RaiseEvent(new RoutedEventArgs(FinishClickEvent, this));
		}

		protected virtual void OnNextClicked(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			RaiseEvent(new RoutedEventArgs(NextClickEvent, this));

			int stepSize = ActivePage.NextStepSize;
			ActivePageIndex += stepSize;
		}

		protected virtual void OnPreviousClicked(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			RaiseEvent(new RoutedEventArgs(PreviousClickEvent, this));

			int stepSize = ActivePage.PreviousStepSize;
			ActivePageIndex -= stepSize;
		}

		protected virtual void OnCancelClicked(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			RaiseEvent(new RoutedEventArgs(CancelClickEvent, this));
		}

		protected virtual void OnHelpClicked(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			RaiseEvent(new RoutedEventArgs(HelpClickEvent, this));
		}

		#endregion

		#region Private Dependency Property Callbacks

		private static void OnActivePageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var wizard = (Wizard)d;
			var index = (int)e.NewValue;
			var oldIndex = (int)e.OldValue;
			if (index != -1 && index != oldIndex && index < wizard.Items.Count && index >= 0)
				wizard.ActivePage = (WizardPage)wizard.Items[index];
			else if (index == -1)
				wizard.ActivePage = null;
		}

		private static object CoerceActivePageIndex(DependencyObject d, object value)
		{
			var wizard = (Wizard)d;
			var index = (int)value;

			if (index >= wizard.Items.Count)
				return wizard.Items.Count - 1;

			if (index >= 0 && index < wizard.Items.Count)
				return index;

			if (index < 0 && wizard.Items.Count > 0)
				return 0;

			return -1;
		}

		private static void OnActivePageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var wizard = (Wizard)d;
			var page = (WizardPage)e.NewValue;
			var oldPage = (WizardPage)e.OldValue;

			if (page != null && oldPage != page)
			{
				// Raise event
				if (oldPage != null)
					oldPage.OnPageClose();

				// update the index
				int index = wizard.Items.IndexOf(page);
				wizard.ActivePageIndex = index;

				// Set boundary values for navigation buttons
				if (index == 0)
				{
					wizard.ActivePage.CanNavigatePrevious = false;
					if (wizard.Items.Count == 1)
						wizard.ActivePage.CanNavigateNext = false;
				}
				else if (index == wizard.Items.Count - 1)
					wizard.ActivePage.CanNavigateNext = false;

				// After page is up and runnig, rais event
				page.OnPageShow();
			}
			else if (page == null)
			{
				// Raise event
				if (oldPage != null)
					oldPage.OnPageClose();

				wizard.ActivePageIndex = -1;
			}
		}

		private static object CoerceActivePage(DependencyObject d, object value)
		{
			var wizard = (Wizard)d;
			var page = (WizardPage)value;

			int index = wizard.Items.IndexOf(page);

			// Given page does not exist in the internal collection
			if (index == -1)
			{
				if (wizard.Items.Count > 0)
					return wizard.Items[0];

				return null;
			}

			return page;
		}

		#endregion
	}

	/// <summary>
	/// Represents single wizard's page.
	/// </summary>
	public class WizardPage : ContentControl
	{
		#region Public Dependency properties

		public static readonly DependencyProperty SideHeaderProperty =
				DependencyProperty.Register("SideHeader", typeof(object), typeof(WizardPage), new UIPropertyMetadata(null));

		public static readonly DependencyProperty HeaderProperty =
				DependencyProperty.Register("Header", typeof(object), typeof(WizardPage), new UIPropertyMetadata(null));

		public static readonly DependencyProperty CanFinishProperty =
				DependencyProperty.Register("CanFinish", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(false));

		public static readonly DependencyProperty CanNextProperty =
				DependencyProperty.Register("CanNext", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

		public static readonly DependencyProperty CanPreviousProperty =
				DependencyProperty.Register("CanPrevious", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

		public static readonly DependencyProperty CanCancelProperty =
				DependencyProperty.Register("CanCancel", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

		public static readonly DependencyProperty CanHelpProperty =
				DependencyProperty.Register("CanHelp", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(false));

		public static readonly DependencyProperty NextStepSizeProperty =
				DependencyProperty.Register("NextStepSize", typeof(int), typeof(WizardPage), new UIPropertyMetadata(1, OnNextStepSizeChanged, CoerceNextStepSize));

		public static readonly DependencyProperty PreviousStepSizeProperty =
				DependencyProperty.Register("PreviousStepSize", typeof(int), typeof(WizardPage), new UIPropertyMetadata(1, OnPreviousStepSizeChanged, CoercePreviousStepSize));

		// Read-only dependency properties
		private static readonly DependencyPropertyKey CanNavigateNextPropertyKey =
				DependencyProperty.RegisterReadOnly("CanNavigateNext", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

		public static readonly DependencyPropertyKey CanNavigatePreviousPropertyKey =
				DependencyProperty.RegisterReadOnly("CanNavigatePrevious", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

		/// <summary>
		/// Gets or sets page's header.
		/// </summary>
		public object Header
		{
			get { return GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		/// <summary>
		/// Gets or sets page's side header.
		/// </summary>
		public object SideHeader
		{
			get { return GetValue(SideHeaderProperty); }
			set { SetValue(SideHeaderProperty, value); }
		}

		/// <summary>
		/// Gets or sets value indicating whether finish button is enabled for this page. This is a dependency property.
		/// </summary>
		public bool CanFinish
		{
			get { return (bool)GetValue(CanFinishProperty); }
			set { SetValue(CanFinishProperty, value); }
		}

		/// <summary>
		/// Gets or sets value indicating whether next button is enabled for this page. This is a dependency property.
		/// </summary>
		public bool CanNext
		{
			get { return (bool)GetValue(CanNextProperty); }
			set { SetValue(CanNextProperty, value); }
		}

		/// <summary>
		/// Gets or sets value indicating whether previous button is enabled for this page. This is a dependency property.
		/// </summary>
		public bool CanPrevious
		{
			get { return (bool)GetValue(CanPreviousProperty); }
			set { SetValue(CanPreviousProperty, value); }
		}

		/// <summary>
		/// Gets or sets value indicating whether cancel button is enabled for this page. This is a dependency property.
		/// </summary>
		public bool CanCancel
		{
			get { return (bool)GetValue(CanCancelProperty); }
			set { SetValue(CanCancelProperty, value); }
		}

		/// <summary>
		/// Gets or sets value indicating whether help button is enabled for this page. This is a dependency property.
		/// </summary>
		public bool CanHelp
		{
			get { return (bool)GetValue(CanHelpProperty); }
			set { SetValue(CanHelpProperty, value); }
		}

		/// <summary>
		/// Gets value indicating whether it is possible to navigate to the next page. This is a readonly dependency property.
		/// </summary>
		public bool CanNavigateNext
		{
			get { return (bool)GetValue(CanNavigateNextPropertyKey.DependencyProperty); }
			internal set { SetValue(CanNavigateNextPropertyKey, value); }
		}

		/// <summary>
		/// Gets value indicating whether it is possible to navigate to the previous page. This is a readonly dependency property.
		/// </summary>
		public bool CanNavigatePrevious
		{
			get { return (bool)GetValue(CanNavigatePreviousPropertyKey.DependencyProperty); }
			internal set { SetValue(CanNavigatePreviousPropertyKey, value); }
		}

		public int PreviousStepSize
		{
			get { return (int)GetValue(PreviousStepSizeProperty); }
			set { SetValue(PreviousStepSizeProperty, value); }
		}

		public int NextStepSize
		{
			get { return (int)GetValue(NextStepSizeProperty); }
			set { SetValue(NextStepSizeProperty, value); }
		}

		#endregion

		#region Public Routed Events

		public event RoutedEventHandler PageClose
		{
			add { AddHandler(PageCloseEvent, value); }
			remove { RemoveHandler(PageCloseEvent, value); }
		}

		public static readonly RoutedEvent PageCloseEvent = EventManager.RegisterRoutedEvent(
				"PageClose", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WizardPage));

		public event RoutedEventHandler PageShow
		{
			add { AddHandler(PageShowEvent, value); }
			remove { RemoveHandler(PageShowEvent, value); }
		}

		public static readonly RoutedEvent PageShowEvent = EventManager.RegisterRoutedEvent(
				"PageShow", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WizardPage));

		#endregion

		#region Protected Internal Helpers

		protected internal virtual void OnPageClose()
		{
			RaiseEvent(new RoutedEventArgs(PageCloseEvent, this));
		}

		protected internal virtual void OnPageShow()
		{
			RaiseEvent(new RoutedEventArgs(PageShowEvent, this));
		}

		#endregion

		#region Private Dependency Property Callbacks

		private static void OnNextStepSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Do nothing
		}

		private static object CoerceNextStepSize(DependencyObject d, object value)
		{
			var stepSize = (int)value;
			if (stepSize < 1)
				return 1;

			return stepSize;
		}

		private static void OnPreviousStepSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Do nothing
		}

		private static object CoercePreviousStepSize(DependencyObject d, object value)
		{
			var stepSize = (int)value;
			if (stepSize < 1)
				return 1;

			return stepSize;
		}

		#endregion
	}

	internal class NavigationMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var canNavigate = (bool)values[0];
			var can = (bool)values[1];

			return can && canNavigate;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
