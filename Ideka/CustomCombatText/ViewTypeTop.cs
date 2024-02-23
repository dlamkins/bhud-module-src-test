using System;
using System.Collections.Generic;
using System.Linq;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class ViewTypeTop : AreaViewType<ModelTypeTop>
	{
		private static readonly TimeSpan AnimationTime = TimeSpan.FromSeconds(0.25);

		private const float AnimationAmount = 5f;

		private readonly Queue<(MessageReceiver, Message)> _messageQueue = new Queue<(MessageReceiver, Message)>();

		private readonly MessageViewSet _shownMessages = new MessageViewSet();

		private readonly object _lock = new object();

		public ViewTypeTop(AreaModel model)
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
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			base.EarlyUpdate(gameTime);
			lock (_lock)
			{
				while (_messageQueue.Any())
				{
					(MessageReceiver, Message) tuple = _messageQueue.Dequeue();
					MessageReceiver receiver = tuple.Item1;
					Message message2 = tuple.Item2;
					MessageView previous = _shownMessages.NewToOld.FirstOrDefault((MessageView x) => x.CanMerge(receiver, message2));
					if (previous != null)
					{
						if (previous.WithinMergeTimeout(gameTime.get_TotalGameTime()))
						{
							_shownMessages.Remove(previous);
							previous.AddMessage(gameTime.get_TotalGameTime(), message2);
							_shownMessages.Add(previous);
							continue;
						}
						RemoveChild(previous);
						_shownMessages.Remove(previous);
					}
					MessageView view = new MessageView(receiver)
					{
						PivotX = Settings.MessagePivotX
					};
					view.AddMessage(gameTime.get_TotalGameTime(), message2);
					MessageView old = _shownMessages.OldToNew.FirstOrDefault((MessageView x) => x.CanMergeAny(view));
					if (old != null)
					{
						RemoveChild(old);
						_shownMessages.Remove(old);
					}
					_shownMessages.Add(view);
				}
			}
			HashSet<MessageView> dead = new HashSet<MessageView>();
			float y = 0f;
			foreach (MessageView message3 in _shownMessages.HighToLow)
			{
				RemoveChild(message3);
				if (dead.Contains(message3) || y + message3.SizeDelta.Y > base.FinalHeight)
				{
					continue;
				}
				TimeSpan time = gameTime.get_TotalGameTime() - message3.LastTime;
				double percent = time.TotalSeconds / Settings.MessageTimeout.TotalSeconds;
				if (percent >= 1.0)
				{
					dead.Add(message3);
					continue;
				}
				AddChild(message3);
				message3.Alpha = (float)MathUtils.Scale(percent, 0.800000011920929, 1.0, 1.0, 0.0, clamp: true);
				message3.Anchor = new Vector2(Settings.MessagePivotX, 0f);
				message3.PivotY = 0f;
				message3.PositionY = y;
				if (Settings.AnimateOnHit)
				{
					double animPercent = MathUtils.Clamp01(time.TotalSeconds / AnimationTime.TotalSeconds);
					message3.PositionY = y + (float)MathUtils.Squared(2.0 * animPercent - 1.0) * 5f;
				}
				y += message3.SizeDelta.Y * message3.Alpha;
			}
			foreach (MessageView message in dead)
			{
				RemoveChild(message);
				_shownMessages.Remove(message);
			}
		}
	}
}
