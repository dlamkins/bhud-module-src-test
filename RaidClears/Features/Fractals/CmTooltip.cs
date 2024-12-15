using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Features.Fractals.Models;
using RaidClears.Features.Fractals.Services;
using RaidClears.Utils.Kenedia;

namespace RaidClears.Features.Fractals
{
	public class CmTooltip : Tooltip
	{
		private readonly DetailedTexture _image = new DetailedTexture();

		private readonly Label _title;

		private readonly Label _id;

		private readonly Label _instabs;

		private readonly Label _tomorrowInstabs;

		private readonly List<DetailedTexture> _instabIcons = new List<DetailedTexture>();

		private readonly List<string> _instabNames = new List<string>();

		private readonly Rectangle _instabsTitle = new Rectangle(4, 53, 150, 32);

		private readonly Rectangle _tomorrowInstabsTitle = new Rectangle(186, 53, 150, 32);

		private CMInterface _cmInterface;

		public CMInterface Fractal
		{
			get
			{
				return _cmInterface;
			}
			set
			{
				Common.SetProperty(ref _cmInterface, value, new ValueChangedEventHandler<CMInterface>(ApplyFractal));
			}
		}

		public CmTooltip()
			: this()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			_image.Texture = Service.Textures!.SettingTabFractals;
			DetailedTexture image = _image;
			Rectangle imageBounds = default(Rectangle);
			((Rectangle)(ref imageBounds))._002Ector(4, 4, 48, 48);
			image.Bounds = imageBounds;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Height(Control.get_Content().get_DefaultFont16().get_LineHeight());
			((Control)val).set_Width(300 - _image.Bounds.X);
			((Control)val).set_Location(new Point(((Rectangle)(ref imageBounds)).get_Right(), ((Rectangle)(ref imageBounds)).get_Top()));
			val.set_Font(Control.get_Content().get_DefaultFont16());
			val.set_TextColor(Colors.Chardonnay);
			_title = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Height(Control.get_Content().get_DefaultFont12().get_LineHeight());
			((Control)val2).set_Width(300 - _image.Bounds.X);
			((Control)val2).set_Location(new Point(((Rectangle)(ref imageBounds)).get_Right(), ((Control)_title).get_Bottom()));
			val2.set_Font(Control.get_Content().get_DefaultFont12());
			val2.set_TextColor(Color.get_White() * 0.8f);
			_id = val2;
		}

		private void ApplyFractal(object sender, ValueChangedEventArgs<CMInterface> e)
		{
			_instabNames.Clear();
			_instabIcons.ForEach(delegate(DetailedTexture i)
			{
				i.Dispose();
			});
			_instabIcons.Clear();
			FractalMap map = e.NewValue!.Map;
			int scale = e.NewValue!.Scale;
			int day = e.NewValue!.DayOfyear;
			List<string> instabs = Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, day);
			List<string> tomorrowInstabs = Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, (day + 1) % 366);
			_instabNames.AddRange(instabs.Concat(tomorrowInstabs).ToList());
			List<int> instabilityAssetIdByNames = Service.FractalMapData.GetInstabilityAssetIdByNames(_instabNames);
			int index = 0;
			instabilityAssetIdByNames.ForEach(delegate(int id)
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				DetailedTexture detailedTexture = new DetailedTexture(id);
				Rectangle bounds = _image.Bounds;
				int num = ((Rectangle)(ref bounds)).get_Left() + ((index >= 3) ? 187 : 0);
				bounds = _image.Bounds;
				detailedTexture.Bounds = new Rectangle(num, ((Rectangle)(ref bounds)).get_Bottom() + 32 + 32 * (index % 3) + 5, 32, 32);
				_instabIcons.Add(detailedTexture);
				index++;
			});
			_title.set_Text(map.Label + " (" + Service.FractalPersistance.GetEncounterLabel(map.ApiLabel) + ")");
			_id.set_Text($"Scale: {scale}");
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Draw(spriteBatch, drawBounds, scissor);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatch spriteBatch2 = spriteBatch;
			((Tooltip)this).PaintBeforeChildren(spriteBatch2, bounds);
			_image.Draw((Control)(object)this, spriteBatch2);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch2, (Control)(object)this, "Instabilities", GameService.Content.get_DefaultFont14(), _instabsTitle, Color.get_Chartreuse(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch2, (Control)(object)this, "Tomorrow", GameService.Content.get_DefaultFont14(), _tomorrowInstabsTitle, Color.get_Chartreuse(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			int i = 0;
			_instabIcons.ForEach(delegate(DetailedTexture icon)
			{
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
				icon.Draw((Control)(object)this, spriteBatch2);
				try
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch2, (Control)(object)this, _instabNames[i], GameService.Content.get_DefaultFont14(), new Rectangle(icon.Bounds.X + icon.Size.X + 5, icon.Bounds.Y, 125, icon.Bounds.Height), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				catch (Exception)
				{
				}
				i++;
			});
		}

		public override void RecalculateLayout()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(370, 200));
			((Container)this).set_ContentRegion(new Rectangle(5, 5, 360, 190));
		}

		protected override void DisposeControl()
		{
			_image.Texture = null;
			((Tooltip)this).DisposeControl();
		}
	}
}
