using Com.IsartDigital.SHMUP;
using Godot;
using System;
using System.Diagnostics;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects
{
	
	public partial class GameObject : Area2D
	{
		protected AnimatedSprite2D renderer;
		protected Vector2 screenSize;

        private const string PATH_RENDERER = "Renderer";
        private const string DEFAULT_STATE = "default";

        public Vector2 textureSize
		{
			get { return renderer.SpriteFrames.GetFrameTexture(DEFAULT_STATE, renderer.Frame).GetSize(); }
			protected set { textureSize = value; }
		}

		public override void _Ready()
		{
			renderer = FindChild(PATH_RENDERER) as AnimatedSprite2D;
			screenSize = GetWindow().Size;
        }

		public virtual void Destroy()
		{
			QueueFree();
		}
	}
}
