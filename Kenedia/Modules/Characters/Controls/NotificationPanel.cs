using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Models;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public class NotificationPanel : FlowPanel
	{
		private readonly ObservableCollection<Character_Model> _characters;

		private readonly List<(Character_Model character, CharacterDeletedNotification control)> _markedCharacters = new List<(Character_Model, CharacterDeletedNotification)>();

		public Point MaxSize { get; set; } = Point.get_Zero();


		public NotificationPanel(ObservableCollection<Character_Model> characters)
			: this()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Height(100);
			((Control)this).set_Width(400);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Panel)this).set_CanScroll(true);
			_characters = characters;
			UpdateCharacters();
		}

		public void UpdateCharacters()
		{
			List<Character_Model> addedCharacters = _markedCharacters.Select(((Character_Model character, CharacterDeletedNotification control) e) => e.character).ToList();
			foreach (Character_Model character in _characters)
			{
				if (character.MarkedAsDeleted && addedCharacters.Find((Character_Model e) => e == character) == null)
				{
					List<(Character_Model character, CharacterDeletedNotification control)> markedCharacters = _markedCharacters;
					Character_Model item = character;
					CharacterDeletedNotification characterDeletedNotification = new CharacterDeletedNotification();
					((Control)characterDeletedNotification).set_Parent((Container)(object)this);
					((Control)characterDeletedNotification).set_Height(25);
					characterDeletedNotification.MarkedCharacter = character;
					markedCharacters.Add((item, characterDeletedNotification));
				}
			}
			((Control)this).RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).RecalculateLayout();
			((FlowPanel)this).SortChildren<BaseNotification>((Comparison<BaseNotification>)delegate(BaseNotification a, BaseNotification b)
			{
				int num = a.NotificationType.CompareTo(b.NotificationType);
				int num2 = a.Id.CompareTo(b.Id);
				return (num != 0) ? num : (num + num2);
			});
			foreach (Control child in ((Container)this).get_Children())
			{
				child.set_Width(((Container)this).get_ContentRegion().Width);
			}
			((Control)this).set_Height((MaxSize.Y > -1) ? Math.Min(MaxSize.Y, (((Container)this).get_Children().get_Count() > 0) ? ((IEnumerable<Control>)((Container)this).get_Children()).Max((Control e) => e.get_Bottom()) : 0) : ((((Container)this).get_Children().get_Count() > 0) ? ((IEnumerable<Control>)((Container)this).get_Children()).Max((Control e) => e.get_Bottom()) : 0));
			if (((Container)this).get_Children() != null && ((Container)this).get_Children().get_Count() != 0)
			{
				ControlCollection<Control> children = ((Container)this).get_Children();
				if (children == null || !(((IEnumerable<Control>)children).Where((Control e) => e.get_Visible())?.Count() <= 0))
				{
					((Control)this).Show();
					return;
				}
			}
			((Control)this).Hide();
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			((FlowPanel)this).OnChildRemoved(e);
			((Control)this).Invalidate();
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
			if (((Container)this).get_Children() != null && ((Container)this).get_Children().get_Count() != 0)
			{
				ControlCollection<Control> children = ((Container)this).get_Children();
				if (children == null || !(((IEnumerable<Control>)children).Where((Control e) => e.get_Visible())?.Count() <= 0))
				{
					return;
				}
			}
			((Control)this).Hide();
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			((FlowPanel)this).OnChildAdded(e);
			((Control)this).Show();
			((Control)this).Invalidate();
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
		}
	}
}
