using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Extensions;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class WindowsUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(WindowsUtil));

		public static async Task<string> GetDxDiagInformation()
		{
			ProcessStartInfo psi = new ProcessStartInfo();
			if (IntPtr.Size == 4 && Environment.Is64BitOperatingSystem)
			{
				psi.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "sysnative\\dxdiag.exe");
			}
			else
			{
				psi.FileName = Path.Combine(Environment.SystemDirectory, "dxdiag.exe");
			}
			string path = Path.GetTempFileName();
			try
			{
				psi.Arguments = "/x " + path;
				using (Process prc = Process.Start(psi))
				{
					await prc.WaitForExitAsync();
					if (prc.ExitCode != 0)
					{
						Logger.Warn($"DXDIAG failed with exit code {prc.ExitCode}");
						return null;
					}
				}
				StringBuilder stringBuilder = new StringBuilder();
				string dxDiagContent = File.ReadAllText(path);
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(dxDiagContent);
				stringBuilder.AppendLine("**---System Information---**");
				stringBuilder.AppendLine();
				foreach (XmlNode systemInformationChildNode in doc.DocumentElement.SelectSingleNode("/DxDiag/SystemInformation").ChildNodes)
				{
					stringBuilder.AppendLine("**" + systemInformationChildNode.Name + "**: " + systemInformationChildNode.InnerText);
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("**---Display Devices---**");
				stringBuilder.AppendLine();
				XmlNodeList displayDeviceNodes = doc.DocumentElement.SelectNodes("/DxDiag/DisplayDevices/DisplayDevice");
				for (int i = 0; i < displayDeviceNodes.Count; i++)
				{
					foreach (XmlNode displayDeviceChildNode in displayDeviceNodes.Item(i).ChildNodes)
					{
						stringBuilder.AppendLine("**" + displayDeviceChildNode.Name + "**: " + displayDeviceChildNode.InnerText);
					}
					if (i < displayDeviceNodes.Count - 1)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine("**----------------**");
						stringBuilder.AppendLine();
					}
				}
				return stringBuilder.ToString();
			}
			finally
			{
				File.Delete(path);
			}
		}
	}
}
