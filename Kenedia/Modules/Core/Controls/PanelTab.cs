using System;
using System.Runtime.CompilerServices;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Controls
{
	public class PanelTab : Panel
	{
		private new AsyncTexture2D _icon;

		private Rectangle _textureRectangle = Rectangle.Empty;

		public TabButton TabButton { get; private set; }

		public new AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				_icon = value;
				TabButton.Icon = Icon;
				this.IconChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public new Rectangle TextureRectangle
		{
			get
			{
				return _textureRectangle;
			}
			set
			{
				_textureRectangle = value;
				TabButton.TextureRectangle = value;
				this.TextureRectangleChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _003CName_003Ek__BackingField;
			}
			set
			{
				_003CName_003Ek__BackingField = value;
				TabButton.BasicTooltipText = value;
			}
		}

		public bool Active
		{
			[CompilerGenerated]
			get
			{
				return _003CActive_003Ek__BackingField;
			}
			set
			{
				_003CActive_003Ek__BackingField = value;
				TabButton.Active = value;
				if (value)
				{
					OnActivated();
				}
				else
				{
					OnDeactivated();
				}
			}
		}

		private event EventHandler Activated;

		private event EventHandler TextureRectangleChanged;

		private event EventHandler Deactivated;

		private event EventHandler IconChanged;

		public PanelTab()
		{
			TabButton = new TabButton
			{
				BasicTooltipText = Name
			};
		}

		protected void OnActivated()
		{
			Show();
			this.Activated?.Invoke(this, EventArgs.Empty);
		}

		protected void OnDeactivated()
		{
			Hide();
			this.Deactivated?.Invoke(this, EventArgs.Empty);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			TabButton?.Dispose();
			_icon = null;
		}
	}
}
