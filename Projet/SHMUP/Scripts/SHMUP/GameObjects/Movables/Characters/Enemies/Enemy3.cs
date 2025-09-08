using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies
{
	
	public partial class Enemy3 : Enemy
	{
		private float elapseTime = 0f;
		private float timeImmobile = 3f;

		float destination;
		int pixelOffset = 5;

        public override void _Ready()
		{
			health = 25f;

			base._Ready();

			speed = EnumSpeeds.ENEMY3;
			destination = Position.Y;
            AreaEntered += OnCollision;

            collectibleToSpawn = AllCollectibles.POWER_UP;
        }

        private void OnCollision(Area2D pArea)
        {
			if (!isMoving) return;

			DoDamage(5f);

            if (pArea is Player) DoDamage(health);
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(lDelta);

			DoAction(lDelta);
		}

        protected override void Move(float pDelta)
        {
            base.Move(pDelta);

			elapseTime += pDelta;

			if (elapseTime >= timeImmobile && player.Position.Y != currentShootPos.Position.Y)
			{
                elapseTime = 0f;
				destination = player.Position.Y;
			}
			
			if (GlobalPosition.X * GameManager.parallaxBackground.Scale.X <= screenSize.X - textureSize.X * 1.5f * Scale.X * renderer.Scale.X)
			{
				Position += Vector2.Right * EnumSpeeds.SCROLL_SPEED * pDelta;
			}

			if (!(Position.Y > destination - pixelOffset && Position.Y < destination + pixelOffset))
			{
				if (destination > Position.Y) direction = Vector2.Down;
				else direction = Vector2.Up;
			}
			else
			{
				direction = Vector2.Zero;
                Shoot(EnumSpeeds.BULLET);
            }
        }
    }
}
