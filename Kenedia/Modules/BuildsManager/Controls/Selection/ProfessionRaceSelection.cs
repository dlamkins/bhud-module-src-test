using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class ProfessionRaceSelection : BaseSelection
	{
		public enum SelectionType
		{
			None,
			Profession,
			Race
		}

		private readonly List<ProfessionRaceSelectable> _races = new List<ProfessionRaceSelectable>();

		private readonly List<ProfessionRaceSelectable> _professions = new List<ProfessionRaceSelectable>();

		private SelectionType _type = SelectionType.Race;

		public SelectionType Type
		{
			get
			{
				return _type;
			}
			set
			{
				Common.SetProperty(ref _type, value, new PropertyChangedEventHandler(OnTypeChanged), triggerOnUpdate: true, "Type");
			}
		}

		public Data Data { get; }

		public ProfessionRaceSelection(Data data)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			Data = data;
			Search.Dispose();
			base.BackgroundImage = AsyncTexture2D.FromAssetId(155963);
			SelectionContent.Location = Point.get_Zero();
			SelectionContent.HeightSizingMode = SizingMode.Fill;
			SelectionContent.ShowBorder = false;
			SelectionContent.ContentPadding = new RectangleDimensions(0);
			HeightSizingMode = SizingMode.Standard;
			WidthSizingMode = SizingMode.Standard;
			base.Width = 225;
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(2);
			base.ContentPadding = new RectangleDimensions(5);
			foreach (Races race in Enum.GetValues(typeof(Races)))
			{
				ProfessionRaceSelectable ctrl;
				_races.Add(ctrl = new ProfessionRaceSelectable(data)
				{
					Parent = SelectionContent,
					SelectionType = SelectionType.Race,
					Value = race,
					OnClickAction = delegate(Enum v)
					{
						base.OnClickAction?.Invoke(v);
					}
				});
			}
			foreach (ProfessionType profession in Enum.GetValues(typeof(ProfessionType)))
			{
				ProfessionRaceSelectable ctrl;
				_professions.Add(ctrl = new ProfessionRaceSelectable(data)
				{
					Parent = SelectionContent,
					SelectionType = SelectionType.Profession,
					Value = profession,
					OnClickAction = delegate(Enum v)
					{
						base.OnClickAction?.Invoke(v);
					}
				});
			}
			OnTypeChanged(this, null);
			Control.Input.Mouse.LeftMouseButtonPressed += Mouse_LeftMouseButtonPressed;
		}

		private void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			Hide();
		}

		private void OnTypeChanged(object sender, PropertyChangedEventArgs e)
		{
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			SelectionContent.FilterChildren((ProfessionRaceSelectable e) => e.SelectionType == Type);
			Control ctrl = SelectionContent.Children.FirstOrDefault();
			base.Height = base.ContentPadding.Vertical + SelectionContent.Children.Where((Control e) => e.Visible).Count() * (Math.Max(ctrl.Height, 48) + (int)SelectionContent.ControlPadding.Y);
			SelectionContent.Invalidate();
		}

		public override void RecalculateLayout()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			base.TextureRectangle = new Rectangle(25, 25, base.Width, base.Height);
		}

		protected override void OnSelectionContent_Resized(object sender, ResizedEventArgs e)
		{
			base.OnSelectionContent_Resized(sender, e);
			foreach (Control child in SelectionContent.Children)
			{
				child.Width = SelectionContent.Width;
			}
		}
	}
}
