using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Com.IsartDigital.SHMUP.GameObjects.Collectibles;
using Com.IsartDigital.SHMUP.Enums;
using Com.IsartDigital.ProjectName;


// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies
{
	
	public partial class Enemy : Character
	{
		public static List<Enemy> allEnemies = new List<Enemy>();

        public bool isMoving = false;

		protected Player player;

		protected string collectibleToSpawn = AllCollectibles.NONE;

        public override void _Ready()
		{
            allEnemies.Add(this);

            isAlly = false;

			base._Ready();
        }

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);

			float lDelta = (float)pDelta;
		}

        protected override void DoAction(float pDelta)
        {
            if (player == null) player = Player.GetInstance();
            if (player == null) return;

            if (isMoving) Move(pDelta);

            base.DoAction(pDelta);
        }

		protected virtual void Move(float pDelta)
		{
			
		}

        public override void Destroy()
		{
			allEnemies.Remove(this);
			if (this is Enemy1) SoundManager.GetInstance().Enemy0Explosion();
			else SoundManager.GetInstance().SingleSfx(SoundNames.ENEMY_EXPLOSION);
			Signals lSignals = Signals.GetInstance();
            lSignals.EmitSignal(nameof(lSignals.EnemyDeath), Position);
			HUD.GetInstance().AddScore(500);
            base.Destroy();
		}

        protected override void Dispose(bool disposing)
        {
			if (collectibleToSpawn != AllCollectibles.NONE)
			{
				Collectible.Create(collectibleToSpawn, Position.X, Position.Y);
			}

            base.Dispose(disposing);
        }
    }
}
