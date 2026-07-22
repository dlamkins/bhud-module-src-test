using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
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
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SpecLine : Control
	{
		public static int Specializations;

		public static int EliteSpecializations;

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

		private readonly TraitTooltip _traitTooltip;

		private readonly Tooltip _basicTooltip;

		private readonly Kenedia.Modules.Core.Controls.Label _basicTooltipLabel;

		private Rectangle _specSelectorBounds;

		private readonly List<(Specialization spec, Rectangle bounds, AsyncTexture2D texture)> _specBounds = new List<(Specialization, Rectangle, AsyncTexture2D)>();

		public bool SelectorOpen
		{
			[CompilerGenerated]
			get
			{
				return _003CSelectorOpen_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSelectorOpen_003Ek__BackingField, value, delegate(bool v)
				{
					_003CSelectorOpen_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<bool>(OnSelectorToggled));
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
			TemplatePresenter = templatePresenter;
			Data = data;
			Data.Loaded += new EventHandler(Data_Loaded);
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
			if (Data.IsLoaded)
			{
				Data_Loaded(this, EventArgs.Empty);
			}
			Control.Input.Mouse.LeftMouseButtonPressed += MouseMouseButtonPressed;
			Control.Input.Mouse.RightMouseButtonPressed += MouseMouseButtonPressed;
			TemplatePresenter.SpecializationChanged += new SpecializationChangedEventHandler(OnSpecializationChanged);
			TemplatePresenter.TemplateChanged += new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
			TemplatePresenter.TraitChanged += new TraitChangedEventHandler(TemplatePresenter_TraitChanged);
			for (int i = 0; i < 20; i++)
			{
				_specBounds.Add((null, new Rectangle(0, 0, 0, 0), null));
			}
		}

		private void Data_Loaded(object sender, EventArgs e)
		{
			_specBounds.Clear();
			EliteSpecializations = ((EliteSpecializations > 0) ? EliteSpecializations : Data.Professions[ProfessionType.Guardian].Specializations.Count<KeyValuePair<int, Specialization>>((KeyValuePair<int, Specialization> e) => e.Value.Elite));
			Specializations = ((Specializations > 0) ? Specializations : (Data.Professions[ProfessionType.Guardian].Specializations.Count - EliteSpecializations));
			int size = Scale(60);
			int offset = 40;
			int common_row_y = ((SpecializationSlot == SpecializationSlotType.Line_3) ? ((base.Height - size * 2) / 2) : ((base.Height - size) / 2));
			int elite_row_y = (base.Height - size * 2) / 2 + size;
			for (int j = 0; j < Specializations; j++)
			{
				_specBounds.Add((null, new Rectangle(offset, common_row_y, size, size), null));
				offset += size + Scale(10);
			}
			if (SpecializationSlot == SpecializationSlotType.Line_3)
			{
				offset = 40 + size / 2;
				for (int i = 0; i < EliteSpecializations; i++)
				{
					_specBounds.Add((null, new Rectangle(offset, elite_row_y, size, size), null));
					offset += size + Scale(10);
				}
			}
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
			_weaponTrait.Bounds = new Rectangle(_hexagon.Bounds.Right - Scale(46) - Scale(20), _hexagon.Bounds.Bottom - Scale(46) - Scale(8), Scale(46), Scale(46));
			_selector.Bounds = new Rectangle(0, 0, Scale(18), base.Height);
			for (int i = 0; i < _minors.Count; i++)
			{
				_minors[i].Bounds = new Rectangle(Scale(225) + i * Scale(160), base.LocalBounds.Center.Y - Scale(42) / 2, Scale(42), Scale(42));
			}
			for (int j = 0; j < _majors.Count; j++)
			{
				int row = j - (int)Math.Floor((double)j / 3.0) * 3;
				_majors[j].Bounds = new Rectangle(Scale(300) + (int)Math.Floor((double)j / 3.0) * Scale(160), Scale(8) + row * Scale(46), Scale(42), Scale(42));
			}
			_specSelectorBounds = new Rectangle(_selector.Bounds.Right, 0, base.Width - _selector.Bounds.Right, base.Height);
		}

		private void SetSpecialization()
		{
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
					_specBounds[i] = (s, _specBounds[i].bounds, TexturesService.GetAsyncTexture(s.IconAssetId));
					i++;
				}
			}
			_weaponTrait.Texture = TexturesService.GetAsyncTexture(BuildSpecialization?.Specialization?.WeaponTrait?.IconAssetId);
			_specializationBackground.Texture = TexturesService.GetAsyncTexture(BuildSpecialization?.Specialization?.BackgroundAssetId);
			UpdateTraitsForSpecialization();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
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
				spriteBatch.DrawLine(new Vector2(_hexagon.Bounds.Right - Scale(18) + base.AbsoluteBounds.X, _hexagon.Bounds.Center.Y + base.AbsoluteBounds.Y), new Vector2(minor.Left + Scale(3) + base.AbsoluteBounds.X, minor.Center.Y + base.AbsoluteBounds.Y), ContentService.Colors.ColonialWhite * 0.8f, Scale(3));
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
						spriteBatch.DrawLine(new Vector2(minor.Right - Scale(2) + base.AbsoluteBounds.X, minor.Center.Y + base.AbsoluteBounds.Y), new Vector2(major.Left + Scale(2) + base.AbsoluteBounds.X, major.Center.Y + base.AbsoluteBounds.Y), ContentService.Colors.ColonialWhite * 0.8f, Scale(2));
						if (minorNext.HasValue)
						{
							spriteBatch.DrawLine(new Vector2(major.Right - Scale(2) + base.AbsoluteBounds.X, major.Center.Y + base.AbsoluteBounds.Y), new Vector2(minorNext.Value.Left + Scale(2) + base.AbsoluteBounds.X, minorNext.Value.Center.Y + base.AbsoluteBounds.Y), ContentService.Colors.ColonialWhite * 0.8f, Scale(2));
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
					_majors[k].Draw(this, spriteBatch, hoverPos, _majors[k].Selected ? Color.White : (_majors[k].Hovered ? Color.DarkGray : (Color.White * 0.6f)), _majors[k].Selected ? null : new Color?(_majors[k].Hovered ? (Color.Gray * 0.1f) : (Color.Black * 0.5f)), SelectorOpen ? new bool?(false) : null);
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
						if (item.Contains(base.RelativeMousePosition) && spec.spec != null)
						{
							TemplatePresenter.Template.SetSpecialization(SpecializationSlot, spec.spec);
						}
					}
				}
				catch (Exception ex)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Warn($"{ex}");
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
			string txt = null;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _specSelectorBounds, Rectangle.Empty, Color.Black * 0.8f, 0f, Vector2.Zero);
			foreach (var spec in _specBounds)
			{
				Rectangle item = spec.bounds;
				bool hovered = item.Contains(base.RelativeMousePosition);
				TemplatePresenter templatePresenter = TemplatePresenter;
				BuildSpecialization slot;
				bool hasSpec = templatePresenter != null && (templatePresenter.Template?.HasSpecialization(spec.spec, out slot)).GetValueOrDefault();
				if (spec.spec != null)
				{
					AsyncTexture2D specIcon = spec.texture;
					spriteBatch.DrawOnCtrl(this, specIcon, spec.bounds, specIcon?.Bounds ?? Rectangle.Empty, hasSpec ? ContentService.Colors.Chardonnay : (hovered ? Color.White : (Color.White * 0.8f)), 0f, Vector2.Zero);
					if (hovered)
					{
						txt = spec.spec.Name;
					}
					if (hasSpec)
					{
						spriteBatch.DrawOnCtrl(this, specIcon, spec.bounds, specIcon?.Bounds.Add(-4, -4, 8, 8) ?? Rectangle.Empty, Color.Black * 0.7f, 0f, Vector2.Zero);
					}
				}
			}
			return txt;
		}
	}
}
