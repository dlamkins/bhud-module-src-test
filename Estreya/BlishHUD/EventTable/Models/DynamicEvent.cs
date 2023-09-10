using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	public class DynamicEvent
	{
		public class DynamicEventLocation
		{
			[JsonProperty("type")]
			public string Type { get; set; }

			[JsonProperty("center")]
			public float[] Center { get; set; }

			[JsonProperty("radius")]
			public float Radius { get; set; }

			[JsonProperty("height")]
			public float Height { get; set; }

			[JsonProperty("rotation")]
			public float Rotation { get; set; }

			[JsonProperty("z_range")]
			public float[] ZRange { get; set; }

			[JsonProperty("points")]
			public float[][] Points { get; set; }
		}

		public class DynamicEventIcon
		{
			[JsonProperty("file_id")]
			public int FileID { get; set; }

			[JsonProperty("signature")]
			public string Signature { get; set; }
		}

		[JsonProperty("id")]
		public string ID { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("level")]
		public int Level { get; set; }

		[JsonProperty("map_id")]
		public int MapId { get; set; }

		[JsonProperty("flags")]
		public string[] Flags { get; set; }

		[JsonProperty("location")]
		public DynamicEventLocation Location { get; set; }

		[JsonProperty("icon")]
		public DynamicEventIcon Icon { get; set; }

		[JsonProperty("custom")]
		public bool IsCustom { get; set; }

		[JsonProperty("color")]
		public string ColorCode { get; set; }

		public Color GetColorAsXnaColor()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			Color defaultColor = Color.get_White();
			if (string.IsNullOrWhiteSpace(ColorCode))
			{
				return defaultColor;
			}
			try
			{
				Color parsedColor = ColorTranslator.FromHtml(ColorCode);
				return new Color((int)parsedColor.R, (int)parsedColor.G, (int)parsedColor.B);
			}
			catch (Exception)
			{
				return defaultColor;
			}
		}
	}
}
