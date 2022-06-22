using System;
using System.ComponentModel;
using System.Drawing;
using Blish_HUD;
using Estreya.BlishHUD.EventTable.UI.Views.Controls;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	public class EventPhaseMarker
	{
		private static readonly Logger Logger = Logger.GetLogger<EventPhaseMarker>();

		[JsonIgnore]
		private string _colorCode;

		[JsonIgnore]
		private Color? _color;

		[JsonProperty("time")]
		[TypeOverride(typeof(string))]
		[Description("Specifies the time in minutes after the event started.")]
		public float Time { get; set; }

		[JsonProperty("color")]
		public string ColorCode
		{
			get
			{
				return _colorCode;
			}
			set
			{
				_colorCode = value;
				_color = null;
			}
		}

		[JsonIgnore]
		public Color Color
		{
			get
			{
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				if (!_color.HasValue)
				{
					try
					{
						Color color = (string.IsNullOrWhiteSpace(ColorCode) ? System.Drawing.Color.White : ColorTranslator.FromHtml(ColorCode));
						_color = new Color((int)color.R, (int)color.G, (int)color.B);
					}
					catch (Exception ex)
					{
						Logger.Error(ex, "Failed generating color:");
						_color = Color.get_White();
					}
				}
				return (Color)(((_003F?)_color) ?? Color.get_White());
			}
		}

		[JsonProperty("description")]
		public string Description { get; set; }

		public EventPhaseMarker()
		{
		}

		public EventPhaseMarker(float time, string colorCode)
		{
			Time = time;
			ColorCode = colorCode;
		}

		public EventPhaseMarker(float time, string colorCode, string description)
			: this(time, colorCode)
		{
			Description = description;
		}
	}
}
