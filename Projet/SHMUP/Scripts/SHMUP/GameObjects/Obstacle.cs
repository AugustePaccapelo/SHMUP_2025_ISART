using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Ammos;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters;
using System.Collections.Generic;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects
{
	public partial class Obstacle : GameObject
	{
		[Export] private bool isDestructible = false;
		private float damage = 35f;

        private static List<Obstacle> allObstacle = new List<Obstacle>();

        private Player player;

		private int health = 3;

		private bool doingDamage = false;

		public override void _Ready()
		{
			base._Ready();

            AreaEntered += OnCollision;
            AreaExited += OnAreaExited;

            if (isDestructible)
			{
				renderer.Frame = 1;
			}

			allObstacle.Add(this);
		}

        private void OnCollision(Area2D pArea)
        {
			if (pArea is Player) doingDamage = true;
			
			if (isDestructible)
			{
				health -= 1;

				if (health <= 0) Destroy();
				if (health <= 2) renderer.Frame = 2;
			}
        }

        private void OnAreaExited(Area2D pArea)
        {
			if (pArea is Player) doingDamage = false;
        }

        public override void Destroy()
        {
			//Particles
            base.Destroy();
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

            base._Process(pDelta);

			if (doingDamage)
			{
				if (player is null) player = GameManager.player;
                player.DoDamage(damage * lDelta);
				Vector2 lPlayerDirection = player.direction;
				if (lPlayerDirection.X > 0) lPlayerDirection.X = 0;
				player.direction = lPlayerDirection;
				player.Position += Vector2.Left * lDelta * GameManager.scrollSpeed;
			}
        }

		public static void SetVisibility(bool pIsVisible)
		{
			int lLength = allObstacle.Count;
			Obstacle lObstacle;

			if (pIsVisible)
			{
				for (int i = lLength - 1; i >= 0; i--)
				{
					lObstacle = allObstacle[i];
                    lObstacle.Modulate = Colors.White;
                }
				return;
			}

			for (int i = lLength - 1;i >= 0;i--)
			{
				lObstacle = allObstacle[i];
				if (IsInstanceValid(lObstacle)) lObstacle.Modulate = new Color(1f, 1f, 1f, 0.1f);
            }
        }

        protected override void Dispose(bool pDisposing)
        {
            allObstacle.Remove(this);
            base.Dispose(pDisposing);
        }
    }
}
