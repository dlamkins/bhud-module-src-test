using System.Collections.Generic;
using Newtonsoft.Json;

namespace falcon.cmtracker.Persistance
{
	internal class Bosses
	{
		[JsonProperty("tokens")]
		public IList<Token> Tokens { get; set; }

		public Bosses(List<SettingValue> setting)
		{
			Tokens = GenrateTokens(setting);
		}

		private List<Token> GenrateTokens(List<SettingValue> setting)
		{
			return new List<Token>
			{
				new Token
				{
					Id = 77302,
					Name = "Keep Construct",
					Icon = "icon_keep_Construct.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Keep_Construct),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Cairn",
					Icon = "icon_cairn.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Cairn),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Mursaat Overseer",
					Icon = "icon_mursaat_overseer.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Mursaat_Overseer),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Samarog",
					Icon = "icon_samarog.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Samarog),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Deimos",
					Icon = "icon_deimos.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Deimos),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Soulless Horror",
					Icon = "icon_sh.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Soulless_Horror),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Dhuum",
					Icon = "icon_dhuum.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Dhuum),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Conjured Amalgamate",
					Icon = "icon_ca.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Conjured_Amalgamate),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Twin Largos",
					Icon = "icon_twin_largos.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Twin_Largos),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Qadim",
					Icon = "icon_qadim.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Qadim),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Adina",
					Icon = "icon_adina.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Adina),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Sabir",
					Icon = "icon_sabir.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Sabir),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Qadim the Peerless",
					Icon = "icon_qadim2.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Qadim2),
					bossType = BossType.Raid
				},
				new Token
				{
					Id = 77302,
					Name = "Aetherblade Hideout",
					Icon = "icon_aetherblade_hideout.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Aetherblade_Hideout),
					bossType = BossType.Strike
				},
				new Token
				{
					Id = 77302,
					Name = "Xunlai Jade Junkyard",
					Icon = "icon_xunlai_jade.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Xunlai_Jade_Junkyard),
					bossType = BossType.Strike
				},
				new Token
				{
					Id = 77302,
					Name = "Kaineng Overlook",
					Icon = "icon_kaineng_overlook.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Kaineng_Overlook),
					bossType = BossType.Strike
				},
				new Token
				{
					Id = 77302,
					Name = "Harvest Temple",
					Icon = "icon_harvest_temple.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Harvest_Temple),
					bossType = BossType.Strike
				},
				new Token
				{
					Id = 77302,
					Name = "Old Lion's Court",
					Icon = "icon_old.png",
					setting = SettingUtil.GetSettingForBoss(setting, Module.CURRENT_ACCOUNT.get_Value(), Boss.Old),
					bossType = BossType.Strike
				}
			};
		}
	}
}
