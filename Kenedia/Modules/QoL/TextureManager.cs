using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL
{
	public class TextureManager : IDisposable
	{
		public IDictionary<string, AsyncTexture2D> Textures = new Dictionary<string, AsyncTexture2D>();

		private ContentsManager ContentsManager;

		public AsyncTexture2D PlaceHolder;

		private bool disposed;

		public TextureManager()
		{
			ContentsManager = QoL.ModuleInstance.ContentsManager;
			PlaceHolder = AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("textures\\icons\\0.png"));
		}

		public void Dispose()
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
			PlaceHolder.Dispose();
			foreach (KeyValuePair<string, AsyncTexture2D> texture in Textures)
			{
				AsyncTexture2D value = texture.Value;
				if (value != null)
				{
					value.Dispose();
				}
			}
		}

		public Texture2D get(string subfolder, int e)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			string key = "textures\\" + subfolder + "\\" + e + ".png";
			if (!Textures.ContainsKey(key))
			{
				Texture2D texture = ContentsManager.GetTexture(key);
				Textures[key] = new AsyncTexture2D(texture);
				return AsyncTexture2D.op_Implicit(Textures[key]);
			}
			return AsyncTexture2D.op_Implicit(Textures[key]);
		}

		public Texture2D getBackground(_Backgrounds e = _Backgrounds.MainWindow)
		{
			return getBackground(null, e);
		}

		public Texture2D getBackground(string subfolder = null, _Backgrounds e = _Backgrounds.MainWindow)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			subfolder = ((subfolder == null) ? "" : (subfolder + "\\"));
			string text = subfolder;
			int num = (int)e;
			string key = "textures\\backgrounds\\" + text + num + ".png";
			if (!Textures.ContainsKey(key))
			{
				Texture2D texture = ContentsManager.GetTexture(key);
				Textures[key] = new AsyncTexture2D(texture);
				return AsyncTexture2D.op_Implicit(Textures[key]);
			}
			return AsyncTexture2D.op_Implicit(Textures[key]);
		}

		public Texture2D getIcon(_Icons e = _Icons.Bug)
		{
			return getIcon(null, e);
		}

		public Texture2D getIcon(string subfolder = null, _Icons e = _Icons.Bug)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			subfolder = ((subfolder == null) ? "" : (subfolder + "\\"));
			string text = subfolder;
			int num = (int)e;
			string key = "textures\\icons\\" + text + num + ".png";
			QoL.Logger.Debug(key);
			if (!Textures.ContainsKey(key))
			{
				Texture2D texture = ContentsManager.GetTexture(key);
				Textures[key] = new AsyncTexture2D(texture);
				return AsyncTexture2D.op_Implicit(Textures[key]);
			}
			return AsyncTexture2D.op_Implicit(Textures[key]);
		}

		public Texture2D getEmblem(_Emblems e = _Emblems.QuestionMark)
		{
			return getEmblem(null, e);
		}

		public Texture2D getEmblem(string subfolder = null, _Emblems e = _Emblems.QuestionMark)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			subfolder = ((subfolder == null) ? "" : (subfolder + "\\"));
			string text = subfolder;
			int num = (int)e;
			string key = "textures\\emblems\\" + text + num + ".png";
			if (!Textures.ContainsKey(key))
			{
				Texture2D texture = ContentsManager.GetTexture(key);
				Textures[key] = new AsyncTexture2D(texture);
				return AsyncTexture2D.op_Implicit(Textures[key]);
			}
			return AsyncTexture2D.op_Implicit(Textures[key]);
		}

		public Texture2D getControl(_Controls e = (_Controls)0)
		{
			return getControl(null, e);
		}

		public Texture2D getControl(string subfolder = "controls", _Controls e = (_Controls)0)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			subfolder = ((subfolder == null) ? "" : (subfolder + "\\"));
			string text = subfolder;
			int num = (int)e;
			string key = "textures\\controls\\" + text + num + ".png";
			if (!Textures.ContainsKey(key))
			{
				Texture2D texture = ContentsManager.GetTexture(key);
				Textures[key] = new AsyncTexture2D(texture);
				return AsyncTexture2D.op_Implicit(Textures[key]);
			}
			return AsyncTexture2D.op_Implicit(Textures[key]);
		}
	}
}
