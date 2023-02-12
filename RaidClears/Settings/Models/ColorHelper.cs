using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace RaidClears.Settings.Models
{
	public class ColorHelper : Color
	{
		public Color XnaColor
		{
			get
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				List<int> color = ((Color)this).get_Cloth().get_Rgb().ToList();
				return new Color(color[0], color[1], color[2]);
			}
		}

		public ColorHelper()
			: this()
		{
			((Color)this).set_BaseRgb((IReadOnlyList<int>)new List<int>(3));
			SetRGB(255, 255, 255);
		}

		public ColorHelper(string colorCode)
			: this()
		{
			((Color)this).set_BaseRgb((IReadOnlyList<int>)new List<int>(3));
			SetRGB(colorCode);
		}

		public void SetName(string name)
		{
			((Color)this).set_Name(name);
		}

		public void SetRGB(string colorCode)
		{
			colorCode = Regex.Replace(colorCode, "[^a-fA-F0-9]", string.Empty);
			if (colorCode.Length == 6)
			{
				byte r = Convert.ToByte(colorCode.Substring(0, 2), 16);
				byte g = Convert.ToByte(colorCode.Substring(2, 2), 16);
				byte b = Convert.ToByte(colorCode.Substring(4, 2), 16);
				SetRGB(r, g, b);
			}
			else
			{
				SetRGB(255, 255, 255);
			}
		}

		public void SetRGB(int r = 0, int g = 0, int b = 0)
		{
			((Color)this).get_Cloth().set_Rgb((IReadOnlyList<int>)new List<int> { r, g, b });
			((Color)this).set_Name($"RGB: {r} {g} {b}");
		}
	}
}
