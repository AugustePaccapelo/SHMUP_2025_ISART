using Com.IsartDigital.ProjectName;
using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies
{
	
	public partial class Enemy2 : Enemy
	{
		ShapeCast2D shapeCast;

		private const string PATH_SHAPE_CAST = "ShapeCast2D";

        [Export] private float sinSize = 150f;
        [Export] private float frequence = 7.5f;

        private float elapseTime = 0f;

        public override void _Ready()
		{
			health = 15f;

			base._Ready();

			shapeCast = (ShapeCast2D)GetNode(PATH_SHAPE_CAST);

            speed = EnumSpeeds.ENEMY2;
            AreaEntered += OnCollision;

            collectibleToSpawn = AllCollectibles.SMART_BOMB;
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
			
			if (shapeCast.IsColliding())
			{
				Node2D collider = (Node2D)shapeCast.GetCollider(0);
				if (collider is Player)
				{
					Shoot(EnumSpeeds.BULLET);
				}
			}

			DoAction(lDelta);
		}

        protected override void Move(float pDelta)
        {
            base.Move(pDelta);

            float lOscillation = Mathf.Sin((elapseTime * frequence) * 0.5f) * sinSize;
            Position += Vector2.Up * lOscillation * pDelta;
            elapseTime += pDelta;
        }
    }
}
