using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.RecipeTree
{
	public class RecipeTableHeaderItemView : View<IRecipeTableHeaderItemPresenter>
	{
		private Image _icon;

		private LoadingSpinner _loadingSpinner;

		private Label _label;

		private bool _loading;

		private bool _failed;

		private string _labelText;

		public bool Loading
		{
			get
			{
				return _loading;
			}
			set
			{
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				_loading = value;
				if (_loadingSpinner != null)
				{
					((Control)_loadingSpinner).set_Visible(value);
				}
				if (_icon != null)
				{
					((Control)_icon).set_Visible(!value);
				}
				if (_labelText != null)
				{
					_label.set_TextColor(value ? Color.get_Orange() : (Color.get_White() * 0.7f));
				}
			}
		}

		public string LabelTooltip
		{
			get
			{
				Label label = _label;
				return ((label != null) ? ((Control)label).get_BasicTooltipText() : null) ?? string.Empty;
			}
			set
			{
				if (_label != null)
				{
					((Control)_label).set_BasicTooltipText(value);
				}
			}
		}

		public bool Failed
		{
			get
			{
				return _failed;
			}
			set
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				_failed = value;
				if (_failed)
				{
					if (_label != null)
					{
						_label.set_TextColor(Color.get_Red());
					}
					if (_icon != null)
					{
						_icon.set_Texture(ServiceContainer.TextureRepository.Textures.CrossSmall);
					}
				}
				else
				{
					if (_label != null)
					{
						_label.set_TextColor(Color.get_White() * 0.7f);
					}
					if (_icon != null)
					{
						_icon.set_Texture(ServiceContainer.TextureRepository.Textures.CheckmarkSmall);
					}
				}
			}
		}

		public RecipeTableHeaderItemView(IApiService apiService, string labelText)
		{
			_labelText = labelText;
			base.WithPresenter((IRecipeTableHeaderItemPresenter)new RecipeTableHeaderItemPresenter(this, apiService));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(Color.get_White() * 0.7f);
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Text(_labelText);
			((Control)val).set_BasicTooltipText(base.get_Presenter().GetTooltipText());
			((Control)val).set_Location(new Point(30, 8));
			_label = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(25, 25));
			((Control)val2).set_Location(new Point(0, 5));
			((Control)val2).set_Visible(false);
			_loadingSpinner = val2;
			Image val3 = new Image(ServiceContainer.TextureRepository.Textures.CheckmarkSmall);
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Size(new Point(27, 27));
			((Control)val3).set_Location(new Point(3, 4));
			_icon = val3;
		}
	}
}
