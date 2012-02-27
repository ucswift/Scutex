using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.FingerprintViewer
{
	public partial class MainWindow : Window
	{
		private DispatcherTimer tryButtonDelayTimer;
		private int _tryTimerTick = 0;

		public MainWindow()
		{
			InitializeComponent();

			SetIcon();
			SetWindowIcon();

			ccContent.Content = new FingerprintContent();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Environment.Exit(1005);
		}

		private void SetWindowIcon()
		{
			try
			{
				IconBitmapDecoder ibd = new IconBitmapDecoder(
				new Uri("pack://application:,,,/" + GetType().Assembly.GetName().Name + ";component/Scutex2.ico", UriKind.RelativeOrAbsolute),
				BitmapCreateOptions.None,
				BitmapCacheOption.Default);
				Icon = ibd.Frames[0];
			}
			catch
			{ }

		}

		private void SetIcon()
		{
			try
			{
				Uri myUri = new Uri("pack://application:,,,/" + GetType().Assembly.GetName().Name + ";component/Scutex.png",
														UriKind.RelativeOrAbsolute);
				PngBitmapDecoder decoder = new PngBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
				BitmapSource bitmapSource = decoder.Frames[0];
				imageIcon.Source = bitmapSource;
			}
			catch
			{ }
		}

		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}
	}
}