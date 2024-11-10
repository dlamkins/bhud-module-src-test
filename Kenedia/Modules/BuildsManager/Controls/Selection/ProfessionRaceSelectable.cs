using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class ProfessionRaceSelectable : Kenedia.Modules.Core.Controls.Panel
	{
		private readonly bool _created;

		private new readonly Kenedia.Modules.Core.Controls.Image _icon;

		private readonly Kenedia.Modules.Core.Controls.Label _name;

		private Enum _value = ProfessionType.Guardian;

		private ProfessionRaceSelection.SelectionType _selectionType = ProfessionRaceSelection.SelectionType.Profession;

		public Enum Value
		{
			get
			{
				return _value;
			}
			set
			{
				Common.SetProperty(ref _value, value, new PropertyChangedEventHandler(SetValue), triggerOnUpdate: true, "Value");
			}
		}

		public Action<Enum> OnClickAction { get; set; }

		public ProfessionRaceSelection.SelectionType SelectionType
		{
			get
			{
				return _selectionType;
			}
			set
			{
				Common.SetProperty(ref _selectionType, value, (Action)delegate
				{
					Value = null;
				}, triggerOnUpdate: true);
			}
		}

		public Data Data { get; }

		public ProfessionRaceSelectable(Data data)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			Data = data;
			HeightSizingMode = SizingMode.AutoSize;
			base.BorderWidth = new RectangleDimensions(2);
			base.BorderColor = Color.get_Black();
			base.BackgroundColor = Color.get_Black() * 0.4f;
			base.HoveredBorderColor = ContentService.Colors.ColonialWhite;
			base.ContentPadding = new RectangleDimensions(5);
			base.ClipInputToBounds = false;
			_name = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Font = Control.Content.DefaultFont18,
				TextColor = Color.get_White(),
				VerticalAlignment = VerticalAlignment.Middle
			};
			_icon = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = this,
				Size = new Point(36),
				Location = new Point(2, 2)
			};
			_created = true;
			Control.Input.Mouse.LeftMouseButtonPressed += Mouse_LeftMouseButtonPressed;
			if (Data.IsLoaded)
			{
				SetValue(this, null);
			}
			else
			{
				Data.Loaded += new EventHandler(Data_Loaded);
			}
		}

		private void Data_Loaded(object sender, EventArgs e)
		{
			SetValue(this, null);
		}

		private void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Hovered && this.IsVisible())
			{
				OnClickAction?.Invoke(Value);
			}
		}

		public override void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			base.UserLocale_SettingChanged(sender, e);
		}

		private void SetValue(object sender, PropertyChangedEventArgs e)
		{
			if (Value == null)
			{
				_name.Text = null;
				_icon.Texture = null;
				return;
			}
			switch (SelectionType)
			{
			case ProfessionRaceSelection.SelectionType.Profession:
			{
				if (Data.Professions.TryGetValue((ProfessionType)(object)Value, out var profession))
				{
					_name.SetLocalizedText = () => profession?.Name;
					_icon.Texture = profession.IconBig;
				}
				break;
			}
			case ProfessionRaceSelection.SelectionType.Race:
			{
				if (Data.Races.TryGetValue((Races)(object)Value, out var race))
				{
					_name.SetLocalizedText = () => race?.Name;
					_icon.Texture = race.Icon;
				}
				break;
			}
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			if (_created)
			{
				_name.SetLocation(_icon.Right + 10, _icon.Top - 2);
				_name.SetSize(base.Right - _icon.Right, _icon.Height);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Control.Input.Mouse.LeftMouseButtonPressed -= Mouse_LeftMouseButtonPressed;
		}
	}
}
