using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using FontStashSharp;
using LoreBridge.Modules.Chat.Models;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.Chat.Controls
{
	public sealed class TranslationPanel : FlowPanel
	{
		private const int OuterPadding = 6;

		private const int InnerPadding = 0;

		private readonly List<TranslationItemPanel> _entries = new List<TranslationItemPanel>();

		private readonly Messages _messages;

		private SpriteFontBase _font;

		public TranslationPanel(Messages messages, SpriteFontBase font)
			: this()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_OuterControlPadding(new Vector2(0f, 6f));
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 0f));
			_font = font;
			_messages = messages;
			_messages.Added += OnAdded;
			_messages.Updated += OnUpdated;
			_messages.Cleared += OnCleared;
		}

		public void UpdateFont(SpriteFontBase font)
		{
			_font = font;
			foreach (TranslationItemPanel entry in _entries)
			{
				entry.UpdateFont(font);
			}
		}

		private void OnAdded(object sender, Message e)
		{
			List<TranslationItemPanel> entries = _entries;
			TranslationItemPanel translationItemPanel = new TranslationItemPanel(e, _font);
			((Control)translationItemPanel).set_Parent((Container)(object)this);
			entries.Add(translationItemPanel);
		}

		private void OnUpdated(object sender, SortedList<ulong, Message> e)
		{
			foreach (TranslationItemPanel entry in _entries)
			{
				((Control)entry).Dispose();
			}
			foreach (KeyValuePair<ulong, Message> message in e)
			{
				List<TranslationItemPanel> entries = _entries;
				TranslationItemPanel translationItemPanel = new TranslationItemPanel(message.Value, _font);
				((Control)translationItemPanel).set_Parent((Container)(object)this);
				entries.Add(translationItemPanel);
			}
		}

		private void OnCleared(object sender, EventArgs e)
		{
			foreach (TranslationItemPanel entry in _entries)
			{
				((Control)entry).Dispose();
			}
		}
	}
}
