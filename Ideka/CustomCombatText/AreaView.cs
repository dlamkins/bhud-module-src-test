using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public sealed class AreaView : AreaViewBase
	{
		public AreaModel Model => ViewType.Model;

		public AreaType AreaType => Model.ModelType.Type;

		public IAreaModelType ModelType => Model.ModelType;

		public AreaViewType ViewType { get; set; }

		protected override Anchoring Anchoring => Model.Anchoring;

		public AreaView(AreaViewType view)
		{
			ViewType = view;
			base._002Ector();
		}

		public void ReceiveMessage(Message message)
		{
			if (!Model.Enabled)
			{
				return;
			}
			ViewType.ReceiveMessage(message);
			foreach (AreaView areaViewChild in this.GetAreaViewChildren())
			{
				areaViewChild.ReceiveMessage(message);
			}
		}

		protected override void EarlyUpdate(GameTime gameTime)
		{
			base.EarlyUpdate(gameTime);
			ViewType.Update(gameTime);
		}

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			base.EarlyDraw(target);
			ViewType.Draw(target.SpriteBatch, target.Control, target.Rect);
		}
	}
}
