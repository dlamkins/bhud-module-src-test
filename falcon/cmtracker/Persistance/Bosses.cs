using System.Collections.Generic;
using Newtonsoft.Json;

namespace falcon.cmtracker.Persistance
{
	internal class Bosses
	{
		[JsonProperty("tokens")]
		public IList<Token> Tokens { get; set; }

		public Bosses()
		{
			Tokens = GenrateTokens();
		}

		private List<Token> GenrateTokens()
		{
			return new List<Token>
			{
				new Token
				{
					Id = 77302,
					Name = "Keep Construct",
					Icon = "icon_keep_Construct.png",
					setting = Module.W3_Keep_Construct,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Cairn",
					Icon = "icon_cairn.png",
					setting = Module.W4_Cairn,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Mursaat Overseer",
					Icon = "icon_mursaat_overseer.png",
					setting = Module.W4_Mursaat_Overseer,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Samarog",
					Icon = "icon_samarog.png",
					setting = Module.W4_Samarog,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Deimos",
					Icon = "icon_deimos.png",
					setting = Module.W4_Deimos,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Soulless Horror",
					Icon = "icon_sh.png",
					setting = Module.W5_Soulless_Horror,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Dhuum",
					Icon = "icon_dhuum.png",
					setting = Module.W5_Dhuum,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Conjured Amalgamate",
					Icon = "icon_ca.png",
					setting = Module.W6_Conjured_Amalgamate,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Twin Largos",
					Icon = "icon_twin_largos.png",
					setting = Module.W6_Twin_Largos,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Qadim",
					Icon = "icon_qadim.png",
					setting = Module.W6_Qadim,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Adina",
					Icon = "icon_adina.png",
					setting = Module.W7_Adina,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Sabir",
					Icon = "icon_sabir.png",
					setting = Module.W7_Sabir,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Qadim2",
					Icon = "icon_qadim2.png",
					setting = Module.W7_Qadim2,
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Aetherblade Hideout",
					Icon = "icon_aetherblade_hideout.png",
					setting = Module.Strike_Aetherblade_Hideout,
					bossType = BossType.Strike
				},
				new Token
				{
					Id = 77302,
					Name = "Xunlai Jade Junkyard",
					Icon = "icon_xunlai_jade.png",
					setting = Module.Strike_Xunlai_Jade_Junkyard,
					bossType = BossType.Strike
				},
				new Token
				{
					Id = 77302,
					Name = "Kaineng Overlook",
					Icon = "icon_kaineng_overlook.png",
					setting = Module.Strike_Kaineng_Overlook,
					bossType = BossType.Strike
				},
				new Token
				{
					Id = 77302,
					Name = "Harvest Temple",
					Icon = "icon_harvest_temple.png",
					setting = Module.Strike_Harvest_Temple,
					bossType = BossType.Strike
				}
			};
		}
	}
}
