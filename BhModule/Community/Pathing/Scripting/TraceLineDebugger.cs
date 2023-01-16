using System.Linq.Expressions;
using Neo.IronLua;

namespace BhModule.Community.Pathing.Scripting
{
	public class TraceLineDebugger : LuaTraceLineDebugger
	{
		public string LastFrameSource { get; private set; }

		public string LastFrameScope { get; private set; }

		public int LastFrameLine { get; private set; }

		protected override LuaTraceChunk CreateChunk(Lua lua, LambdaExpression expr)
		{
			return base.CreateChunk(lua, expr);
		}

		protected override void OnExceptionUnwind(LuaTraceLineExceptionEventArgs e)
		{
			LastFrameSource = e.SourceName;
			LastFrameScope = e.ScopeName;
			LastFrameLine = e.SourceLine;
			base.OnExceptionUnwind(e);
		}

		protected override void OnFrameEnter(LuaTraceLineEventArgs e)
		{
			LastFrameSource = e.SourceName;
			LastFrameScope = e.ScopeName;
			LastFrameLine = e.SourceLine;
			base.OnFrameEnter(e);
		}

		protected override void OnFrameExit()
		{
			LastFrameSource = null;
			LastFrameScope = null;
			LastFrameLine = 0;
			base.OnFrameExit();
		}

		protected override void OnTracePoint(LuaTraceLineEventArgs e)
		{
			LastFrameSource = e.SourceName;
			LastFrameScope = e.ScopeName;
			LastFrameLine = e.SourceLine;
			base.OnTracePoint(e);
		}
	}
}
