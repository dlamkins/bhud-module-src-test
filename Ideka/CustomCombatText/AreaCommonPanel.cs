using System;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.CustomCombatText
{
	public class AreaCommonPanel : Panel
	{
		public delegate void NameChangedDelegate(AreaView target);

		public delegate void TypeChangedDelegate(AreaView target);

		private const int Spacing = 10;

		private AreaView? _target;

		private readonly StringBox _nameBox;

		private readonly BoolBox _enabledBox;

		private readonly EnumDropdown<AreaType> _typeDropdown;

		public AreaView? Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = null;
				StringBox nameBox = _nameBox;
				StringBox nameBox2 = _nameBox;
				BoolBox enabledBox = _enabledBox;
				BoolBox enabledBox2 = _enabledBox;
				EnumDropdown<AreaType> typeDropdown = _typeDropdown;
				bool flag;
				((Control)_typeDropdown).set_Enabled(flag = value != null);
				bool flag3 = (typeDropdown.ControlEnabled = flag);
				bool flag4;
				((Control)enabledBox2).set_Enabled(flag4 = flag3);
				bool flag6 = (enabledBox.ControlEnabled = flag4);
				bool controlEnabled;
				((Control)nameBox2).set_Enabled(controlEnabled = flag6);
				nameBox.ControlEnabled = controlEnabled;
				_nameBox.Value = value?.Model.Name ?? "";
				_enabledBox.Value = value?.Model.Enabled ?? false;
				_typeDropdown.Value = value?.Model.ModelType.Type ?? AreaType.Container;
				_target = value;
			}
		}

		public event NameChangedDelegate? NameChanged;

		public event TypeChangedDelegate? TypeChanged;

		public AreaCommonPanel()
			: this()
		{
			((Panel)this).set_Title("Common Settings");
			((Panel)this).set_ShowTint(true);
			StringBox stringBox = new StringBox();
			((Control)stringBox).set_Parent((Container)(object)this);
			stringBox.Label = "Name";
			stringBox.AllBasicTooltipText = "For easy identification only.";
			_nameBox = stringBox;
			BoolBox boolBox = new BoolBox();
			((Control)boolBox).set_Parent((Container)(object)this);
			boolBox.Label = "Enabled";
			boolBox.AllBasicTooltipText = "Disabled areas do not show any messages, and neither do areas inside disabled areas.";
			_enabledBox = boolBox;
			EnumDropdown<AreaType> enumDropdown = new EnumDropdown<AreaType>(new Func<AreaType, string>(DataExtensions.Describe), AreaType.Container);
			((Control)enumDropdown).set_Parent((Container)(object)this);
			enumDropdown.Label = "Area Type";
			_typeDropdown = enumDropdown;
			UpdateLayout();
			_nameBox.ValueCommitted += delegate(string value)
			{
				AreaView target3 = Target;
				if (target3 != null)
				{
					target3.Model.Name = value;
					this.NameChanged?.Invoke(target3);
				}
			};
			_enabledBox.ValueCommitted += delegate(bool value)
			{
				AreaView target2 = Target;
				if (target2 != null)
				{
					target2.Model.Enabled = value;
				}
			};
			_typeDropdown.ValueCommitted += delegate(AreaType value)
			{
				AreaView target = Target;
				if (target != null && target.AreaType != value)
				{
					IAreaModelType modelType = value.GetModelType();
					if (modelType == null)
					{
						ScreenNotification.ShowNotification("Type set failed", (NotificationType)2, (Texture2D)null, 4);
						_typeDropdown.Value = target.AreaType;
					}
					else
					{
						target.Model.ModelType = modelType;
						target.ViewType = modelType.CreateView(target.Model);
						this.TypeChanged?.Invoke(target);
					}
				}
			};
			Target = null;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			if (_nameBox != null)
			{
				StringBox nameBox = _nameBox;
				int top;
				((Control)_nameBox).set_Left(top = 10);
				((Control)nameBox).set_Top(top);
				((Control)(object)_nameBox).ArrangeTopDown(10, (Control)_enabledBox, (Control)_typeDropdown);
				((Control)(object)_nameBox).WidthFillRight(10);
				((Control)(object)_enabledBox).WidthFillRight(10);
				((Control)(object)_typeDropdown).WidthFillRight(10);
				ValueControl.AlignLabels(_nameBox, _enabledBox, _typeDropdown);
				((Container)(object)this).MatchHeightToBottom((Control)(object)_typeDropdown, 10);
			}
		}
	}
}
