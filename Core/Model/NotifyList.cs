using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// A List that supports notifications when the list or any item has changed.
	/// </summary>
	/// <typeparam name="T">The type of item in this list.</typeparam>
	public class NotifyList<T> : BindingList<T> where T : BaseObject
	{
		#region Public Events
		public event EventHandler<NotifyEventArgs<T>> Changed;
		public event EventHandler<NotifyEventArgs<T>> ItemPropertyChanged;
		#endregion Public Events

		#region Constructors
		public NotifyList()
			: base()
		{
		}

		public NotifyList(IEnumerable<T> array)
			: base()
		{
			this.RaiseListChangedEvents = false;
			foreach (var item in array.ToArray())
			{
				this.Add(item);
			}
			this.RaiseListChangedEvents = true;
		}
		#endregion Constructors

		#region Public Methods
		public void MoveUp(T item)
		{
			int oldIndex = IndexOf(item);
			int newIndex = Math.Max(0, oldIndex - 1);
			ChangePosition(item, oldIndex, newIndex);
		}

		public void MoveDown(T item)
		{
			int oldIndex = IndexOf(item);
			int newIndex = Math.Min(Count - 1, oldIndex + 1);
			ChangePosition(item, oldIndex, newIndex);
		}

		public void MoveToTop(T item)
		{
			int oldIndex = IndexOf(item);
			ChangePosition(item, oldIndex, 0);
		}

		public void MoveToBottom(T item)
		{
			int oldIndex = IndexOf(item);
			ChangePosition(item, oldIndex, Count - 1);
		}
		#endregion Public Methods

		#region Private Methods
		private void ChangePosition(T item, int oldIndex, int newIndex)
		{
			if (oldIndex != newIndex)
			{
				RaiseListChangedEvents = false;
				RemoveAt(oldIndex);
				Insert(newIndex, item);
				RaiseListChangedEvents = true;
				//    OnChanged(ChangedEventType.Reset, null);
				ResetBindings();
				OnChanged(ChangedEventType.Reset, null);
			}
		}
		#endregion Private Methods

		#region Event Handlers
		protected void OnChanged(ChangedEventType type, T item)
		{
			// if (RaiseListChangedEvents)
			{
				if (Changed != null) Changed(this, new NotifyEventArgs<T>(type, item));
			}
		}

		[SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
		protected void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (ItemPropertyChanged != null) ItemPropertyChanged(this, new NotifyEventArgs<T>(ChangedEventType.Modified, (T)sender));
		}
		#endregion Event Handlers

		#region Protected Overrides
		protected override void RemoveItem(int index)
		{
			T item = this[index];
			item.PropertyChanged -= OnItemPropertyChanged;

			base.RemoveItem(index);
			if (RaiseListChangedEvents) OnChanged(ChangedEventType.Removed, item);
		}

		protected override void InsertItem(int index, T item)
		{
			item.PropertyChanged += OnItemPropertyChanged;

			base.InsertItem(index, item);
			if (RaiseListChangedEvents) OnChanged(ChangedEventType.Added, this[index]);
		}
		#endregion Protected Overrides
	}
}
