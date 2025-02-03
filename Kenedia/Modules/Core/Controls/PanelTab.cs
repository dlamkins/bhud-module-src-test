using System;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Controls
{
	public class PanelTab : Panel
	{
		private new AsyncTexture2D _icon;

		private Rectangle _textureRectangle = Rectangle.get_Empty();

		private bool _active;

		private string _name;

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
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textureRectangle;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				_textureRectangle = value;
				TabButton.TextureRectangle = value;
				this.TextureRectangleChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				TabButton.BasicTooltipText = value;
			}
		}

		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				_active = value;
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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
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
