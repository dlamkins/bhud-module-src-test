using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using felix.BlishEmotes.Services;

namespace felix.BlishEmotes
{
	public class Category : RadialBase
	{
		[JsonIgnore]
		public static readonly string FAVOURITES_CATEGORY_NAME = "Favourites";

		[JsonIgnore]
		public static readonly string VERSION = "V1";

		[JsonProperty("id", Required = Required.Always)]
		public Guid Id { get; set; }

		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; }

		[JsonProperty("emoteIds", Required = Required.Always)]
		public List<string> EmoteIds { get; set; }

		[JsonProperty("isFavourite", DefaultValueHandling = DefaultValueHandling.Populate)]
		public bool IsFavourite { get; set; }

		[JsonProperty("textureFileName", DefaultValueHandling = DefaultValueHandling.Populate)]
		[DefaultValue(Textures.CategorySettingsIcon)]
		public string TextureFileName { get; set; }

		[JsonIgnore]
		public List<Emote> Emotes { get; set; } = new List<Emote>();


		[JsonIgnore]
		public override string Label => Name;

		public Category Clone()
		{
			return new Category
			{
				Id = Id,
				Name = Name,
				EmoteIds = new List<string>(EmoteIds),
				Emotes = new List<Emote>(Emotes),
				IsFavourite = IsFavourite,
				TextureFileName = TextureFileName,
				Texture = base.Texture
			};
		}

		public void AddEmote(Emote emote)
		{
			if (!EmoteIds.Contains(emote.Id))
			{
				EmoteIds.Add(emote.Id);
				Emotes.Add(emote);
			}
		}

		public void RemoveEmote(Emote emote)
		{
			EmoteIds.Remove(emote.Id);
			Emotes.RemoveAll((Emote e) => e.Id == emote.Id);
		}
	}
}
