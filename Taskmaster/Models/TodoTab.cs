using System;
using System.Collections.Generic;
using System.Linq;

namespace Taskmaster.Models
{
	public class TodoTab
	{
		public Guid Id { get; set; } = Guid.NewGuid();


		public string Name { get; set; } = "";


		public int Order { get; set; }

		public List<TodoTask> Tasks { get; set; } = new List<TodoTask>();


		public string AccentColorHex { get; set; }

		public int DoneCount => Tasks.Count((TodoTask t) => t.IsDone);

		public int TotalCount => Tasks.Count;
	}
}
