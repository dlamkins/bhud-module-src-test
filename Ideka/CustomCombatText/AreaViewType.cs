using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Ideka.CustomCombatText
{
	public abstract class AreaViewType : AnchoredRect
	{
		public AreaModel Model { get; }

		protected AreaViewType(AreaModel model)
		{
			Model = model;
			base._002Ector();
		}

		public void ReceiveMessage(Message message)
		{
			foreach (MessageReceiver receiver in Model.Receivers)
			{
				if (receiver.Matches(message))
				{
					AcceptedMessage(receiver, message);
				}
			}
		}

		protected abstract void AcceptedMessage(MessageReceiver receiver, Message message);

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (CTextModule.Settings.Debug.Value)
			{
				ShapeExtensions.DrawRectangle(target.SpriteBatch, target.Rect, Color.get_Black(), 1f, 0f);
			}
		}
	}
	public abstract class AreaViewType<TModel> : AreaViewType where TModel : IAreaModelType, new()
	{
		protected const float FadeLength = 0.2f;

		protected readonly TModel Settings;

		protected float FinalHeight { get; private set; }

		protected AreaViewType(AreaModel model)
		{
			IAreaModelType modelType = model.ModelType;
			object settings;
			if (modelType is TModel)
			{
				settings = (TModel)modelType;
			}
			else
			{
				settings = (TModel)(model.ModelType ?? (model.ModelType = new TModel()));
			}
			Settings = (TModel)settings;
			base._002Ector(model);
		}

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			base.EarlyDraw(target);
			FinalHeight = target.Rect.Height;
		}
	}
}
