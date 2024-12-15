using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Utils.Kenedia;

namespace RaidClears.Features.Raids
{
	public class RaidTooltipView : Tooltip
	{
		private readonly Label _title;

		private readonly Label _id;

		private readonly Image _icon;

		private RaidEncounter _encounter;

		private StrikeMission _strikeMission;

		public RaidEncounter Encoutner
		{
			get
			{
				return _encounter;
			}
			set
			{
				Common.SetProperty(ref _encounter, value, new ValueChangedEventHandler<RaidEncounter>(ApplyEncounter));
			}
		}

		public StrikeMission StrikeMission
		{
			get
			{
				return _strikeMission;
			}
			set
			{
				Common.SetProperty(ref _strikeMission, value, new ValueChangedEventHandler<StrikeMission>(ApplyStrikeMission));
			}
		}

		public RaidTooltipView()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Expected O, but got Unknown
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Height(48);
			((Control)val).set_Width(48);
			val.set_Texture(AsyncTexture2D.op_Implicit(Textures.get_Pixel()));
			((Control)val).set_Location(new Point
			{
				X = 0,
				Y = 0
			});
			_icon = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Height(Control.get_Content().get_DefaultFont16().get_LineHeight());
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(((Control)_icon).get_Right() + 5, ((Control)_icon).get_Top() + 5));
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			val2.set_TextColor(Colors.Chardonnay);
			_title = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Height(Control.get_Content().get_DefaultFont12().get_LineHeight());
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Location(new Point(((Control)_title).get_Left(), ((Control)_title).get_Bottom()));
			val3.set_Font(Control.get_Content().get_DefaultFont12());
			val3.set_TextColor(Color.get_White() * 0.8f);
			_id = val3;
		}

		private void ApplyEncounter(object sender, ValueChangedEventArgs<RaidEncounter> e)
		{
			if (e.NewValue != null)
			{
				_title.set_Text(e.NewValue!.Name ?? "");
				_id.set_Text("(" + Service.RaidSettings.GetEncounterLabel(e.NewValue!.ApiId) + ")");
				if (e.NewValue!.AssetId > 0)
				{
					_icon.set_Texture(Service.Textures!.DatAsset(e.NewValue!.AssetId));
				}
			}
		}

		private void ApplyStrikeMission(object sender, ValueChangedEventArgs<StrikeMission> e)
		{
			if (e.NewValue != null)
			{
				_title.set_Text(e.NewValue!.Name ?? "");
				_id.set_Text("(" + Service.StrikeSettings.GetEncounterLabel(e.NewValue!.Id) + ")");
				if (e.NewValue!.AssetId > 0)
				{
					_icon.set_Texture(Service.Textures!.DatAsset(e.NewValue!.AssetId));
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Draw(spriteBatch, drawBounds, scissor);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((Tooltip)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void RecalculateLayout()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(230, 58));
			((Container)this).set_ContentRegion(new Rectangle(5, 5, 220, 48));
		}

		protected override void DisposeControl()
		{
			((Tooltip)this).DisposeControl();
		}
	}
}
