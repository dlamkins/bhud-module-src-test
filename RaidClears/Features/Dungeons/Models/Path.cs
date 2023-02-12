using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Dungeons.Models
{
	public class Path : BoxModel
	{
		private bool _isFrequented;

		private Color _freqColor = Color.get_White();

		private Color _normalTextColor = Color.get_White();

		public Path(string id, string name, string shortName)
			: base(id, name, shortName)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)


		public Path(Encounters.DungeonPaths path)
			: base(path.GetApiLabel(), path.GetLabel(), path.GetLabelShort())
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)


		public void SetFrequenter(bool freqStatus)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			_isFrequented = freqStatus;
			((Label)base.Box).set_TextColor(freqStatus ? _freqColor : _normalTextColor);
		}

		public void ApplyTextColor()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			((Label)base.Box).set_TextColor(_isFrequented ? _freqColor : _normalTextColor);
			((Control)base.Box).Invalidate();
		}

		public void RegisterFrequenterSettings(SettingEntry<bool> highlightFreq, SettingEntry<string> freqColor, SettingEntry<string> normalTextColor)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			SettingEntry<string> freqColor2 = freqColor;
			SettingEntry<string> normalTextColor2 = normalTextColor;
			_freqColor = (highlightFreq.get_Value() ? freqColor2.get_Value().HexToXnaColor() : normalTextColor2.get_Value().HexToXnaColor());
			ApplyTextColor();
			freqColor2.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object _, ValueChangedEventArgs<string> e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				_freqColor = e.get_NewValue().HexToXnaColor();
				ApplyTextColor();
			});
			normalTextColor2.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object _, ValueChangedEventArgs<string> e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				_normalTextColor = e.get_NewValue().HexToXnaColor();
				ApplyTextColor();
			});
			highlightFreq.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				_freqColor = (e.get_NewValue() ? freqColor2.get_Value().HexToXnaColor() : normalTextColor2.get_Value().HexToXnaColor());
				ApplyTextColor();
			});
		}
	}
}
