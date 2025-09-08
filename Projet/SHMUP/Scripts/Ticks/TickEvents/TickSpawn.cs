using Godot;
using System;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName {
	
	public partial class TickSpawn : TickEvent
	{
		private int numOfEnemySpawned = 3;
        private float enemiesSpace = 100f;

        protected override void OnBeat()
        {
            base.OnBeat();

            Vector2 lPosition = new Vector2(GetWindow().Size.X + boss.Position.X, boss.Position.Y);
            float lSpace = (numOfEnemySpawned - 1) * enemiesSpace;
            float lFirstY = lPosition.Y - lSpace * 0.5f;

            for (int i = 0; i < numOfEnemySpawned; i++)
            {
                lPosition.Y = lFirstY - lSpace * i;
                Enemy1.Create(lPosition, i % 2 == 0 ? 1: -1);
            }
        }
    }
}
