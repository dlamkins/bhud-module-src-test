using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using felix.BlishEmotes.Exceptions;

namespace felix.BlishEmotes
{
	internal class CategoriesManager
	{
		public static readonly string NEW_CATEGORY_NAME = "New Category";

		private static readonly Logger Logger = Logger.GetLogger<CategoriesManager>();

		private PersistenceManager PersistenceManager;

		private Dictionary<Guid, Category> categories;

		public Guid FavouriteCategoryId { get; private set; }

		public event EventHandler<List<Category>> CategoriesUpdated;

		public CategoriesManager(PersistenceManager persistenceManager)
		{
			PersistenceManager = persistenceManager;
			categories = new Dictionary<Guid, Category>();
		}

		public void Load()
		{
			try
			{
				foreach (Category category in PersistenceManager.LoadCategories())
				{
					if (category.IsFavourite)
					{
						FavouriteCategoryId = category.Id;
					}
					categories.Add(category.Id, category);
				}
				_ = FavouriteCategoryId;
			}
			catch (FileNotFoundException)
			{
				SetupDefaultCategories();
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to load categories.");
				Logger.Error(ex.Message);
				Logger.Debug(ex.StackTrace);
			}
		}

		public void ReorderCategories(List<Category> newOrder, bool saveToFile = true)
		{
			if (newOrder.Count != categories.Count)
			{
				Logger.Error("Reordered category list length does not match current category list length.");
				return;
			}
			categories.Clear();
			foreach (Category category in newOrder)
			{
				categories.Add(category.Id, category);
			}
			if (saveToFile)
			{
				PersistenceManager.SaveCategories(categories.Values.ToList());
			}
		}

		public Category CreateCategory(string name, List<Emote> emotes = null, bool saveToFile = true)
		{
			return CreateCategory(name, emotes?.Select((Emote emote) => emote.Id).ToList(), emotes, isFavourite: false, saveToFile);
		}

		private Category CreateCategory(string name, List<string> emoteIds = null, List<Emote> emotes = null, bool isFavourite = false, bool saveToFile = true)
		{
			emoteIds = emoteIds ?? new List<string>();
			emotes = emotes ?? new List<Emote>();
			if (name == NEW_CATEGORY_NAME)
			{
				int next = GetNextNewCategoryNumber();
				if (next > 0)
				{
					name = $"{name} {next}";
				}
			}
			AssertUniqueName(name);
			Category newCategory = new Category
			{
				Id = Guid.NewGuid(),
				Name = name,
				IsFavourite = isFavourite,
				EmoteIds = emoteIds,
				Emotes = emotes
			};
			categories.Add(newCategory.Id, newCategory);
			if (isFavourite)
			{
				FavouriteCategoryId = newCategory.Id;
			}
			if (saveToFile)
			{
				PersistenceManager.SaveCategories(categories.Values.ToList());
			}
			Logger.Debug($"Created category {newCategory.Id}-{newCategory.Name}");
			this.CategoriesUpdated?.Invoke(this, GetAll());
			return newCategory.Clone();
		}

		public Category UpdateCategory(Category category, bool saveToFile = true)
		{
			categories.TryGetValue(category.Id, out var current);
			if (current == null)
			{
				Logger.Debug($"No category found for id {category.Id}");
				throw new NotFoundException($"No category found for id {category.Id}");
			}
			if (current.Name != category.Name)
			{
				AssertUniqueName(category.Name);
			}
			categories[category.Id] = category;
			if (saveToFile)
			{
				PersistenceManager.SaveCategories(categories.Values.ToList());
			}
			Logger.Debug($"Updated category {category.Id}-{category.Name}");
			this.CategoriesUpdated?.Invoke(this, GetAll());
			return category.Clone();
		}

		public bool DeleteCategory(Category category, bool saveToFile = true)
		{
			categories.TryGetValue(category.Id, out var current);
			if (current == null)
			{
				Logger.Debug($"Tried deleting non-existing category with id {category.Id}");
				return true;
			}
			if (current.IsFavourite)
			{
				Logger.Debug("Tried to delete favourite category -> abort.");
				return false;
			}
			categories.Remove(category.Id);
			categories = new Dictionary<Guid, Category>(categories);
			if (saveToFile)
			{
				PersistenceManager.SaveCategories(categories.Values.ToList());
			}
			Logger.Debug($"Deleted category {category.Id}-{category.Name}");
			this.CategoriesUpdated?.Invoke(this, GetAll());
			return true;
		}

		public Category GetById(Guid id)
		{
			categories.TryGetValue(id, out var category);
			if (category == null)
			{
				Logger.Debug($"No category found for id {id}");
				throw new NotFoundException($"No category found for id {id}");
			}
			return category.Clone();
		}

		public List<Category> GetAll()
		{
			return new List<Category>(categories.Values.Select((Category category) => category.Clone()));
		}

		public bool IsEmoteInCategory(Guid categoryId, Emote emote)
		{
			categories.TryGetValue(categoryId, out var category);
			if (category == null)
			{
				Logger.Debug($"No category found for id {categoryId}");
				throw new NotFoundException($"No category found for id {categoryId}");
			}
			return category.EmoteIds.Contains(emote.Id);
		}

		public void ToggleEmoteFromCategory(Guid categoryId, Emote emote, bool saveToFile = true)
		{
			try
			{
				if (IsEmoteInCategory(categoryId, emote))
				{
					categories[categoryId].RemoveEmote(emote);
				}
				else
				{
					categories[categoryId].AddEmote(emote);
				}
				if (saveToFile)
				{
					PersistenceManager.SaveCategories(categories.Values.ToList());
				}
			}
			catch (NotFoundException)
			{
				Logger.Warn("Failed to toggle emote " + emote.Id + " - Category not found!");
			}
		}

		public void ResolveEmoteIds(List<Emote> emotes)
		{
			foreach (Category category in categories.Values)
			{
				category.Emotes = emotes.Where((Emote emote) => category.EmoteIds.Contains(emote.Id)).ToList();
			}
		}

		public void Unload()
		{
			PersistenceManager.SaveCategories(categories.Values.ToList());
		}

		private void SetupDefaultCategories()
		{
			Logger.Debug("SetupDefaultCategories");
			CreateCategory(Category.FAVOURITES_CATEGORY_NAME, null, null, isFavourite: true, saveToFile: false);
			CreateCategory("Greeting", new List<string> { "beckon", "bow", "salute", "wave" }, null, isFavourite: false, saveToFile: false);
			CreateCategory("Reaction", new List<string>
			{
				"cower", "cry", "facepalm", "hiss", "no", "sad", "shiver", "shiverplus", "shrug", "surprised",
				"thanks", "yes"
			}, null, isFavourite: false, saveToFile: false);
			CreateCategory("Fun", new List<string> { "cheer", "laugh", "paper", "rock", "rockout", "scissors" }, null, isFavourite: false, saveToFile: false);
			CreateCategory("Pose", new List<string>
			{
				"bless", "crossarms", "heroic", "kneel", "magicjuggle", "playdead", "point", "serve", "sit", "sleep",
				"stretch", "threaten"
			}, null, isFavourite: false, saveToFile: false);
			CreateCategory("Dance", new List<string> { "dance", "geargrind", "shuffle", "step" }, null, isFavourite: false, saveToFile: false);
			CreateCategory("Miscellaneous", new List<string> { "ponder", "possessed", "rank", "readbook", "sipcoffee", "talk" }, null, isFavourite: false, saveToFile: false);
			PersistenceManager.SaveCategories(categories.Values.ToList());
		}

		private void AssertUniqueName(string name)
		{
			if (categories.Values.Any((Category category) => category.Name == name))
			{
				Logger.Debug("Name must be unique - " + name + " already in use.");
				throw new UniqueViolationException("Name must be unique - " + name + " already in use.");
			}
		}

		private int GetNextNewCategoryNumber()
		{
			List<int> newCategoryNumbers = categories.Values.Where((Category category) => category.Name.StartsWith(NEW_CATEGORY_NAME)).Select(delegate(Category category)
			{
				string text = category.Name.Replace(NEW_CATEGORY_NAME, "").Trim();
				if (text.Length == 0)
				{
					text = "0";
				}
				int result;
				return int.TryParse(text, out result) ? result : 0;
			}).ToList();
			if (newCategoryNumbers.Count != 0)
			{
				return newCategoryNumbers.Max() + 1;
			}
			return 0;
		}
	}
}
