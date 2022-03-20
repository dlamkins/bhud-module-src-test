using System;
using BhModule.Community.Pathing.Entity;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.State.UserResources.Population
{
	public class MarkerPopulationDefaults
	{
		public float Alpha { get; set; } = 1f;


		public Color Tint { get; set; } = Color.get_White();


		public float FadeNear { get; set; } = -1f;


		public float FadeFar { get; set; } = -1f;


		public float HeightOffset { get; set; } = 1.5f;


		public float IconSize { get; set; } = 1f;


		public float MinSize { get; set; } = 5f;


		public float MaxSize { get; set; } = 2048f;


		public float TriggerRange { get; set; } = 2f;


		public float MapDisplaySize { get; set; } = 20f;


		public bool MapVisibility { get; set; } = true;


		public CullDirection Cull { get; set; }

		public Guid Guid { get; set; } = Guid.Empty;


		public bool MiniMapVisibility { get; set; } = true;


		public bool ScaleOnMapWithZoom { get; set; } = true;


		public bool InGameVisibility { get; set; } = true;


		public Vector3 RotateXyz { get; set; } = Vector3.get_Zero();


		public Color TitleColor { get; set; } = Color.get_White();


		public bool CanFade { get; set; } = true;

	}
}
