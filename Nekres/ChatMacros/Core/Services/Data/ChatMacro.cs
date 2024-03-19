using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Microsoft.Xna.Framework;

namespace Nekres.ChatMacros.Core.Services.Data
{
	internal class ChatMacro : BaseMacro
	{
		[BsonRef("chat_lines")]
		[BsonField("lines")]
		public List<ChatLine> Lines { get; set; }

		public ChatMacro()
		{
			Lines = new List<ChatLine>();
		}

		public override Color GetDisplayColor()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (!Lines.IsNullOrEmpty())
			{
				return Lines[0].Channel.GetHeadingColor();
			}
			return base.GetDisplayColor();
		}

		public List<string> ToChatMessage()
		{
			return Lines.Select((ChatLine line) => line.ToChatMessage()).ToList();
		}
	}
}
