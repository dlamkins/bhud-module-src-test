using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Tooltips
{
	public class VendorSourceTooltipView : View, ITooltipView, IView
	{
		private List<Control> _controls = new List<Control>();

		protected bool Initialized;

		private VendorSource Source { get; set; }

		public Container BuildPanel { get; set; }

		public VendorSourceTooltipView(VendorSource source)
		{
			Source = source;
		}

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			if (!Initialized)
			{
				Initialize();
			}
		}

		public virtual void Initialize()
		{
			AsyncTexture2D icon = ServiceContainer.TextureRepository.Textures.MerchantIconColor;
			_controls.Add(new Image(icon)
			{
				Parent = BuildPanel,
				Size = new Point(25, 25),
				Location = new Point(0, 0)
			});
			List<string> vendorNames = Source.VendorItems.Select((VendorSellsItem v) => v.VendorName).Distinct().ToList();
			_controls.Add(new Label
			{
				Parent = BuildPanel,
				Text = string.Format(MysticCrafting.Module.Strings.Recipe.SoldByVendors, vendorNames.Count),
				Font = GameService.Content.DefaultFont16,
				Location = new Point(30, 3),
				StrokeText = true,
				AutoSizeWidth = true
			});
			int yPos = 40;
			foreach (string vendor in vendorNames.Take(10))
			{
				Label vendorLabel = new Label
				{
					Parent = BuildPanel,
					Text = (vendor ?? ""),
					Font = GameService.Content.DefaultFont14,
					Location = new Point(5, yPos),
					TextColor = Color.White,
					StrokeText = true,
					AutoSizeWidth = true
				};
				_controls.Add(vendorLabel);
				yPos += 20;
			}
			yPos += 10;
			if (vendorNames.Count > 10)
			{
				Label moreLabel = new Label
				{
					Parent = BuildPanel,
					Text = string.Format(MysticCrafting.Module.Strings.Recipe.VendorsMore, vendorNames.Count - 10),
					Font = GameService.Content.DefaultFont14,
					Location = new Point(5, yPos),
					TextColor = Color.White,
					StrokeText = true,
					AutoSizeWidth = true
				};
				_controls.Add(moreLabel);
			}
		}

		protected override void Unload()
		{
			_controls?.SafeDispose();
			_controls?.Clear();
			BuildPanel = null;
			base.Unload();
		}
	}
}
