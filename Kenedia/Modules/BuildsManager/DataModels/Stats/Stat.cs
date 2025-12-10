using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.DataModels.Stats
{
	[DataContract]
	public class Stat : IDisposable, IDataMember
	{
		public class StatTextureMapInfo
		{
			private Point _startOffset = new Point(8, 8);

			private Point _textureSize = new Point(36, 36);

			private Point _textureShift = new Point(44, 0);

			public string Name { get; }

			public List<int> Ids { get; }

			public int Position { get; }

			[JsonIgnore]
			public Rectangle TextureRectangle { get; }

			public StatTextureMapInfo(string name, List<int> ids, int position)
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				Name = name;
				Ids = ids;
				Position = position;
				TextureRectangle = new Rectangle(_startOffset.X + _textureShift.X * (position - 1), _startOffset.Y + _textureShift.Y * (position - 1), _textureSize.X, _textureSize.Y);
			}

			public bool MatchesId(int id)
			{
				return Ids.Contains(id);
			}
		}

		private bool _isDisposed;

		public static List<StatTextureMapInfo> StatTextureMap { get; set; } = new List<StatTextureMapInfo>();


		public string DisplayAttributes => Attributes.ToString(0.0);

		[DataMember]
		public StatAttributes Attributes { get; set; } = new StatAttributes();


		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public byte MappedId { get; set; }

		[DataMember]
		public LocalizedString Names { get; protected set; } = new LocalizedString();


		public string Name
		{
			get
			{
				return Names.Text;
			}
			set
			{
				Names.Text = value;
			}
		}

		public DetailedTexture Icon
		{
			get
			{
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				if (_003CIcon_003Ek__BackingField == null)
				{
					_003CIcon_003Ek__BackingField = new DetailedTexture(TexturesService.GetTextureFromDisk(Path.Combine(BuildsManager.Data.Paths.ModuleDataPath, "stat_map.png")))
					{
						TextureRegion = TextureInfo.TextureRectangle
					};
				}
				return _003CIcon_003Ek__BackingField;
			}
			set
			{
				_003CIcon_003Ek__BackingField = value;
			}
		}

		public StatTextureMapInfo TextureInfo
		{
			get
			{
				if (_003CTextureInfo_003Ek__BackingField == null)
				{
					_003CTextureInfo_003Ek__BackingField = StatTextureMap?.FirstOrDefault((StatTextureMapInfo e) => e.Ids.Contains(Id));
				}
				return _003CTextureInfo_003Ek__BackingField;
			}
		}

		public Stat()
		{
		}

		public Stat(Itemstat stat)
		{
			Apply(stat);
		}

		public void Apply(Itemstat stat)
		{
			Name = stat.Name;
			Id = stat.Id;
			foreach (ItemstatAttribute att in stat.Attributes)
			{
				Attributes[att.Attribute.ToEnum()] = new StatAttribute(att);
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Icon = null;
			}
		}
	}
}
