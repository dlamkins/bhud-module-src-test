using Microsoft.Xna.Framework;

namespace Charr.Timers_BlishHUD.Pathing.Entities
{
	public class EntityBillboard : Billboard
	{
		public Entity AttachedEntity { get; }

		public override Vector3 Position => AttachedEntity.Position + AttachedEntity.RenderOffset;

		public EntityBillboard(Entity attachedEntity)
		{
			AttachedEntity = attachedEntity;
		}
	}
}
