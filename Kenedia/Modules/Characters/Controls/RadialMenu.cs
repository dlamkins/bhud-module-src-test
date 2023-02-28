using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Extensions;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Kenedia.Modules.Characters.Controls
{
	public class RadialMenu : Control
	{
		private readonly Data _data;

		private readonly AsyncTexture2D _dummy = AsyncTexture2D.FromAssetId(1128572);

		private readonly CharacterTooltip _tooltip;

		private readonly Settings _settings;

		private readonly ObservableCollection<Character_Model> _characters;

		private readonly Func<Character_Model> _currentCharacter;

		private List<Character_Model> _displayedCharacters;

		private int _iconSize;

		private readonly List<RadialMenuSection> _sections = new List<RadialMenuSection>();

		private RadialMenuSection? _selected;

		private Point _center;

		private Character_Model CurrentCharacter => _currentCharacter?.Invoke();

		public RadialMenu(Settings settings, ObservableCollection<Character_Model> characters, Container parent, Func<Character_Model> currentCharacter, Data data, TextureManager textureManager)
			: this()
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			_settings = settings;
			_characters = characters;
			_currentCharacter = currentCharacter;
			_data = data;
			((Control)this).set_Parent(parent);
			CharacterTooltip characterTooltip = new CharacterTooltip(currentCharacter, textureManager, data, settings);
			((Control)characterTooltip).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)characterTooltip).set_ZIndex(1073741824);
			((Control)characterTooltip).set_Size(new Point(300, 50));
			((Control)characterTooltip).set_Visible(false);
			_tooltip = characterTooltip;
			foreach (Character_Model character in _characters)
			{
				character.Updated += Character_Updated;
			}
			((Control)((Control)this).get_Parent()).add_Resized((EventHandler<ResizedEventArgs>)Parent_Resized);
			Control.get_Input().get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)Keyboard_KeyPressed);
		}

		public bool HasDisplayedCharacters()
		{
			if (_characters != null)
			{
				_displayedCharacters = ((_characters.Count > 0) ? _characters.Where((Character_Model e) => e.ShowOnRadial).ToList() : new List<Character_Model>());
			}
			return _displayedCharacters.Count() > 0;
		}

		private void Keyboard_KeyPressed(object sender, KeyboardEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			if ((int)e.get_Key() == 27)
			{
				((Control)this).Hide();
			}
		}

		private void Parent_Resized(object sender, ResizedEventArgs e)
		{
			((Control)this).RecalculateLayout();
		}

		private void Character_Updated(object sender, EventArgs e)
		{
			((Control)this).RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			if (((Control)this).get_Parent() == null)
			{
				return;
			}
			_displayedCharacters = ((_characters.Count > 0) ? _characters.Where((Character_Model e) => e.ShowOnRadial).ToList() : new List<Character_Model>());
			_displayedCharacters.Sort((Character_Model a, Character_Model b) => a.Name.CompareTo(b.Name));
			int num = (int)((float)Math.Min(((Control)((Control)this).get_Parent()).get_Width(), ((Control)((Control)this).get_Parent()).get_Height()) * _settings.Radial_Scale.get_Value());
			((Control)this).set_Size(new Point(((Control)((Control)this).get_Parent()).get_Width(), ((Control)((Control)this).get_Parent()).get_Height()));
			_sections.Clear();
			_center = ((Control)this).get_RelativeMousePosition();
			int amount = _displayedCharacters.Count;
			_ = Math.PI * 2.0 / (double)amount;
			int radius = num / 2;
			_iconSize = Math.Min(105, (int)((double)radius * (Math.Sqrt(2.0) / (double)amount)) * 3);
			if (amount > 2)
			{
				List<Vector2> points = new List<Vector2>();
				Vector2 c = ((Point)(ref _center)).ToVector2();
				Vector2 p2 = default(Vector2);
				for (int j = 0; j < amount; j++)
				{
					((Vector2)(ref p2))._002Ector((float)(_center.X - (int)((double)radius * Math.Sin(Math.PI * 2.0 * (double)j / (double)amount))), (float)(_center.Y - (int)((double)radius * Math.Cos(Math.PI * 2.0 * (double)j / (double)amount))));
					points.Add(p2);
				}
				Vector2 v = default(Vector2);
				for (int i = 0; i < points.Count; i++)
				{
					Vector2 a2 = points[i];
					Vector2 b2 = ((i == points.Count - 1) ? points[0] : points[i + 1]);
					Triangle t = new Triangle(a2, b2, c);
					((Vector2)(ref v))._002Ector((a2.X + b2.X + c.X) / 3f, (a2.Y + b2.Y + c.Y) / 3f);
					Point p = ((Vector2)(ref v)).ToPoint();
					Math.Min(((Vector2)(ref a2)).ToPoint().Distance2D(((Vector2)(ref v)).ToPoint()), Math.Min(((Vector2)(ref b2)).ToPoint().Distance2D(((Vector2)(ref v)).ToPoint()), ((Vector2)(ref c)).ToPoint().Distance2D(((Vector2)(ref v)).ToPoint())));
					_sections.Add(new RadialMenuSection
					{
						Character = _displayedCharacters[i],
						Triangle = t,
						Lines = t.DrawingPoints(),
						IconPos = v,
						IconRectangle = new Rectangle(p.X - _iconSize / 2, p.Y - _iconSize / 2, _iconSize, _iconSize)
					});
				}
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			((Control)this).OnMouseMoved(e);
			_selected = null;
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			if (!Control.get_Input().get_Keyboard().get_KeysDown()
				.Contains(_settings.RadialKey.get_Value().get_PrimaryKey()))
			{
				((Control)this).Hide();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			Point relativeMousePosition = ((Control)this).get_RelativeMousePosition();
			Vector2 mouse = ((Point)(ref relativeMousePosition)).ToVector2();
			string txt = string.Empty;
			foreach (RadialMenuSection section in _sections)
			{
				if (_selected == null)
				{
					_selected = (section.Triangle.Contains(mouse) ? section : null);
				}
				if (!(((Control)this).get_RelativeMousePosition() == _center) && _selected == null && section.Triangle.Contains(mouse))
				{
					continue;
				}
				if (section.Lines != null)
				{
					foreach (PointF line2 in section.Lines)
					{
						ShapeExtensions.DrawLine(spriteBatch, section.Triangle.Point3, new Vector2(line2.X, line2.Y), _settings.Radial_UseProfessionColor.get_Value() ? (section.Character.Profession.GetData(_data.Professions).Color * 0.7f) : _settings.Radial_IdleColor.get_Value(), 1f, 0f);
					}
					ShapeExtensions.DrawPolygon(spriteBatch, Vector2.get_Zero(), (IReadOnlyList<Vector2>)section.Triangle.ToVectorList(), _settings.Radial_IdleBorderColor.get_Value(), 1f, 0f);
				}
				else
				{
					_ = section.Rectangle;
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), section.Rectangle, (Rectangle?)Rectangle.get_Empty(), _settings.Radial_IdleColor.get_Value(), 0f, default(Vector2), (SpriteEffects)0);
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_settings.Radial_UseProfessionIcons.get_Value() ? section.Character.ProfessionIcon : section.Character.Icon), section.IconRectangle, (Rectangle?)(_settings.Radial_UseProfessionIcons.get_Value() ? section.Character.ProfessionIcon.get_Bounds() : section.Character.Icon.get_Bounds()), _settings.Radial_UseProfessionIconsColor.get_Value() ? section.Character.Profession.GetData(_data.Professions).Color : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			if (_selected != null)
			{
				if (_settings.Radial_ShowAdvancedTooltip.get_Value())
				{
					_tooltip.Character = _selected!.Character;
					((Control)_tooltip).Show();
				}
				else
				{
					txt = _selected!.Character.Name;
				}
				if (_selected!.Lines != null)
				{
					foreach (PointF line in _selected!.Lines)
					{
						ShapeExtensions.DrawLine(spriteBatch, _selected!.Triangle.Point3, new Vector2(line.X, line.Y), _settings.Radial_UseProfessionColor.get_Value() ? _selected!.Character.Profession.GetData(_data.Professions).Color : _settings.Radial_HoveredColor.get_Value(), 1f, 0f);
					}
					ShapeExtensions.DrawPolygon(spriteBatch, Vector2.get_Zero(), (IReadOnlyList<Vector2>)_selected!.Triangle.ToVectorList(), _settings.Radial_HoveredBorderColor.get_Value(), 1f, 0f);
				}
				else
				{
					_ = _selected!.Rectangle;
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _selected!.Rectangle, (Rectangle?)Rectangle.get_Empty(), _settings.Radial_HoveredColor.get_Value(), 0f, default(Vector2), (SpriteEffects)0);
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_settings.Radial_UseProfessionIcons.get_Value() ? _selected!.Character.ProfessionIcon : _selected!.Character.Icon), _selected!.IconRectangle, (Rectangle?)(_settings.Radial_UseProfessionIcons.get_Value() ? _selected!.Character.ProfessionIcon.get_Bounds() : _selected!.Character.Icon.get_Bounds()), _settings.Radial_UseProfessionIconsColor.get_Value() ? _selected!.Character.Profession.GetData(_data.Professions).Color : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			else
			{
				_tooltip.Character = null;
				((Control)_tooltip).Hide();
			}
			if (!_settings.Radial_ShowAdvancedTooltip.get_Value())
			{
				((Control)this).set_BasicTooltipText(txt);
			}
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			_003C_003En__0(e);
			if (_selected != null)
			{
				((Control)this).Hide();
				if (await ExtendedInputService.WaitForNoKeyPressed())
				{
					_selected?.Character.Swap();
				}
			}
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			((Control)this).RecalculateLayout();
			_selected = null;
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).OnHidden(e);
			_tooltip.Character = null;
			((Control)_tooltip).Hide();
		}

		protected override void DisposeControl()
		{
			if (((Control)this).get_Parent() != null)
			{
				((Control)((Control)this).get_Parent()).remove_Resized((EventHandler<ResizedEventArgs>)Parent_Resized);
			}
			foreach (Character_Model character in _characters)
			{
				character.Updated -= Character_Updated;
			}
			CharacterTooltip tooltip = _tooltip;
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			((Control)this).DisposeControl();
		}
	}
}
