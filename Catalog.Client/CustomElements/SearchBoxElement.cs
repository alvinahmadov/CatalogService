using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Telerik.WinControls;
using Telerik.WinControls.Layouts;
using Telerik.WinControls.UI;

namespace Catalog.Client
{
	public class SearchTextBox : RadTextBox
	{
		#region Properties
		public bool SearchButtonClicked { get; set; } = false;

		public override string ThemeClassName { get => typeof(RadTextBox).FullName; }

		private RadButtonElement clearButton;
		private RadButtonElement searchButton;
		private RadTextBoxItem textBoxItem;
		private StackLayoutElement stackPanel;

		#endregion

		protected override void InitializeTextElement()
		{
			base.InitializeTextElement();
			this.TextBoxElement.TextBoxItem.StretchVertically = false;
			this.TextBoxElement.TextBoxItem.StretchHorizontally = true;
			this.TextBoxElement.TextBoxItem.Margin = new Padding(0);
			this.TextBoxElement.TextBoxItem.Padding = new Padding(0);

			this.clearButton = new RadButtonElement(String.Empty, Properties.Resources.delete)
			{
				DisplayStyle = DisplayStyle.Image,
				Margin = new Padding(0),
				Padding = new Padding(30),
				TextImageRelation = TextImageRelation.ImageAboveText,
				ImageAlignment = ContentAlignment.MiddleCenter,
				Alignment = ContentAlignment.MiddleCenter,
				Text = String.Empty,
				ShadowColor = SystemColors.Window,
				BackColor = Color.Transparent,
				Image = Properties.Resources.delete,
				BackgroundShape = new RadImageShape(),
				Shape = new CircleShape(),
			};

			clearButton.ImagePrimitive.ImageLayout = ImageLayout.Stretch;

			this.searchButton = new RadButtonElement
			{
				Margin = new Padding(0),
				Text = string.Empty,
				Image = Properties.Resources.searchbtn,
				ShowBorder = false
			};

			this.searchButton.ButtonFillElement.Visibility = Telerik.WinControls.ElementVisibility.Visible;

			this.stackPanel = new StackLayoutElement
			{
				Orientation = Orientation.Horizontal,
				Margin = new Padding(5, 0, 5, 0)
			};

			stackPanel.Children.Add(this.clearButton);
			stackPanel.Children.Add(this.searchButton);

			clearButton.Visibility = ElementVisibility.Hidden;
			this.textBoxItem = this.TextBoxElement.TextBoxItem;
			this.TextBoxElement.Children.Remove(this.textBoxItem);
			var dockPanel = new DockLayoutPanel();
			dockPanel.Children.Add(stackPanel);
			dockPanel.Children.Add(this.textBoxItem);
			DockLayoutPanel.SetDock(this.textBoxItem, Telerik.WinControls.Layouts.Dock.Left);
			DockLayoutPanel.SetDock(stackPanel, Telerik.WinControls.Layouts.Dock.Right);
			this.TextBoxElement.Children.Add(dockPanel);
			this.Margin = new Padding(50, 0, 50, 0);
		}

		#region Events

		protected override void OnLoad(Size desiredSize)
		{
			base.OnLoad(desiredSize);
			SearchButtonClicked = false;
			searchButton.Click += SearchButton_Click;
			clearButton.Click += ClearButton_Click;
			textBoxItem.KeyPress += TextBoxElement_KeyPress;
			textBoxItem.TextChanging += TextBoxItem_TextChanging;
		}

		private void TextBoxElement_KeyPress(Object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (int) Keys.Enter)
			{
				searchButton.PerformClick();
			}
		}

		private void TextBoxItem_TextChanging(Object sender, TextChangingEventArgs e)
		{

			if (e.NewValue.Length > 0 && e.NewValue.Length != e.OldValue.Length)
				clearButton.Visibility = ElementVisibility.Visible;
			else 
				clearButton.Visibility = ElementVisibility.Hidden;

		}

		public class SearchBoxEventArgs : EventArgs
		{
			public string SearchText { get; set; }

			public bool Clicked { get; set; }

			public SearchBoxEventArgs(string text) 
			{
				this.SearchText = text;
				this.Clicked = true;
			}

			public SearchBoxEventArgs(string text, bool clicked)
			{
				this.SearchText = text;
				this.Clicked = clicked;
			}
		}

		public event EventHandler<SearchBoxEventArgs> Search;

		public event EventHandler Clean;

		private void SearchButton_Click(object sender, EventArgs e)
		{
			if (Text.Length > 0)
			{
				this.SearchButtonClicked = true;
				Search?.Invoke(this, new SearchBoxEventArgs(Text.ToLower().Trim(), SearchButtonClicked));
			}
			else this.SearchButtonClicked = false;
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			if (SearchButtonClicked)
			{
				var cleanEvent = new EventArgs();
				Clean?.Invoke(this, cleanEvent);
				SearchButtonClicked = false;
			}
			this.textBoxItem.Text = String.Empty;
			this.clearButton.Visibility = ElementVisibility.Hidden;
		}

		#endregion
	}
}