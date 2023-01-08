using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class TextureManager : IDisposable
	{
		private bool disposed;

		public _UpgradeIDs _UpgradeIDs;

		public List<Texture2D> _Backgrounds = new List<Texture2D>();

		public List<Texture2D> _Icons = new List<Texture2D>();

		public List<Texture2D> _Emblems = new List<Texture2D>();

		public List<Texture2D> _Controls = new List<Texture2D>();

		public List<Texture2D> _EquipmentTextures = new List<Texture2D>();

		public List<Texture2D> _Stats = new List<Texture2D>();

		public List<Texture2D> _StatIcons = new List<Texture2D>();

		public List<Texture2D> _EquipSlotTextures = new List<Texture2D>();

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				((IEnumerable<IDisposable>)_Backgrounds)?.DisposeAll();
				((IEnumerable<IDisposable>)_Icons)?.DisposeAll();
				((IEnumerable<IDisposable>)_Emblems)?.DisposeAll();
				((IEnumerable<IDisposable>)_Controls)?.DisposeAll();
				((IEnumerable<IDisposable>)_EquipmentTextures)?.DisposeAll();
				((IEnumerable<IDisposable>)_Stats)?.DisposeAll();
				((IEnumerable<IDisposable>)_StatIcons)?.DisposeAll();
				((IEnumerable<IDisposable>)_EquipSlotTextures)?.DisposeAll();
			}
		}

		public TextureManager()
		{
			ContentsManager ContentsManager = BuildsManager.ModuleInstance.ContentsManager;
			Array values = Enum.GetValues(typeof(_Backgrounds));
			_Backgrounds = new List<Texture2D>((IEnumerable<Texture2D>)(object)new Texture2D[values.Cast<int>().Max() + 1]);
			foreach (_Backgrounds num4 in values)
			{
				int num8 = (int)num4;
				Texture2D texture6 = ContentsManager.GetTexture("textures\\backgrounds\\" + num8 + ".png");
				_Backgrounds.Insert((int)num4, texture6);
			}
			values = Enum.GetValues(typeof(_Icons));
			_Icons = new List<Texture2D>((IEnumerable<Texture2D>)(object)new Texture2D[values.Cast<int>().Max() + 1]);
			foreach (_Icons num7 in values)
			{
				int num8 = (int)num7;
				Texture2D texture7 = ContentsManager.GetTexture("textures\\icons\\" + num8 + ".png");
				_Icons.Insert((int)num7, texture7);
			}
			values = Enum.GetValues(typeof(_Emblems));
			_Emblems = new List<Texture2D>((IEnumerable<Texture2D>)(object)new Texture2D[values.Cast<int>().Max() + 1]);
			foreach (_Emblems num6 in values)
			{
				int num8 = (int)num6;
				Texture2D texture5 = ContentsManager.GetTexture("textures\\emblems\\" + num8 + ".png");
				_Emblems.Insert((int)num6, texture5);
			}
			values = Enum.GetValues(typeof(_Controls));
			_Controls = new List<Texture2D>((IEnumerable<Texture2D>)(object)new Texture2D[values.Cast<int>().Max() + 1]);
			foreach (_Controls num5 in values)
			{
				int num8 = (int)num5;
				Texture2D texture4 = ContentsManager.GetTexture("textures\\controls\\" + num8 + ".png");
				_Controls.Insert((int)num5, texture4);
			}
			values = Enum.GetValues(typeof(_EquipmentTextures));
			_EquipmentTextures = new List<Texture2D>((IEnumerable<Texture2D>)(object)new Texture2D[values.Cast<int>().Max() + 1]);
			foreach (_EquipmentTextures num3 in values)
			{
				int num8 = (int)num3;
				Texture2D texture3 = ContentsManager.GetTexture("textures\\equipment slots\\" + num8 + ".png");
				_EquipmentTextures.Insert((int)num3, texture3);
			}
			values = Enum.GetValues(typeof(_EquipSlotTextures));
			_EquipSlotTextures = new List<Texture2D>((IEnumerable<Texture2D>)(object)new Texture2D[values.Cast<int>().Max() + 1]);
			foreach (_EquipSlotTextures num2 in values)
			{
				int num8 = (int)num2;
				Texture2D texture2 = Texture2DExtension.GetRegion(ContentsManager.GetTexture("textures\\equipment slots\\" + num8 + ".png"), 37, 37, 54, 54);
				_EquipSlotTextures.Insert((int)num2, texture2);
			}
			values = Enum.GetValues(typeof(_Stats));
			_Stats = new List<Texture2D>((IEnumerable<Texture2D>)(object)new Texture2D[values.Cast<int>().Max() + 1]);
			foreach (_Stats num in values)
			{
				int num8 = (int)num;
				Texture2D texture = ContentsManager.GetTexture("textures\\stats\\" + num8 + ".png");
				_Stats.Insert((int)num, texture);
			}
			BuildsManager.ModuleInstance.LoadingTexture = getIcon(Kenedia.Modules.BuildsManager._Icons.SingleSpinner);
		}

		public Texture2D getBackground(_Backgrounds background)
		{
			if ((int)background < _Backgrounds.Count && _Backgrounds[(int)background] != null)
			{
				return _Backgrounds[(int)background];
			}
			return _Icons[0];
		}

		public Texture2D getIcon(_Icons icon)
		{
			if ((int)icon < _Icons.Count && _Icons[(int)icon] != null)
			{
				return _Icons[(int)icon];
			}
			return _Icons[0];
		}

		public Texture2D getEmblem(_Emblems emblem)
		{
			if ((int)emblem < _Emblems.Count && _Emblems[(int)emblem] != null)
			{
				return _Emblems[(int)emblem];
			}
			return _Icons[0];
		}

		public Texture2D getEquipTexture(_EquipmentTextures equipment)
		{
			if ((int)equipment < _EquipmentTextures.Count && _EquipmentTextures[(int)equipment] != null)
			{
				return _EquipmentTextures[(int)equipment];
			}
			return _Icons[0];
		}

		public Texture2D getStatTexture(_Stats stat)
		{
			if ((int)stat < _Stats.Count && _Stats[(int)stat] != null)
			{
				return _Stats[(int)stat];
			}
			return _Icons[0];
		}

		public Texture2D getControlTexture(_Controls control)
		{
			if ((int)control < _Controls.Count && _Controls[(int)control] != null)
			{
				return _Controls[(int)control];
			}
			return _Icons[0];
		}
	}
}