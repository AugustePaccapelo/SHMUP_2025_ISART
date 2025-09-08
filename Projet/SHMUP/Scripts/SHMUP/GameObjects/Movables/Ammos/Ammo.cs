using Com.IsartDigital.SHMUP.Enums;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables.Ammos
{
	
	public partial class Ammo : Movable
	{
		public bool isAlly;
		private float visibleDistance = 750f;

		public override void _Ready()
		{
			base._Ready();
			if (isAlly)
			{
				direction = Vector2.Right;
				CollisionLayer = (int)CollisionLayers.BulletPlayer;
				CollisionMask = (int)CollisionLayers.BulletEnemy | (int)CollisionLayers.Enemy 
					| (int)CollisionLayers.Obstacle;
			}
			else
			{
                direction = Vector2.Left;
                CollisionLayer = (int)CollisionLayers.BulletEnemy;
				CollisionMask = (int)CollisionLayers.BulletPlayer | (int)CollisionLayers.Player;
				Hide();
            }
            AreaEntered += OnCollision;
		}

        private void OnCollision(Area2D pArea)
        {
            if (pArea is Player)
            {
                Player.GetInstance().DoDamage(5f);
            }
            QueueFree();
        }

        public override void _Process(double pDelta)
		{
			base._Process(pDelta);

			float lDelta = (float)pDelta;

			DoAction(lDelta);

			Position += Vector2.Right * EnumSpeeds.SCROLL_SPEED * lDelta;

			if (!Visible && (Player.GetInstance().GlobalPosition - GlobalPosition).Length() < visibleDistance) Show();
		}
	}
}
