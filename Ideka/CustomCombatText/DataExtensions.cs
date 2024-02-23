namespace Ideka.CustomCombatText
{
	public static class DataExtensions
	{
		public static string? Describe(this AreaType type)
		{
			return type switch
			{
				AreaType.Container => "Container", 
				AreaType.Scroll => "Scroll", 
				AreaType.Top => "Top", 
				_ => null, 
			};
		}

		public static string? Describe(this EntityFilter type)
		{
			return type switch
			{
				EntityFilter.Any => "Any", 
				EntityFilter.TargetOnly => "Target only", 
				EntityFilter.NonSelf => "Non self", 
				_ => null, 
			};
		}

		public static string? Describe(this EventResult type)
		{
			return type switch
			{
				EventResult.Strike => "Strike", 
				EventResult.Crit => "Crit", 
				EventResult.Glance => "Glance", 
				EventResult.Block => "Block", 
				EventResult.Evade => "Evade", 
				EventResult.Invuln => "Invuln", 
				EventResult.Miss => "Miss", 
				EventResult.Bleeding => "Bleeding", 
				EventResult.Burning => "Burning", 
				EventResult.Poison => "Poison", 
				EventResult.Confusion => "Confusion", 
				EventResult.Torment => "Torment", 
				EventResult.DamageTick => "Damage tick", 
				EventResult.Heal => "Heal", 
				EventResult.Barrier => "Barrier", 
				EventResult.HealTick => "Heal tick", 
				EventResult.Interrupt => "Interrupt", 
				EventResult.Breakbar => "Breakbar", 
				_ => null, 
			};
		}

		public static string? Describe(this MessageCategory type)
		{
			return type switch
			{
				MessageCategory.PlayerOut => "Player out", 
				MessageCategory.PlayerIn => "Player in", 
				MessageCategory.PetOut => "Pet out", 
				MessageCategory.PetIn => "Pet in", 
				_ => null, 
			};
		}

		public static string? Describe(this ModelTypeScroll.Curve type)
		{
			return type switch
			{
				ModelTypeScroll.Curve.None => "None", 
				ModelTypeScroll.Curve.Left => "Left", 
				ModelTypeScroll.Curve.Right => "Right", 
				_ => null, 
			};
		}
	}
}
