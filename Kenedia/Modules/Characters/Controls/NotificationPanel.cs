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
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			base.Height = 100;
			base.Width = 400;
			base.FlowDirection = ControlFlowDirection.SingleTopToBottom;
			base.CanScroll = true;
			_characters = characters;
			UpdateCharacters();
		}

		public void UpdateCharacters()
		{
			List<Character_Model> addedCharacters = _markedCharacters.Select<(Character_Model, CharacterDeletedNotification), Character_Model>(((Character_Model character, CharacterDeletedNotification control) e) => e.character).ToList();
			foreach (Character_Model character in _characters)
			{
				if (character.MarkedAsDeleted && addedCharacters.Find((Character_Model e) => e == character) == null)
				{
					_markedCharacters.Add((character, new CharacterDeletedNotification
					{
						Parent = this,
						Height = 25,
						MarkedCharacter = character
					}));
				}
			}
			RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			SortChildren(delegate(BaseNotification a, BaseNotification b)
			{
				int num = a.NotificationType.CompareTo(b.NotificationType);
				int num2 = a.Id.CompareTo(b.Id);
				return (num != 0) ? num : (num + num2);
			});
			foreach (Control child in base.Children)
			{
				child.Width = base.ContentRegion.Width;
			}
			base.Height = ((MaxSize.Y > -1) ? Math.Min(MaxSize.Y, (base.Children.Count > 0) ? base.Children.Max((Control e) => e.Bottom) : 0) : ((base.Children.Count > 0) ? base.Children.Max((Control e) => e.Bottom) : 0));
			if (base.Children != null && base.Children.Count != 0)
			{
				ControlCollection<Control> children = base.Children;
				if (children == null || !(children.Where((Control e) => e.Visible)?.Count() <= 0))
				{
					Show();
					return;
				}
			}
			Hide();
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			base.OnChildRemoved(e);
			Invalidate();
			base.Parent?.Invalidate();
			if (base.Children != null && base.Children.Count != 0)
			{
				ControlCollection<Control> children = base.Children;
				if (children == null || !(children.Where((Control e) => e.Visible)?.Count() <= 0))
				{
					return;
				}
			}
			Hide();
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			base.OnChildAdded(e);
			Show();
			Invalidate();
			base.Parent?.Invalidate();
		}
	}
}
