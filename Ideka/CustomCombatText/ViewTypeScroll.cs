using System;
using System.Collections.Generic;
using System.Linq;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class ViewTypeScroll : AreaViewType<ModelTypeScroll>
	{
		private readonly Queue<(MessageReceiver, Message)> _messageQueue = new Queue<(MessageReceiver, Message)>();

		private readonly List<MessageView> _shownMessages = new List<MessageView>();

		private readonly object _lock = new object();

		public ViewTypeScroll(AreaModel model)
			: base(model)
		{
		}

		protected override void AcceptedMessage(MessageReceiver receiver, Message message)
		{
			lock (_lock)
			{
				_messageQueue.Enqueue((receiver, message));
			}
		}

		protected override void EarlyUpdate(GameTime gameTime)
		{
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			GameTime gameTime2 = gameTime;
			base.EarlyUpdate(gameTime2);
			lock (_lock)
			{
				while (_messageQueue.Any())
				{
					var (receiver, message) = _messageQueue.Peek();
					if (!_shownMessages.Any())
					{
						showMessage(null);
						continue;
					}
					MessageView merge2 = _shownMessages.OrderByDescending((MessageView x) => x.LastTime).FirstOrDefault((MessageView x) => x.CanMerge(receiver, message) && x.WithinMergeTimeout(gameTime2.get_TotalGameTime()));
					if (merge2 != null)
					{
						showMessage(merge2);
						continue;
					}
					TimeSpan timeSpan = TimeSpan.FromSeconds(receiver.Font.get_LineHeight() / Settings.ScrollSpeed);
					TimeSpan lastMessageTime = gameTime2.get_TotalGameTime() - _shownMessages.Last().Time;
					TimeSpan timeUntilNextMessageCanBeShown = timeSpan - lastMessageTime;
					if (timeUntilNextMessageCanBeShown <= TimeSpan.Zero)
					{
						showMessage(null);
						continue;
					}
					if (_messageQueue.Count <= Settings.MaxQueueSize)
					{
						break;
					}
					foreach (MessageView shownMessage in _shownMessages)
					{
						shownMessage.Time -= timeUntilNextMessageCanBeShown;
					}
					showMessage(null);
					void showMessage(MessageView? merge)
					{
						_messageQueue.Dequeue();
						MessageView view = merge ?? new MessageView(receiver)
						{
							PivotX = Settings.MessagePivotX
						};
						view.AddMessage(gameTime2.get_TotalGameTime(), message);
						if (merge == null)
						{
							_shownMessages.Add(view);
							AddChild(view);
						}
					}
				}
			}
			for (int i = _shownMessages.Count - 1; i >= 0; i--)
			{
				MessageView message2 = _shownMessages[i];
				float percent = (float)((gameTime2.get_TotalGameTime() - message2.Time).TotalSeconds * (double)Settings.ScrollSpeed) / (base.FinalHeight - message2.SizeDelta.Y);
				if (percent >= 1f)
				{
					_shownMessages.RemoveAt(i);
					RemoveChild(message2);
				}
				else
				{
					message2.Alpha = (float)MathUtils.Scale(percent, 0.800000011920929, 1.0, 1.0, 0.0, clamp: true);
					MessageView messageView = message2;
					messageView.Anchor = new Vector2(Settings.CurveType switch
					{
						ModelTypeScroll.Curve.Left => MathUtils.Squared(2f * percent - 1f), 
						ModelTypeScroll.Curve.Right => 1f - MathUtils.Squared(2f * percent - 1f), 
						_ => Settings.MessagePivotX, 
					}, percent);
					message2.PivotY = percent;
				}
			}
		}
	}
}
