using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class AnchoringPanel : Panel
	{
		private class Advanced : Container
		{
			private AnchoredRect? _target;

			private readonly FloatBox _anchorMinX;

			private readonly FloatBox _anchorMinY;

			private readonly FloatBox _anchorMaxX;

			private readonly FloatBox _anchorMaxY;

			private readonly FloatBox _pivotX;

			private readonly FloatBox _pivotY;

			private readonly IntBox _positionX;

			private readonly IntBox _positionY;

			private readonly IntBox _sizeDeltaX;

			private readonly IntBox _sizeDeltaY;

			public Func<bool>? GetKeepAbsolutePosition { get; set; }

			public AnchoredRect? Target
			{
				get
				{
					return _target;
				}
				set
				{
					//IL_013a: Unknown result type (might be due to invalid IL or missing references)
					//IL_015a: Unknown result type (might be due to invalid IL or missing references)
					//IL_017a: Unknown result type (might be due to invalid IL or missing references)
					//IL_019a: Unknown result type (might be due to invalid IL or missing references)
					//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
					//IL_01da: Unknown result type (might be due to invalid IL or missing references)
					//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
					//IL_021b: Unknown result type (might be due to invalid IL or missing references)
					//IL_023c: Unknown result type (might be due to invalid IL or missing references)
					//IL_025d: Unknown result type (might be due to invalid IL or missing references)
					_target = null;
					FloatBox anchorMinX = _anchorMinX;
					bool controlEnabled;
					((Control)_anchorMinX).set_Enabled(controlEnabled = value != null);
					anchorMinX.ControlEnabled = controlEnabled;
					FloatBox anchorMinY = _anchorMinY;
					((Control)_anchorMinY).set_Enabled(controlEnabled = value != null);
					anchorMinY.ControlEnabled = controlEnabled;
					FloatBox anchorMaxX = _anchorMaxX;
					((Control)_anchorMaxX).set_Enabled(controlEnabled = value != null);
					anchorMaxX.ControlEnabled = controlEnabled;
					FloatBox anchorMaxY = _anchorMaxY;
					((Control)_anchorMaxY).set_Enabled(controlEnabled = value != null);
					anchorMaxY.ControlEnabled = controlEnabled;
					FloatBox pivotX = _pivotX;
					((Control)_pivotX).set_Enabled(controlEnabled = value != null);
					pivotX.ControlEnabled = controlEnabled;
					FloatBox pivotY = _pivotY;
					((Control)_pivotY).set_Enabled(controlEnabled = value != null);
					pivotY.ControlEnabled = controlEnabled;
					IntBox positionX = _positionX;
					((Control)_positionX).set_Enabled(controlEnabled = value != null);
					positionX.ControlEnabled = controlEnabled;
					IntBox positionY = _positionY;
					((Control)_positionY).set_Enabled(controlEnabled = value != null);
					positionY.ControlEnabled = controlEnabled;
					IntBox sizeDeltaX = _sizeDeltaX;
					((Control)_sizeDeltaX).set_Enabled(controlEnabled = value != null);
					sizeDeltaX.ControlEnabled = controlEnabled;
					IntBox sizeDeltaY = _sizeDeltaY;
					((Control)_sizeDeltaY).set_Enabled(controlEnabled = value != null);
					sizeDeltaY.ControlEnabled = controlEnabled;
					_anchorMinX.Value = value?.AnchorMin.X ?? 0f;
					_anchorMinY.Value = value?.AnchorMin.Y ?? 0f;
					_anchorMaxX.Value = value?.AnchorMax.X ?? 1f;
					_anchorMaxY.Value = value?.AnchorMax.Y ?? 1f;
					_pivotX.Value = value?.Pivot.X ?? 0.5f;
					_pivotY.Value = value?.Pivot.Y ?? 0.5f;
					_positionX.Value = (int)(value?.Position.X ?? 0f);
					_positionY.Value = (int)(value?.Position.Y ?? 0f);
					_sizeDeltaX.Value = (int)(value?.SizeDelta.X ?? 0f);
					_sizeDeltaY.Value = (int)(value?.SizeDelta.Y ?? 0f);
					_target = value;
				}
			}

			public Advanced()
				: this()
			{
				_anchorMinX = percentBox("Anchor Min X", 0.001f);
				_anchorMinY = percentBox("Anchor Min Y", 0.001f);
				_anchorMaxX = percentBox("Anchor Max X", 0.001f);
				_anchorMaxY = percentBox("Anchor Max Y", 0.001f);
				_pivotX = percentBox("Pivot X", 0.001f);
				_pivotY = percentBox("Pivot Y", 0.001f);
				_positionX = intBox("Position X");
				_positionY = intBox("Position Y", 1f);
				_sizeDeltaX = intBox("Size Delta X");
				_sizeDeltaY = intBox("Size Delta Y");
				UpdateLayout();
				_anchorMinX.ValueCommitted += delegate(float value)
				{
					withTarget(delegate(AnchoredRect x)
					{
						x.AnchorMinX = value;
					}, keep: true);
					void withTarget(Action<AnchoredRect> act, bool keep = false)
					{
						Action<AnchoredRect> act2 = act;
						if (Target != null)
						{
							if (keep && (GetKeepAbsolutePosition?.Invoke() ?? false))
							{
								Target!.KeepingAbsolute(delegate
								{
									act2(Target);
								});
								Target = Target;
							}
							else
							{
								act2(Target);
							}
						}
					}
				};
				_anchorMinY.ValueCommitted += delegate(float value)
				{
					Advanced._003C_002Ector_003Eg__withTarget_007C18_2((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.AnchorMinY = value;
					}, keep: true);
				};
				_anchorMaxX.ValueCommitted += delegate(float value)
				{
					Advanced._003C_002Ector_003Eg__withTarget_007C18_2((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.AnchorMaxX = value;
					}, keep: true);
				};
				_anchorMaxY.ValueCommitted += delegate(float value)
				{
					Advanced._003C_002Ector_003Eg__withTarget_007C18_2((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.AnchorMaxY = value;
					}, keep: true);
				};
				_pivotX.ValueCommitted += delegate(float value)
				{
					Advanced._003C_002Ector_003Eg__withTarget_007C18_2((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.PivotX = value;
					}, keep: true);
				};
				_pivotY.ValueCommitted += delegate(float value)
				{
					Advanced._003C_002Ector_003Eg__withTarget_007C18_2((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.PivotY = value;
					}, keep: true);
				};
				_positionX.ValueCommitted += delegate(int value)
				{
					Advanced._003C_002Ector_003Eg__withTarget_007C18_2((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.PositionX = value;
					}, keep: false);
				};
				_positionY.ValueCommitted += delegate(int value)
				{
					Advanced._003C_002Ector_003Eg__withTarget_007C18_2((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.PositionY = value;
					}, keep: false);
				};
				_sizeDeltaX.ValueCommitted += delegate(int value)
				{
					Advanced._003C_002Ector_003Eg__withTarget_007C18_2((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.SizeDeltaX = value;
					}, keep: false);
				};
				_sizeDeltaY.ValueCommitted += delegate(int value)
				{
					Advanced._003C_002Ector_003Eg__withTarget_007C18_2((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.SizeDeltaY = value;
					}, keep: false);
				};
				Target = null;
				IntBox intBox(string label, float yScale = -1f)
				{
					IntBox intBox = new IntBox();
					((Control)intBox).set_Parent((Container)(object)this);
					intBox.Label = label;
					intBox.Scale = 1f;
					intBox.YScale = yScale;
					intBox.DraggingCommits = true;
					intBox.LabelBasicTooltipText = "Click and drag.";
					return intBox;
				}
				FloatBox percentBox(string label, float scale)
				{
					FloatBox floatBox = new FloatBox();
					((Control)floatBox).set_Parent((Container)(object)this);
					floatBox.Label = label;
					floatBox.MinValue = 0f;
					floatBox.MaxValue = 1f;
					floatBox.Scale = scale;
					floatBox.YScale = 1f;
					floatBox.DraggingCommits = true;
					floatBox.LabelBasicTooltipText = "Click and drag.";
					return floatBox;
				}
			}

			protected override void OnResized(ResizedEventArgs e)
			{
				((Container)this).OnResized(e);
				UpdateLayout();
			}

			private void UpdateLayout()
			{
				if (_anchorMinX != null)
				{
					FloatBox anchorMinX = _anchorMinX;
					int top;
					((Control)_anchorMinX).set_Left(top = 10);
					((Control)anchorMinX).set_Top(top);
					((Control)(object)_anchorMinX).ArrangeTopDown(10, (Control)_anchorMaxX, (Control)_pivotX, (Control)_positionX, (Control)_sizeDeltaX);
					arrangeAndFillOut((Control[])(object)new Control[2]
					{
						(Control)_anchorMinX,
						(Control)_anchorMinY
					});
					arrangeAndFillOut((Control[])(object)new Control[2]
					{
						(Control)_anchorMaxX,
						(Control)_anchorMaxY
					});
					arrangeAndFillOut((Control[])(object)new Control[2]
					{
						(Control)_pivotX,
						(Control)_pivotY
					});
					arrangeAndFillOut((Control[])(object)new Control[2]
					{
						(Control)_positionX,
						(Control)_positionY
					});
					arrangeAndFillOut((Control[])(object)new Control[2]
					{
						(Control)_sizeDeltaX,
						(Control)_sizeDeltaY
					});
					ValueControl.AlignLabels(_anchorMinX, _anchorMaxX, _pivotX, _positionX, _sizeDeltaX);
					ValueControl.AlignLabels(_anchorMinY, _anchorMaxY, _pivotY, _positionY, _sizeDeltaY);
					((Container)(object)this).MatchHeightToBottom((Control)(object)_sizeDeltaX, 10);
				}
				void arrangeAndFillOut(Control[] others)
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					if (others.Any())
					{
						int width = (((Container)this).get_ContentRegion().Width - 10 * (others.Length + 1)) / others.Length;
						for (int i = 0; i < others.Length; i++)
						{
							others[i].set_Width(width);
						}
						others[0].set_Left(10);
						others[0].ArrangeLeftRight(10, others.Skip(1).ToArray());
					}
				}
			}
		}

		private class Basic : Container
		{
			private enum Preset
			{
				Custom,
				Full,
				Middle,
				TopLeft,
				TopRight,
				BottomLeft,
				BottomRight,
				MiddleLeft,
				MiddleRight,
				CenterTop,
				CenterBottom
			}

			private readonly struct PresetValue
			{
				public Vector2 AnchorMin { get; init; }

				public Vector2 AnchorMax { get; init; }

				public Vector2 Pivot { get; init; }

				public bool Fits(AnchoredRect rect)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0027: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					if (rect.AnchorMin == AnchorMin && rect.AnchorMax == AnchorMax)
					{
						return rect.Pivot == Pivot;
					}
					return false;
				}

				public void Apply(AnchoredRect rect)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					rect.AnchorMin = AnchorMin;
					rect.AnchorMax = AnchorMax;
					rect.Pivot = Pivot;
				}
			}

			private AnchoredRect? _target;

			private static readonly Dictionary<Preset, PresetValue> PresetValues = new Dictionary<Preset, PresetValue>
			{
				[Preset.Full] = new PresetValue
				{
					AnchorMin = new Vector2(0f, 0f),
					AnchorMax = new Vector2(1f, 1f),
					Pivot = new Vector2(0.5f, 0.5f)
				},
				[Preset.Middle] = new PresetValue
				{
					AnchorMin = new Vector2(0.5f, 0.5f),
					AnchorMax = new Vector2(0.5f, 0.5f),
					Pivot = new Vector2(0.5f, 0.5f)
				},
				[Preset.TopLeft] = new PresetValue
				{
					AnchorMin = new Vector2(0f, 0f),
					AnchorMax = new Vector2(0f, 0f),
					Pivot = new Vector2(0f, 0f)
				},
				[Preset.TopRight] = new PresetValue
				{
					AnchorMin = new Vector2(1f, 0f),
					AnchorMax = new Vector2(1f, 0f),
					Pivot = new Vector2(1f, 0f)
				},
				[Preset.BottomLeft] = new PresetValue
				{
					AnchorMin = new Vector2(0f, 1f),
					AnchorMax = new Vector2(0f, 1f),
					Pivot = new Vector2(0f, 1f)
				},
				[Preset.BottomRight] = new PresetValue
				{
					AnchorMin = new Vector2(1f, 1f),
					AnchorMax = new Vector2(1f, 1f),
					Pivot = new Vector2(1f, 1f)
				},
				[Preset.MiddleLeft] = new PresetValue
				{
					AnchorMin = new Vector2(0f, 0.5f),
					AnchorMax = new Vector2(0f, 0.5f),
					Pivot = new Vector2(0f, 0.5f)
				},
				[Preset.MiddleRight] = new PresetValue
				{
					AnchorMin = new Vector2(1f, 0.5f),
					AnchorMax = new Vector2(1f, 0.5f),
					Pivot = new Vector2(1f, 0.5f)
				},
				[Preset.CenterTop] = new PresetValue
				{
					AnchorMin = new Vector2(0f, 0.5f),
					AnchorMax = new Vector2(0f, 0.5f),
					Pivot = new Vector2(0f, 0.5f)
				},
				[Preset.CenterBottom] = new PresetValue
				{
					AnchorMin = new Vector2(1f, 0.5f),
					AnchorMax = new Vector2(1f, 0.5f),
					Pivot = new Vector2(1f, 0.5f)
				}
			};

			private readonly EnumDropdown<Preset> _presetDropdown;

			private readonly IntBox _positionX;

			private readonly IntBox _positionY;

			private readonly IntBox _sizeDeltaX;

			private readonly IntBox _sizeDeltaY;

			public Func<bool>? GetKeepAbsolutePosition { get; set; }

			public AnchoredRect? Target
			{
				get
				{
					return _target;
				}
				set
				{
					//IL_011b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0142: Unknown result type (might be due to invalid IL or missing references)
					//IL_0169: Unknown result type (might be due to invalid IL or missing references)
					//IL_0190: Unknown result type (might be due to invalid IL or missing references)
					AnchoredRect value2 = value;
					_target = null;
					EnumDropdown<Preset> presetDropdown = _presetDropdown;
					bool controlEnabled;
					((Control)_presetDropdown).set_Enabled(controlEnabled = value2 != null);
					presetDropdown.ControlEnabled = controlEnabled;
					IntBox positionX = _positionX;
					((Control)_positionX).set_Enabled(controlEnabled = value2 != null);
					positionX.ControlEnabled = controlEnabled;
					IntBox positionY = _positionY;
					((Control)_positionY).set_Enabled(controlEnabled = value2 != null);
					positionY.ControlEnabled = controlEnabled;
					IntBox sizeDeltaX = _sizeDeltaX;
					((Control)_sizeDeltaX).set_Enabled(controlEnabled = value2 != null);
					sizeDeltaX.ControlEnabled = controlEnabled;
					IntBox sizeDeltaY = _sizeDeltaY;
					((Control)_sizeDeltaY).set_Enabled(controlEnabled = value2 != null);
					sizeDeltaY.ControlEnabled = controlEnabled;
					EnumDropdown<Preset> presetDropdown2 = _presetDropdown;
					int value3;
					if (value2 != null)
					{
						IEnumerable<KeyValuePair<Preset, PresetValue>> x2 = PresetValues.Where((KeyValuePair<Preset, PresetValue> x) => x.Value.Fits(value2));
						value3 = (int)(x2.Any() ? x2.First().Key : Preset.Custom);
					}
					else
					{
						value3 = 1;
					}
					presetDropdown2.Value = (Preset)value3;
					_positionX.Value = (int)(value2?.Position.X ?? 0f);
					_positionY.Value = (int)(value2?.Position.Y ?? 0f);
					_sizeDeltaX.Value = (int)(value2?.SizeDelta.X ?? 0f);
					_sizeDeltaY.Value = (int)(value2?.SizeDelta.Y ?? 0f);
					_target = value2;
				}
			}

			private static string? DescribePreset(Preset type)
			{
				return type switch
				{
					Preset.Custom => "Custom", 
					Preset.Full => "Full", 
					Preset.Middle => "Middle", 
					Preset.TopLeft => "Top left", 
					Preset.TopRight => "Top right", 
					Preset.BottomLeft => "Bottom left", 
					Preset.BottomRight => "Bottom right", 
					Preset.MiddleLeft => "Middle left", 
					Preset.MiddleRight => "Middle right", 
					Preset.CenterTop => "Center top", 
					Preset.CenterBottom => "Center bottom", 
					_ => null, 
				};
			}

			public Basic()
				: this()
			{
				EnumDropdown<Preset> enumDropdown = new EnumDropdown<Preset>(new Func<Preset, string>(DescribePreset), Preset.Custom);
				((Control)enumDropdown).set_Parent((Container)(object)this);
				enumDropdown.Label = "Anchoring Preset";
				_presetDropdown = enumDropdown;
				_positionX = intBox("Position X");
				_positionY = intBox("Position Y", 1f);
				_sizeDeltaX = intBox("Size Delta X");
				_sizeDeltaY = intBox("Size Delta Y");
				UpdateLayout();
				_presetDropdown.ValueCommitted += delegate(Preset value)
				{
					withTarget(delegate(AnchoredRect x)
					{
						if (PresetValues.TryGetValue(value, out var value2))
						{
							value2.Apply(x);
						}
					}, keep: true);
					void withTarget(Action<AnchoredRect> act, bool keep = false)
					{
						Action<AnchoredRect> act2 = act;
						if (Target != null)
						{
							if (keep && (GetKeepAbsolutePosition?.Invoke() ?? false))
							{
								Target!.KeepingAbsolute(delegate
								{
									act2(Target);
								});
								Target = Target;
							}
							else
							{
								act2(Target);
							}
						}
					}
				};
				_positionX.ValueCommitted += delegate(int value)
				{
					Basic._003C_002Ector_003Eg__withTarget_007C17_1((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.PositionX = value;
					}, keep: false);
				};
				_positionY.ValueCommitted += delegate(int value)
				{
					Basic._003C_002Ector_003Eg__withTarget_007C17_1((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.PositionY = value;
					}, keep: false);
				};
				_sizeDeltaX.ValueCommitted += delegate(int value)
				{
					Basic._003C_002Ector_003Eg__withTarget_007C17_1((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.SizeDeltaX = value;
					}, keep: false);
				};
				_sizeDeltaY.ValueCommitted += delegate(int value)
				{
					Basic._003C_002Ector_003Eg__withTarget_007C17_1((Action<AnchoredRect>)delegate(AnchoredRect x)
					{
						x.SizeDeltaY = value;
					}, keep: false);
				};
				Target = null;
				IntBox intBox(string label, float yScale = -1f)
				{
					IntBox intBox = new IntBox();
					((Control)intBox).set_Parent((Container)(object)this);
					intBox.Label = label;
					intBox.Scale = 1f;
					intBox.YScale = yScale;
					intBox.DraggingCommits = true;
					intBox.LabelBasicTooltipText = "Click and drag.";
					return intBox;
				}
			}

			protected override void OnResized(ResizedEventArgs e)
			{
				((Container)this).OnResized(e);
				UpdateLayout();
			}

			private void UpdateLayout()
			{
				if (_presetDropdown != null)
				{
					EnumDropdown<Preset> presetDropdown = _presetDropdown;
					int top;
					((Control)_presetDropdown).set_Left(top = 10);
					((Control)presetDropdown).set_Top(top);
					((Control)(object)_presetDropdown).ArrangeTopDown(10, (Control)_positionX, (Control)_sizeDeltaX);
					((Control)(object)_presetDropdown).WidthFillRight(10);
					arrangeAndFillOut((Control[])(object)new Control[2]
					{
						(Control)_positionX,
						(Control)_positionY
					});
					arrangeAndFillOut((Control[])(object)new Control[2]
					{
						(Control)_sizeDeltaX,
						(Control)_sizeDeltaY
					});
					ValueControl.AlignLabels(_positionX, _sizeDeltaX);
					ValueControl.AlignLabels(_positionY, _sizeDeltaY);
					((Container)(object)this).MatchHeightToBottom((Control)(object)_sizeDeltaX, 10);
				}
				void arrangeAndFillOut(Control[] others)
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					if (others.Any())
					{
						int width = (((Container)this).get_ContentRegion().Width - 10 * (others.Length + 1)) / others.Length;
						for (int i = 0; i < others.Length; i++)
						{
							others[i].set_Width(width);
						}
						others[0].set_Left(10);
						others[0].ArrangeLeftRight(10, others.Skip(1).ToArray());
					}
				}
			}
		}

		private const int Spacing = 10;

		private AnchoredRect? _target;

		private readonly Advanced _advanced;

		private readonly Basic _basic;

		private readonly BoolBox _keepAbsolutePositionBox;

		private readonly BoolBox _advancedBox;

		public bool KeepAbsolutePosition => _keepAbsolutePositionBox.Value;

		public AnchoredRect? Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = null;
				AnchoredRect anchoredRect3 = (_advanced.Target = (_basic.Target = value));
				_target = value;
			}
		}

		public AnchoringPanel()
			: this()
		{
			((Panel)this).set_Title("Anchoring");
			((Panel)this).set_ShowTint(true);
			Advanced advanced = new Advanced();
			((Control)advanced).set_Parent((Container)(object)this);
			advanced.GetKeepAbsolutePosition = () => KeepAbsolutePosition;
			_advanced = advanced;
			Basic basic = new Basic();
			((Control)basic).set_Parent((Container)(object)this);
			basic.GetKeepAbsolutePosition = () => KeepAbsolutePosition;
			_basic = basic;
			BoolBox boolBox = new BoolBox();
			((Control)boolBox).set_Parent((Container)(object)this);
			boolBox.Label = "Keep Absolute Position";
			boolBox.Value = true;
			boolBox.AllBasicTooltipText = "When changing an area's anchors, pivot, or container, automatically adjust the position and size delta so its absolute position and size remain the same on the screen.";
			_keepAbsolutePositionBox = boolBox;
			BoolBox boolBox2 = new BoolBox();
			((Control)boolBox2).set_Parent((Container)(object)this);
			boolBox2.Label = "Advanced";
			_advancedBox = boolBox2;
			UpdateLayout();
			_advancedBox.ValueCommitted += delegate(bool value)
			{
				AnchoredRect anchoredRect2 = (_advanced.Target = (_basic.Target = Target));
				((Control)_advanced).set_Visible(value);
				((Control)_basic).set_Visible(!value);
			};
			_advancedBox.CommitValue(value: false);
			Target = null;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			if (_advanced != null)
			{
				((Control)(object)_advanced).WidthFillRight();
				((Control)(object)_basic).WidthFillRight();
				Container big;
				Container small;
				if (((Control)_advanced).get_Height() >= ((Control)_basic).get_Height())
				{
					Basic basic = _basic;
					Container advanced = (Container)(object)_advanced;
					big = advanced;
					small = (Container)(object)basic;
				}
				else
				{
					Advanced advanced2 = _advanced;
					Container advanced = (Container)(object)_basic;
					big = advanced;
					small = (Container)(object)advanced2;
				}
				((Control)(object)small).MiddleWith((Control)(object)big);
				((Control)(object)big).ArrangeTopDown(0, (Control)_keepAbsolutePositionBox);
				((Control)_keepAbsolutePositionBox).set_Left(10);
				((Control)_keepAbsolutePositionBox).set_Width(((Container)this).get_ContentRegion().Width / 2 - 10 - 5);
				((Control)(object)_keepAbsolutePositionBox).ArrangeLeftRight(10, (Control)_advancedBox);
				((Control)(object)_advancedBox).WidthFillRight(10);
				((Container)(object)this).MatchHeightToBottom((Control)(object)_keepAbsolutePositionBox, 10);
			}
		}
	}
}
