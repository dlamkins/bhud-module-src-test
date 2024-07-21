using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Discovery.Home
{
	public class HomeView : View<IHomePresenter>
	{
		private readonly IDiscoveryTabPresenter _discoveryTabPresenter;

		public ViewContainer Container { get; set; }

		public HomeViewModel Model { get; set; }

		public Container BuildPanel { get; set; }

		public EventHandler LightClick { get; set; }

		public EventHandler MediumClick { get; set; }

		public EventHandler HeavyClick { get; set; }

		public HomeView(HomeViewModel model, IDiscoveryTabPresenter discoveryPresenter)
		{
			Model = model;
			_discoveryTabPresenter = discoveryPresenter;
			base.WithPresenter((IHomePresenter)new HomePresenter(this, model));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			BuildPanel = buildPanel;
			ViewContainer val = new ViewContainer();
			val.set_FadeView(true);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), ((Control)buildPanel).get_Height()));
			((Control)val).set_Padding(new Thickness(0f));
			((Control)val).set_Location(new Point(0, 0));
			Container = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)Container);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(150, 20));
			val2.set_Font(GameService.Content.get_DefaultFont32());
			val2.set_Text("Welcome to the Mystic Crafting module.");
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)Container);
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Location(new Point(170, 70));
			val3.set_TextColor(Color.get_LightYellow());
			val3.set_Font(GameService.Content.get_DefaultFont16());
			val3.set_Text("Select one of the options below to start crafting your Obsidian Armor.");
			ImageButton imageButton = new ImageButton();
			((Control)imageButton).set_Parent((Container)(object)Container);
			imageButton.Texture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_1.png");
			imageButton.HoverTexture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_1_hover.png");
			((Control)imageButton).set_Size(new Point(260, 500));
			((Control)imageButton).set_Location(new Point(0, 110));
			ImageButton obsidianButton1 = imageButton;
			((Control)obsidianButton1).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				LightClick?.Invoke(s, (EventArgs)(object)e);
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)Container);
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Location(new Point(100, 620));
			val4.set_Font(GameService.Content.get_DefaultFont32());
			val4.set_TextColor(Color.get_LightYellow());
			val4.set_Text("Light");
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				LightClick?.Invoke(s, (EventArgs)(object)e);
			});
			ImageButton imageButton2 = new ImageButton();
			((Control)imageButton2).set_Parent((Container)(object)Container);
			imageButton2.Texture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_2.png");
			imageButton2.HoverTexture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_2_hover.png");
			((Control)imageButton2).set_Size(new Point(260, 500));
			((Control)imageButton2).set_Location(new Point(((Control)obsidianButton1).get_Right(), 110));
			ImageButton obsidianButton2 = imageButton2;
			((Control)obsidianButton2).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				MediumClick?.Invoke(s, (EventArgs)(object)e);
			});
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)Container);
			val5.set_AutoSizeWidth(true);
			val5.set_AutoSizeHeight(true);
			((Control)val5).set_Location(new Point(340, 620));
			val5.set_Font(GameService.Content.get_DefaultFont32());
			val5.set_TextColor(Color.get_LightYellow());
			val5.set_Text("Medium");
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				MediumClick?.Invoke(s, (EventArgs)(object)e);
			});
			ImageButton imageButton3 = new ImageButton();
			((Control)imageButton3).set_Parent((Container)(object)Container);
			imageButton3.Texture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_3.png");
			imageButton3.HoverTexture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_3_hover.png");
			((Control)imageButton3).set_Size(new Point(260, 500));
			((Control)imageButton3).set_Location(new Point(((Control)obsidianButton2).get_Right(), 110));
			((Control)imageButton3).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				HeavyClick?.Invoke(s, (EventArgs)(object)e);
			});
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)Container);
			val6.set_AutoSizeWidth(true);
			val6.set_AutoSizeHeight(true);
			((Control)val6).set_Location(new Point(620, 620));
			val6.set_Font(GameService.Content.get_DefaultFont32());
			val6.set_TextColor(Color.get_LightYellow());
			val6.set_Text("Heavy");
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				HeavyClick?.Invoke(s, (EventArgs)(object)e);
			});
		}
	}
}
