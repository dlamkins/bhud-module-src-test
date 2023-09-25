using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace felix.BlishEmotes
{
	internal class Category
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

		[JsonIgnore]
		public List<Emote> Emotes { get; set; } = new List<Emote>();


		public Category Clone()
		{
			return new Category
			{
				Id = Id,
				Name = Name,
				EmoteIds = new List<string>(EmoteIds),
				Emotes = new List<Emote>(Emotes),
				IsFavourite = IsFavourite
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
