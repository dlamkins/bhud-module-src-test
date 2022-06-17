using System.ComponentModel;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Pathing.Entities
{
	public abstract class Entity : INotifyPropertyChanged, IEntity, IUpdatable, IRenderable3D
	{
		protected Vector3 _position = Vector3.get_Zero();

		protected Vector3 _rotation = Vector3.get_Zero();

		protected Vector3 _renderOffset = Vector3.get_Zero();

		protected float _opacity = 1f;

		protected bool _visible = true;

		private bool _pendingRebuild = true;

		private EntityBillboard _billboard;

		private EntityText _basicTitleTextBillboard;

		protected static BasicEffect StandardEffect { get; }

		public virtual Vector3 Position
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _position;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(ref _position, value, rebuildEntity: false, "Position");
			}
		}

		public virtual Vector3 Rotation
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _rotation;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(ref _rotation, value, rebuildEntity: false, "Rotation");
			}
		}

		public float RotationX
		{
			get
			{
				return _rotation.X;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _rotation, new Vector3(value, _rotation.Y, _rotation.Z), rebuildEntity: false, "Rotation"))
				{
					OnPropertyChanged("RotationX");
				}
			}
		}

		public float RotationY
		{
			get
			{
				return _rotation.Y;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _rotation, new Vector3(_rotation.X, value, _rotation.Z), rebuildEntity: false, "Rotation"))
				{
					OnPropertyChanged("RotationY");
				}
			}
		}

		public float RotationZ
		{
			get
			{
				return _rotation.Z;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _rotation, new Vector3(_rotation.X, _rotation.Y, value), rebuildEntity: false, "Rotation"))
				{
					OnPropertyChanged("RotationZ");
				}
			}
		}

		public bool PendingRebuild => _pendingRebuild;

		public virtual Vector3 RenderOffset
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _renderOffset;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(ref _renderOffset, value, rebuildEntity: false, "RenderOffset");
			}
		}

		public virtual float VerticalOffset
		{
			get
			{
				return _renderOffset.Z;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _renderOffset, new Vector3(_renderOffset.X, _renderOffset.Y, value), rebuildEntity: false, "RenderOffset"))
				{
					OnPropertyChanged("VerticalOffset");
				}
			}
		}

		public virtual float HorizontalOffset
		{
			get
			{
				return _renderOffset.X;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _renderOffset, new Vector3(value, _renderOffset.Y, _renderOffset.Z), rebuildEntity: false, "RenderOffset"))
				{
					OnPropertyChanged("HorizontalOffset");
				}
			}
		}

		public virtual float DepthOffset
		{
			get
			{
				return _renderOffset.Y;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				if (SetProperty(ref _renderOffset, new Vector3(_renderOffset.X, value, _renderOffset.Y), rebuildEntity: false, "RenderOffset"))
				{
					OnPropertyChanged("DepthOffset");
				}
			}
		}

		public virtual float Opacity
		{
			get
			{
				return _opacity;
			}
			set
			{
				SetProperty(ref _opacity, value, rebuildEntity: false, "Opacity");
			}
		}

		public virtual bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				SetProperty(ref _visible, value, rebuildEntity: false, "Visible");
			}
		}

		public EntityBillboard Billboard
		{
			get
			{
				return _billboard ?? _basicTitleTextBillboard;
			}
			set
			{
				SetProperty(ref _billboard, value, rebuildEntity: false, "Billboard");
			}
		}

		public string BasicTitleText
		{
			get
			{
				return _basicTitleTextBillboard?.Text ?? string.Empty;
			}
			set
			{
				if (_basicTitleTextBillboard == null)
				{
					_basicTitleTextBillboard = BuildTitleText();
				}
				_basicTitleTextBillboard.Text = value;
			}
		}

		public Color BasicTitleTextColor
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				return _basicTitleTextBillboard?.TextColor ?? Color.get_White();
			}
			set
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				if (_basicTitleTextBillboard == null)
				{
					_basicTitleTextBillboard = BuildTitleText();
				}
				_basicTitleTextBillboard.TextColor = value;
			}
		}

		public virtual float DistanceFromPlayer => Vector3.Distance(Position, GameService.Gw2Mumble.PlayerCharacter.Position);

		public virtual float DistanceFromCamera => Vector3.Distance(Position, GameService.Gw2Mumble.PlayerCamera.Position);

		public float DrawOrder => Vector3.DistanceSquared(Position, GameService.Gw2Mumble.PlayerCamera.Position);

		public event PropertyChangedEventHandler PropertyChanged;

		private EntityText BuildTitleText()
		{
			return new EntityText(this)
			{
				VerticalOffset = 2f
			};
		}

		public virtual void DoUpdate(GameTime gameTime)
		{
			if (_pendingRebuild)
			{
				HandleRebuild(GameService.Graphics.GraphicsDevice);
				_pendingRebuild = false;
			}
			Update(gameTime);
			Billboard?.DoUpdate(gameTime);
		}

		public virtual void DoDraw(GraphicsDevice graphicsDevice)
		{
			Draw(graphicsDevice);
			Billboard?.DoDraw(graphicsDevice);
		}

		public void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			DoDraw(graphicsDevice);
		}

		public abstract void HandleRebuild(GraphicsDevice graphicsDevice);

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(GraphicsDevice graphicsDevice)
		{
		}

		protected bool SetProperty<T>(ref T property, T newValue, bool rebuildEntity = false, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(property, newValue) || propertyName == null)
			{
				return false;
			}
			property = newValue;
			_pendingRebuild |= rebuildEntity;
			OnPropertyChanged(propertyName);
			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		static Entity()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			BasicEffect val = new BasicEffect(GameService.Graphics.GraphicsDevice);
			val.set_TextureEnabled(true);
			StandardEffect = val;
		}
	}
}
