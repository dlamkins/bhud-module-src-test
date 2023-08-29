using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Manlaan.CommanderMarkers.Pathing.Entities
{
	public class EntityText : EntityBillboard
	{
		private CachedStringRender _cachedTextRender;

		private string _text = string.Empty;

		private Color _textColor = Color.get_White();

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (SetProperty(ref _text, value, rebuildEntity: false, "Text"))
				{
					UpdateTextRender();
				}
			}
		}

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _textColor, value, rebuildEntity: false, "TextColor"))
				{
					UpdateTextRender();
				}
			}
		}

		public EntityText(Entity attachedEntity)
			: base(attachedEntity)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			base.AutoResizeBillboard = false;
		}

		private void UpdateTextRender()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			Size2 textSize = GameService.Content.get_DefaultFont32().MeasureString(_text);
			CachedStringRender cachedTextRender = _cachedTextRender;
			if (cachedTextRender != null)
			{
				cachedTextRender.Dispose();
			}
			if (!string.IsNullOrEmpty(_text))
			{
				_cachedTextRender = CachedStringRender.GetCachedStringRender(_text, GameService.Content.get_DefaultFont32(), new Rectangle(0, 0, (int)textSize.Width, (int)textSize.Height), _textColor, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				Rectangle destinationRectangle = _cachedTextRender.get_DestinationRectangle();
				Point size = ((Rectangle)(ref destinationRectangle)).get_Size();
				base.Size = Vector2Extension.ToWorldCoord(((Point)(ref size)).ToVector2()) / 2f;
				base.Texture = _cachedTextRender.get_CachedRender();
			}
		}
	}
}
