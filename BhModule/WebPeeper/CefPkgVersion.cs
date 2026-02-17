using System;

namespace BhModule.WebPeeper
{
	internal class CefPkgVersion
	{
		public readonly Version CefSharp = new Version(cefSharpversion);

		public readonly Version Cef = new Version(string.IsNullOrEmpty(cefVersion) ? cefSharpversion : cefVersion);

		public CefPkgVersion(string cefSharpversion, string cefVersion = "")
		{
		}

		public override string ToString()
		{
			return CefSharp.ToString();
		}

		public override bool Equals(object obj)
		{
			CefPkgVersion cefPkgVersion = obj as CefPkgVersion;
			if ((object)cefPkgVersion != null)
			{
				return cefPkgVersion == this;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return CefSharp.GetHashCode();
		}

		public static bool operator >(CefPkgVersion v1, CefPkgVersion v2)
		{
			return v1.CefSharp > v2.CefSharp;
		}

		public static bool operator <(CefPkgVersion v1, CefPkgVersion v2)
		{
			return v1.CefSharp < v2.CefSharp;
		}

		public static bool operator >=(CefPkgVersion v1, CefPkgVersion v2)
		{
			return v1.CefSharp > v2.CefSharp;
		}

		public static bool operator <=(CefPkgVersion v1, CefPkgVersion v2)
		{
			return v1.CefSharp < v2.CefSharp;
		}

		public static bool operator ==(CefPkgVersion v1, CefPkgVersion v2)
		{
			return v1.CefSharp == v2.CefSharp;
		}

		public static bool operator !=(CefPkgVersion v1, CefPkgVersion v2)
		{
			return v1.CefSharp != v2.CefSharp;
		}
	}
}
