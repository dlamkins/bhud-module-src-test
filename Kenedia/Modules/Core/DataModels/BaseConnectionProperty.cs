using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Kenedia.Modules.Core.DataModels
{
	public class BaseConnectionProperty
	{
		[JsonIgnore]
		public List<int?> Values
		{
			get
			{
				List<int?> list = new List<int?>();
				PropertyInfo[] properties = GetType().GetProperties();
				foreach (PropertyInfo propInfo in properties)
				{
					if (propInfo.PropertyType == typeof(int?) && propInfo.GetIndexParameters().Length == 0)
					{
						list.Add((int?)propInfo.GetValue(this));
					}
				}
				return list;
			}
		}

		[JsonIgnore]
		public int? this[string key]
		{
			get
			{
				return this[key];
			}
			set
			{
				this[key] = value;
			}
		}

		public void Clear()
		{
			PropertyInfo[] properties = GetType().GetProperties();
			foreach (PropertyInfo propInfo in properties)
			{
				if (propInfo.PropertyType == typeof(int?) && propInfo.GetIndexParameters().Length == 0)
				{
					propInfo.SetValue(this, null);
				}
			}
		}

		public bool Contains(int? id)
		{
			return Values.Contains(id);
		}

		public bool HasValues()
		{
			return Values.Any((int? e) => e.HasValue);
		}
	}
}
