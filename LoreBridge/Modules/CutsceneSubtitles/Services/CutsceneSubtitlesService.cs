using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameServices.ArcDps.V2;
using Blish_HUD.GameServices.ArcDps.V2.Models.UnofficialExtras;
using FontStashSharp;
using LoreBridge.Controls;
using LoreBridge.Modules.AreaTranslation.Controls;
using LoreBridge.Services.Ocr;
using LoreBridge.Translation;
using LoreBridge.Translation.Language;
using LoreBridge.Translation.Translators;
using LoreBridge.Utils;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.CutsceneSubtitles.Services
{
	public class CutsceneSubtitlesService : IDisposable
	{
		private readonly ScreenDetectionRegion _cutsceneRegion;

		private readonly WindowsOcr _engine;

		private readonly SortedList<ulong, string> _messages = new SortedList<ulong, string>();

		private readonly LabelCustom _subtitlesLabel;

		private readonly Yandex _translator;

		private bool _enabled;

		private bool _isCutsceneWithMessages;

		private double _lastTick;

		private IArcDpsMessageListener<NpcMessageInfo> _npcMessageListener;

		private string _prevDetectedText;

		private double _timeAfterLastDetect;

		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if (value)
				{
					Start();
				}
				else
				{
					End();
				}
				_enabled = value;
			}
		}

		public CutsceneSubtitlesService(WindowsOcr engine, SpriteFontBase font)
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			_translator = new Yandex(new TranslatorConfig
			{
				TargetLang = LanguagesInfo.GetByLanguage(19)
			});
			_engine = engine;
			ScreenDetectionRegion screenDetectionRegion = new ScreenDetectionRegion();
			((Control)screenDetectionRegion).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)screenDetectionRegion).set_Visible(false);
			_cutsceneRegion = screenDetectionRegion;
			LabelCustom labelCustom = new LabelCustom();
			((Control)labelCustom).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			labelCustom.AutoSizeHeight = true;
			labelCustom.Font = font;
			labelCustom.TextColor = Color.get_White();
			labelCustom.WrapText = true;
			((Control)labelCustom).set_BackgroundColor(Color.get_Black());
			((Control)labelCustom).set_Visible(false);
			labelCustom.HorizontalAlignment = (HorizontalAlignment)1;
			labelCustom.VerticalAlignment = (VerticalAlignment)0;
			_subtitlesLabel = labelCustom;
		}

		public void Dispose()
		{
			((Control)_cutsceneRegion).Dispose();
			((Control)_subtitlesLabel).Dispose();
			((IDisposable)_npcMessageListener)?.Dispose();
		}

		public void Run(GameTime gameTime)
		{
			if (_enabled && !(gameTime.get_TotalGameTime().TotalMilliseconds - _lastTick < 500.0))
			{
				_lastTick = gameTime.get_TotalGameTime().TotalMilliseconds;
				if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && !_isCutsceneWithMessages)
				{
					DetectText(gameTime);
				}
			}
		}

		private async Task OnNpcChatMessage(NpcMessageInfo chatMessage, CancellationToken cancellationToken)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			_isCutsceneWithMessages = true;
			string translation = await _translator.TranslateAsync(((NpcMessageInfo)(ref chatMessage)).get_Message());
			if (!string.IsNullOrWhiteSpace(translation))
			{
				_messages.Add(((NpcMessageInfo)(ref chatMessage)).get_TimeStamp(), string.Join("", ((NpcMessageInfo)(ref chatMessage)).get_CharacterName() + ": ", translation));
				_subtitlesLabel.Text = _messages.Last().Value;
			}
		}

		private void CreateNpcMessageListener()
		{
			try
			{
				if (_npcMessageListener == null)
				{
					_npcMessageListener = (IArcDpsMessageListener<NpcMessageInfo>)(object)new ArcDpsMessageListener<NpcMessageInfo>((MessageType)6, (Func<NpcMessageInfo, CancellationToken, Task>)OnNpcChatMessage);
				}
				GameService.ArcDpsV2.RegisterMessageType<NpcMessageInfo>(_npcMessageListener);
			}
			catch (Exception)
			{
			}
		}

		private void RecalculateSubtitlesBounds()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Rectangle bounds = ((Control)GameService.Graphics.get_SpriteScreen()).get_LocalBounds();
			((Control)_subtitlesLabel).set_Width(bounds.Width / 2);
			((Control)_subtitlesLabel).set_Left(bounds.Width / 2 - bounds.Width / 4);
			((Control)_subtitlesLabel).set_Top(((Rectangle)(ref bounds)).get_Top() + 2);
		}

		private void RecalculateBottomBarBounds()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Rectangle bounds = ((Control)GameService.Graphics.get_SpriteScreen()).get_LocalBounds();
			((Control)_cutsceneRegion).set_Height(CalculateBottomBarHeight());
			((Control)_cutsceneRegion).set_Width(bounds.Width);
			((Control)_cutsceneRegion).set_Left(0);
			((Control)_cutsceneRegion).set_Bottom(((Rectangle)(ref bounds)).get_Bottom());
		}

		private void Start()
		{
			CreateNpcMessageListener();
			RecalculateSubtitlesBounds();
			RecalculateBottomBarBounds();
			((Control)_cutsceneRegion).set_Visible(true);
			((Control)_subtitlesLabel).set_Visible(true);
		}

		private void End()
		{
			((IDisposable)_npcMessageListener)?.Dispose();
			((Control)_cutsceneRegion).set_Visible(false);
			((Control)_subtitlesLabel).set_Visible(false);
			_subtitlesLabel.Text = null;
			_messages.Clear();
			_isCutsceneWithMessages = false;
		}

		private int CalculateBottomBarHeight()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			Point resolution = GameService.Graphics.get_Resolution();
			Bitmap left = Screen.GetScreen(new Point(0, 0), new Size(2, resolution.Y));
			Bitmap right = Screen.GetScreen(new Point(resolution.X - 2, 0), new Size(2, resolution.Y));
			Bitmap center = Screen.GetScreen(new Point(resolution.X / 2 - 1, 0), new Size(2, resolution.Y));
			int h1 = 0;
			bool found = false;
			int y3 = left.Height - 1;
			while (y3 >= 0 && !found)
			{
				for (int x3 = 0; x3 < left.Width; x3++)
				{
					Color pixelColor3 = left.GetPixel(x3, y3);
					if (pixelColor3.R > 1 && pixelColor3.G > 1 && pixelColor3.B > 1)
					{
						h1 = resolution.Y - y3;
						found = true;
						break;
					}
				}
				y3--;
			}
			int h2 = 0;
			found = false;
			int y2 = right.Height - 1;
			while (y2 >= 0 && !found)
			{
				for (int x2 = 0; x2 < right.Width; x2++)
				{
					Color pixelColor2 = right.GetPixel(x2, y2);
					if (pixelColor2.R > 1 && pixelColor2.G > 1 && pixelColor2.B > 1)
					{
						h2 = resolution.Y - y2;
						found = true;
						break;
					}
				}
				y2--;
			}
			int h3 = 0;
			found = false;
			int y = center.Height - 1;
			while (y >= 0 && !found)
			{
				for (int x = 0; x < center.Width; x++)
				{
					Color pixelColor = center.GetPixel(x, y);
					if (pixelColor.R > 1 && pixelColor.G > 1 && pixelColor.B > 1)
					{
						h3 = resolution.Y - y;
						found = true;
						break;
					}
				}
				y--;
			}
			return Math.Max(Math.Max(h1, h2), h3);
		}

		private async Task DetectText(GameTime gameTime)
		{
			float factor = GameService.Graphics.get_UIScaleMultiplier();
			Bitmap bitmap = Screen.GetScreen(new Point((int)((float)((Control)_cutsceneRegion).get_Location().X * factor), (int)((float)((Control)_cutsceneRegion).get_Location().Y * factor)), new Size((int)((float)((Control)_cutsceneRegion).get_Size().X * factor), (int)((float)((Control)_cutsceneRegion).get_Size().Y * factor)));
			string[] text = _engine.GetTextLines(bitmap);
			if (text.Last().ToLower() == "skip to end")
			{
				text = text.Take(text.Length - 1).ToArray();
			}
			string text2 = string.Join("\n", text);
			if (string.IsNullOrWhiteSpace(text2))
			{
				_subtitlesLabel.Text = null;
				_timeAfterLastDetect = gameTime.get_TotalGameTime().TotalMilliseconds;
			}
			else
			{
				if (gameTime.get_TotalGameTime().TotalMilliseconds - _timeAfterLastDetect < 500.0)
				{
					return;
				}
				_timeAfterLastDetect = gameTime.get_TotalGameTime().TotalMilliseconds;
				if (_prevDetectedText != text2)
				{
					_prevDetectedText = text2;
					string translation = await _translator.TranslateAsync(text2);
					if (!string.IsNullOrWhiteSpace(translation) && _isCutsceneWithMessages)
					{
						_subtitlesLabel.Text = translation;
					}
				}
			}
		}
	}
}
