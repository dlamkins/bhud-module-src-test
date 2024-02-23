namespace Ideka.CustomCombatText
{
	public class ViewTypeContainer : AreaViewType<ModelTypeContainer>
	{
		public ViewTypeContainer(AreaModel model)
			: base(model)
		{
		}

		protected override void AcceptedMessage(MessageReceiver receiver, Message message)
		{
		}
	}
}
