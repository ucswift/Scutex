using System.Windows.Controls;

namespace WaveTech.Scutex.Licensing.Gui
{
	/// <summary>
	/// Default root element of a Page, subclasses of which provide sample-specific
	/// properties that expose localized resources.  This is necessary because
	/// Blend only works properly with bindings, not x:Static references.  If Blend
	/// worked properly (i.e. didn't destroy x:Static refs in certain situations),
	/// we could just use x:Static refs against the resx-classes.
	/// </summary>
	internal class LocalizationRoot : UserControl
	{
	}
}