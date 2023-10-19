using System;

namespace Estreya.BlishHUD.EventTable.Contexts
{
	public struct AddDynamicEvent
	{
		public struct AddDynamicEventLocation
		{
			public const string TYPE_POLY = "poly";

			public const string TYPE_SPHERE = "sphere";

			public const string TYPE_CYLINDER = "cylinder";

			public string Type { get; set; }

			public float[] Center { get; set; }

			public float Radius { get; set; }

			public float Height { get; set; }

			public float Rotation { get; set; }

			public float[] ZRange { get; set; }

			public float[][] Points { get; set; }

			public AddDynamicEventLocation()
			{
				Type = null;
				Center = null;
				Radius = 0f;
				Height = 0f;
				Rotation = 0f;
				ZRange = null;
				Points = null;
			}
		}

		public struct AddDynamicEventIcon
		{
			public int FileID { get; set; }

			public string Signature { get; set; }

			public AddDynamicEventIcon()
			{
				FileID = 0;
				Signature = null;
			}
		}

		public Guid? Id { get; set; }

		public string Name { get; set; }

		public int Level { get; set; }

		public int MapId { get; set; }

		public string[] Flags { get; set; }

		public AddDynamicEventLocation? Location { get; set; }

		public AddDynamicEventIcon? Icon { get; set; }

		public string ColorCode { get; set; }

		public AddDynamicEvent()
		{
			Id = null;
			Name = null;
			Level = 0;
			MapId = 0;
			Flags = null;
			Location = null;
			Icon = null;
			ColorCode = null;
		}
	}
}
