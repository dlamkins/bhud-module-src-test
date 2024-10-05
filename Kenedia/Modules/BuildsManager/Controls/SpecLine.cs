using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SpecLine : Control
	{
		private readonly double _ratio = 4.792592592592593;

		private readonly DetailedTexture _baseFrame = new DetailedTexture(993595)
		{
			TextureRegion = new Rectangle(0, 0, 647, 135)
		};

		private readonly DetailedTexture _eliteFrame = new DetailedTexture(993596)
		{
			TextureRegion = new Rectangle(0, 0, 647, 135)
		};

		private readonly DetailedTexture _background = new DetailedTexture(993593)
		{
			TextureRegion = new Rectangle(0, 0, 647, 135)
		};

		private readonly DetailedTexture _specializationBackground = new DetailedTexture(993593)
		{
			TextureRegion = new Rectangle(0, 120, 647, 135)
		};

		private readonly DetailedTexture _selector = new DetailedTexture(993583, 993584);

		private readonly DetailedTexture _hexagon = new DetailedTexture(993598);

		private readonly DetailedTexture _noSpecHexagon = new DetailedTexture(993597);

		private readonly TraitIcon _weaponTrait = new TraitIcon();

		private readonly Dictionary<int, TraitIcon> _minors = new Dictionary<int, TraitIcon>
		{
			{
				0,
				new TraitIcon()
			},
			{
				1,
				new TraitIcon()
			},
			{
				2,
				new TraitIcon()
			}
		};

		private readonly Dictionary<int, TraitIcon> _majors = new Dictionary<int, TraitIcon>
		{
			{
				0,
				new TraitIcon()
			},
			{
				1,
				new TraitIcon()
			},
			{
				2,
				new TraitIcon()
			},
			{
				3,
				new TraitIcon()
			},
			{
				4,
				new TraitIcon()
			},
			{
				5,
				new TraitIcon()
			},
			{
				6,
				new TraitIcon()
			},
			{
				7,
				new TraitIcon()
			},
			{
				8,
				new TraitIcon()
			}
		};

		private double _scale = 4.792592592592593;

		private Dictionary<int, Trait> _minorsTraits = new Dictionary<int, Trait>();

		private Dictionary<int, Trait> _majorTraits = new Dictionary<int, Trait>();

		private bool _selectorOpen;

		private readonly TraitTooltip _traitTooltip;

		private readonly Tooltip _basicTooltip;

		private readonly Kenedia.Modules.Core.Controls.Label _basicTooltipLabel;

		private Rectangle _specSelectorBounds;

		private readonly List<(Specialization spec, Rectangle bounds)> _specBounds = new List<(Specialization, Rectangle)>();

		public bool SelectorOpen
		{
			get
			{
				return _selectorOpen;
			}
			set
			{
				Common.SetProperty(ref _selectorOpen, value, new ValueChangedEventHandler<bool>(OnSelectorToggled));
			}
		}

		private new string BasicTooltipText
		{
			get
			{
				return _basicTooltipLabel.Text;
			}
			set
			{
				_basicTooltipLabel.Text = value;
			}
		}

		public Func<bool> CanInteract { get; set; } = () => true;


		public TemplatePresenter TemplatePresenter { get; }

		public Data Data { get; }

		public BuildSpecialization? BuildSpecialization => TemplatePresenter?.Template?[SpecializationSlot];

		public SpecializationSlotType SpecializationSlot { get; private set; }

		private void OnSelectorToggled(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<bool> e)
		{
			TraitTooltip tooltip = base.Tooltip as TraitTooltip;
			if (tooltip != null)
			{
				tooltip.Trait = null;
			}
		}

		public SpecLine(SpecializationSlotType line, TemplatePresenter templatePresenter, Data data)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			TemplatePresenter = templatePresenter;
			Data = data;
			base.Tooltip = (_traitTooltip = new TraitTooltip());
			_basicTooltip = new Tooltip();
			_basicTooltipLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = _basicTooltip,
				AutoSizeHeight = true,
				AutoSizeWidth = true
			};
			SpecializationSlot = line;
			base.Height = 165;
			base.BackgroundColor = new Color(48, 48, 48);
			Control.Input.Mouse.LeftMouseButtonPressed += MouseMouseButtonPressed;
			Control.Input.Mouse.RightMouseButtonPressed += MouseMouseButtonPressed;
			int size = Scale(72);
			int offset = 40;
			for (int i = 0; i < ((SpecializationSlot == SpecializationSlotType.Line_3) ? 8 : 5); i++)
			{
				_specBounds.Add(((Specialization)null, new Rectangle(offset, (base.Height - size) / 2, size, size)));
				offset += size + Scale(10);
			}
			TemplatePresenter.SpecializationChanged += new SpecializationChangedEventHandler(OnSpecializationChanged);
			TemplatePresenter.TemplateChanged += new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
			TemplatePresenter.TraitChanged += new TraitChangedEventHandler(TemplatePresenter_TraitChanged);
			SetSpecialization();
		}

		private void TemplatePresenter_TraitChanged(object sender, TraitChangedEventArgs e)
		{
			if (e.SpecSlot == SpecializationSlot)
			{
				UpdateTraitsForSpecialization();
			}
		}

		private void UpdateTraitsForSpecialization()
		{
			_minorsTraits = BuildSpecialization!.Specialization?.MinorTraits.ToDictionary<KeyValuePair<int, Trait>, int, Trait>((KeyValuePair<int, Trait> e) => e.Value.Index, (KeyValuePair<int, Trait> e) => e.Value);
			_majorTraits = BuildSpecialization!.Specialization?.MajorTraits.ToDictionary<KeyValuePair<int, Trait>, int, Trait>((KeyValuePair<int, Trait> e) => e.Value.Index, (KeyValuePair<int, Trait> e) => e.Value);
			Trait trait = default(Trait);
			for (int i = 0; i < _minors.Count; i++)
			{
				_minors[i].Trait = ((_minorsTraits?.TryGetValue(i, out trait) ?? false) ? trait : null);
			}
			Trait trait2 = default(Trait);
			for (int j = 0; j < _majors.Count; j++)
			{
				_majors[j].Trait = ((_majorTraits?.TryGetValue(j, out trait2) ?? false) ? trait2 : null);
				_majors[j].Selected = _majors[j].Trait != null && BuildSpecialization!.Traits[_majors[j].Trait.Tier] == _majors[j].Trait;
			}
		}

		private void TemplatePresenter_TemplateChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Template> e)
		{
			SetSpecialization();
		}

		private void OnSpecializationChanged(object sender, SpecializationChangedEventArgs e)
		{
			if (e.Slot == SpecializationSlot)
			{
				SetSpecialization();
			}
		}

		private void MouseMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (!base.MouseOver)
			{
				SelectorOpen = false;
			}
		}

		public override void RecalculateLayout()
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int ratioWidth = (int)Math.Ceiling((double)base.Height * _ratio);
			int ratioHeight = (int)Math.Ceiling((double)base.Width / _ratio);
			if (ratioWidth != base.Width)
			{
				base.Width = ratioWidth;
			}
			else if (ratioHeight != base.Height)
			{
				base.Height = ratioHeight;
			}
			_scale = (double)base.Height / 149.0;
			_baseFrame.Bounds = new Rectangle(0, 0, base.Width, base.Height);
			_eliteFrame.Bounds = new Rectangle(0, 0, base.Width, base.Height);
			_background.Bounds = new Rectangle(0, 0, base.Width, base.Height);
			_specializationBackground.Bounds = new Rectangle(0, 0, base.Width, base.Height);
			_hexagon.Bounds = new Rectangle(Scale(64), Scale(4), base.Height - Scale(8), base.Height - Scale(8));
			_noSpecHexagon.Bounds = new Rectangle(Scale(64), Scale(4), base.Height - Scale(8), base.Height - Scale(8));
			TraitIcon weaponTrait = _weaponTrait;
			Rectangle val = _hexagon.Bounds;
			int num = ((Rectangle)(ref val)).get_Right() - Scale(46) - Scale(20);
			val = _hexagon.Bounds;
			weaponTrait.Bounds = new Rectangle(num, ((Rectangle)(ref val)).get_Bottom() - Scale(46) - Scale(8), Scale(46), Scale(46));
			_selector.Bounds = new Rectangle(0, 0, Scale(18), base.Height);
			for (int j = 0; j < _minors.Count; j++)
			{
				TraitIcon traitIcon = _minors[j];
				int num2 = Scale(225) + j * Scale(160);
				val = base.LocalBounds;
				traitIcon.Bounds = new Rectangle(num2, ((Rectangle)(ref val)).get_Center().Y - Scale(42) / 2, Scale(42), Scale(42));
			}
			for (int i = 0; i < _majors.Count; i++)
			{
				int row = i - (int)Math.Floor((double)i / 3.0) * 3;
				_majors[i].Bounds = new Rectangle(Scale(300) + (int)Math.Floor((double)i / 3.0) * Scale(160), Scale(8) + row * Scale(46), Scale(42), Scale(42));
			}
			val = _selector.Bounds;
			int right = ((Rectangle)(ref val)).get_Right();
			int width = base.Width;
			val = _selector.Bounds;
			_specSelectorBounds = new Rectangle(right, 0, width - ((Rectangle)(ref val)).get_Right(), base.Height);
		}

		private void SetSpecialization()
		{
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			_ = GameService.Gw2Mumble.PlayerCharacter;
			ProfessionType? professionType = TemplatePresenter?.Template?.Profession;
			if (!professionType.HasValue)
			{
				return;
			}
			ProfessionType templateProfession = professionType.GetValueOrDefault();
			Profession profession = default(Profession);
			if (!(Data?.Professions?.TryGetValue(templateProfession, out profession) ?? false))
			{
				return;
			}
			int i = 0;
			bool isEliteSpecLine = SpecializationSlot == SpecializationSlotType.Line_3;
			foreach (Specialization s in from x in profession.Specializations.Values
				where !x.Elite || isEliteSpecLine
				orderby x.Elite, x.Id
				select x)
			{
				if (!s.Elite || SpecializationSlot == SpecializationSlotType.Line_3)
				{
					_specBounds[i] = (s, _specBounds[i].bounds);
					i++;
				}
			}
			_weaponTrait.Texture = BuildSpecialization?.Specialization?.WeaponTrait?.Icon;
			_specializationBackground.Texture = BuildSpecialization?.Specialization?.Background;
			UpdateTraitsForSpecialization();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			_ = string.Empty;
			bool flag = ((CanInteract?.Invoke() ?? true) ? true : false);
			bool hasSpec = BuildSpecialization != null && BuildSpecialization!.Specialization != null;
			Point? hoverPos = (flag ? new Point?(base.RelativeMousePosition) : null);
			BasicTooltipText = null;
			if (BuildSpecialization != null && BuildSpecialization!.Specialization != null && !SelectorOpen)
			{
				_traitTooltip.Trait = null;
				_specializationBackground.Draw(this, spriteBatch);
				Rectangle minor = _minors[0].Bounds;
				Rectangle val = _hexagon.Bounds;
				float num = ((Rectangle)(ref val)).get_Right() - Scale(18) + base.AbsoluteBounds.X;
				val = _hexagon.Bounds;
				ShapeExtensions.DrawLine(spriteBatch, new Vector2(num, (float)(((Rectangle)(ref val)).get_Center().Y + base.AbsoluteBounds.Y)), new Vector2((float)(((Rectangle)(ref minor)).get_Left() + Scale(3) + base.AbsoluteBounds.X), (float)(((Rectangle)(ref minor)).get_Center().Y + base.AbsoluteBounds.Y)), ContentService.Colors.ColonialWhite * 0.8f, (float)Scale(3), 0f);
				for (int i = 0; i < _majors.Count; i++)
				{
					Rectangle major = _majors[i].Bounds;
					if (_majors[i].Trait == null)
					{
						continue;
					}
					minor = _minors[(int)(_majors[i].Trait.Tier - 1)].Bounds;
					if (_majors[i].Selected)
					{
						Rectangle? minorNext = (_minors.ContainsKey((int)_majors[i].Trait.Tier) ? new Rectangle?(_minors[(int)_majors[i].Trait.Tier].Bounds) : null);
						ShapeExtensions.DrawLine(spriteBatch, new Vector2((float)(((Rectangle)(ref minor)).get_Right() - Scale(2) + base.AbsoluteBounds.X), (float)(((Rectangle)(ref minor)).get_Center().Y + base.AbsoluteBounds.Y)), new Vector2((float)(((Rectangle)(ref major)).get_Left() + Scale(2) + base.AbsoluteBounds.X), (float)(((Rectangle)(ref major)).get_Center().Y + base.AbsoluteBounds.Y)), ContentService.Colors.ColonialWhite * 0.8f, (float)Scale(2), 0f);
						if (minorNext.HasValue)
						{
							Vector2 val2 = new Vector2((float)(((Rectangle)(ref major)).get_Right() - Scale(2) + base.AbsoluteBounds.X), (float)(((Rectangle)(ref major)).get_Center().Y + base.AbsoluteBounds.Y));
							val = minorNext.Value;
							float num2 = ((Rectangle)(ref val)).get_Left() + Scale(2) + base.AbsoluteBounds.X;
							val = minorNext.Value;
							ShapeExtensions.DrawLine(spriteBatch, val2, new Vector2(num2, (float)(((Rectangle)(ref val)).get_Center().Y + base.AbsoluteBounds.Y)), ContentService.Colors.ColonialWhite * 0.8f, (float)Scale(2), 0f);
						}
					}
				}
				for (int j = 0; j < _minors.Count; j++)
				{
					_minors[j].Draw(this, spriteBatch, hoverPos, null, null, SelectorOpen ? new bool?(false) : null);
					if (_minors[j].Hovered)
					{
						_traitTooltip.Trait = _minors[j].Trait;
					}
				}
				for (int k = 0; k < _majors.Count; k++)
				{
					_majors[k].Draw(this, spriteBatch, hoverPos, _majors[k].Selected ? Color.get_White() : (_majors[k].Hovered ? Color.get_DarkGray() : (Color.get_White() * 0.6f)), _majors[k].Selected ? null : new Color?(_majors[k].Hovered ? (Color.get_Gray() * 0.1f) : (Color.get_Black() * 0.5f)), SelectorOpen ? new bool?(false) : null);
					if (_majors[k].Hovered)
					{
						_traitTooltip.Trait = _majors[k].Trait;
					}
				}
			}
			_baseFrame.Draw(this, spriteBatch);
			_selector.Draw(this, spriteBatch, hoverPos, null, null, SelectorOpen ? new bool?(true) : null);
			if (_selector.Hovered)
			{
				BasicTooltipText = "Change Specialization";
			}
			(hasSpec ? _hexagon : _noSpecHexagon).Draw(this, spriteBatch, hoverPos);
			if (SpecializationSlot == SpecializationSlotType.Line_3)
			{
				_eliteFrame.Draw(this, spriteBatch);
			}
			_weaponTrait.Draw(this, spriteBatch, hoverPos, null, null, SelectorOpen ? new bool?(false) : null);
			if (_weaponTrait.Hovered)
			{
				_traitTooltip.Trait = _weaponTrait.Trait;
			}
			if (SelectorOpen)
			{
				BasicTooltipText = DrawSelector(spriteBatch, bounds) ?? BasicTooltipText;
			}
			base.Tooltip = (SelectorOpen ? _basicTooltip : _traitTooltip);
			_basicTooltip.Opacity = (string.IsNullOrEmpty(BasicTooltipText) ? 0f : 1f);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			if (!(CanInteract?.Invoke() ?? false))
			{
				return;
			}
			if (!SelectorOpen)
			{
				TraitIcon trait = _majors.FirstOrDefault<KeyValuePair<int, TraitIcon>>((KeyValuePair<int, TraitIcon> e) => e.Value.Hovered).Value;
				for (int i = 0; i < _majors.Count; i++)
				{
					if (trait != null && _majors[i].Trait.Tier == trait.Trait.Tier)
					{
						_majors[i].Selected = trait == _majors[i] && !_majors[i].Selected;
					}
				}
				if (trait != null)
				{
					TemplatePresenter.Template?.SetTrait(SpecializationSlot, trait.Trait, trait.Trait.Tier);
					return;
				}
			}
			else
			{
				try
				{
					foreach (var spec in _specBounds.ToList())
					{
						Rectangle item = spec.bounds;
						if (((Rectangle)(ref item)).Contains(base.RelativeMousePosition) && spec.spec != null)
						{
							TemplatePresenter.Template.SetSpecialization(SpecializationSlot, spec.spec);
						}
					}
				}
				catch (Exception ex)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn($"{ex}");
				}
			}
			SelectorOpen = (_hexagon.Hovered || _noSpecHexagon.Hovered || _selector.Hovered) && !SelectorOpen;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Control.Input.Mouse.LeftMouseButtonPressed -= MouseMouseButtonPressed;
			Control.Input.Mouse.RightMouseButtonPressed -= MouseMouseButtonPressed;
			TemplatePresenter.SpecializationChanged -= new SpecializationChangedEventHandler(OnSpecializationChanged);
			TemplatePresenter.TemplateChanged -= new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
			TemplatePresenter.TraitChanged -= new TraitChangedEventHandler(TemplatePresenter_TraitChanged);
			_baseFrame?.Dispose();
			_eliteFrame?.Dispose();
			_background?.Dispose();
			_specializationBackground?.Dispose();
			_selector?.Dispose();
			_hexagon?.Dispose();
			_noSpecHexagon?.Dispose();
			_weaponTrait?.Dispose();
			_minors?.Values?.DisposeAll();
			_majors?.Values?.DisposeAll();
		}

		private int Scale(int input)
		{
			return (int)Math.Ceiling((double)input * _scale);
		}

		private string? DrawSelector(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			string txt = null;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _specSelectorBounds, Rectangle.get_Empty(), Color.get_Black() * 0.8f, 0f, Vector2.get_Zero(), (SpriteEffects)0);
			foreach (var spec in _specBounds)
			{
				Rectangle item = spec.bounds;
				bool hovered = ((Rectangle)(ref item)).Contains(base.RelativeMousePosition);
				TemplatePresenter templatePresenter = TemplatePresenter;
				BuildSpecialization slot;
				bool hasSpec = templatePresenter != null && templatePresenter.Template?.HasSpecialization(spec.spec, out slot) == true;
				if (spec.spec != null)
				{
					spriteBatch.DrawOnCtrl(this, spec.spec.Icon, spec.bounds, spec.spec.Icon.Bounds, hasSpec ? ContentService.Colors.Chardonnay : (hovered ? Color.get_White() : (Color.get_White() * 0.8f)), 0f, Vector2.get_Zero(), (SpriteEffects)0);
					if (hovered)
					{
						txt = spec.spec.Name;
					}
					if (hasSpec)
					{
						spriteBatch.DrawOnCtrl(this, spec.spec.Icon, spec.bounds, spec.spec.Icon.Bounds.Add(-4, -4, 8, 8), Color.get_Black() * 0.7f, 0f, Vector2.get_Zero(), (SpriteEffects)0);
					}
				}
			}
			return txt;
		}
	}
}
