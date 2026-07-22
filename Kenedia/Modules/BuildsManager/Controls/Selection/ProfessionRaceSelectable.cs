using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
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

		public Enum Value
		{
			[CompilerGenerated]
			get
			{
				return _003CValue_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CValue_003Ek__BackingField, value, delegate(Enum v)
				{
					_003CValue_003Ek__BackingField = v;
				}, new PropertyChangedEventHandler(SetValue), triggerOnUpdate: true, "Value");
			}
		}

		public Action<Enum> OnClickAction { get; set; }

		public ProfessionRaceSelection.SelectionType SelectionType
		{
			[CompilerGenerated]
			get
			{
				return _003CSelectionType_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSelectionType_003Ek__BackingField, value, (Action<ProfessionRaceSelection.SelectionType>)delegate(ProfessionRaceSelection.SelectionType v)
				{
					_003CSelectionType_003Ek__BackingField = v;
				}, (Action)delegate
				{
					Value = null;
				}, triggerOnUpdate: true);
			}
		}

		public Data Data { get; }

		public ProfessionRaceSelectable(Data data)
		{
			_003CValue_003Ek__BackingField = ProfessionType.Guardian;
			_003CSelectionType_003Ek__BackingField = ProfessionRaceSelection.SelectionType.Profession;
			base._002Ector();
			Data = data;
			HeightSizingMode = SizingMode.AutoSize;
			base.BorderWidth = new RectangleDimensions(2);
			base.BorderColor = Color.Black;
			base.BackgroundColor = Color.Black * 0.4f;
			base.HoveredBorderColor = ContentService.Colors.ColonialWhite;
			base.ContentPadding = new RectangleDimensions(5);
			base.ClipInputToBounds = false;
			_name = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Font = Control.Content.DefaultFont18,
				TextColor = Color.White,
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

		public override void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
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
					_icon.Texture = TexturesService.GetAsyncTexture(profession?.IconBigAssetId);
				}
				break;
			}
			case ProfessionRaceSelection.SelectionType.Race:
			{
				if (Data.Races.TryGetValue((Races)(object)Value, out var race))
				{
					_name.SetLocalizedText = () => race?.Name;
					_icon.Texture = BaseModule<BuildsManager, MainWindow, Settings, Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.ModuleInstance.ContentsManager.GetTexture(race?.IconPath);
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
