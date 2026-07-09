using System.Collections.Generic;
using System.Linq;

namespace rp.spark.Services
{
	public class ProfileValidationResult
	{
		public List<string> Errors { get; } = new List<string>();


		public bool IsValid => !Errors.Any();

		public void AddError(string error)
		{
			Errors.Add(error);
		}
	}
}
