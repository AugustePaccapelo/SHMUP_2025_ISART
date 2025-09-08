using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies
{
	
	public partial class Enemy1 : Enemy
	{
        [Export] private float sinSize = 500f;
        [Export] private float frequence = 7.5f;
        [Export] private float damages = 5f;

        private static PackedScene enemy1Scene = (PackedScene)GD.Load("res://Scenes/SHMUP/GameObjects/Movables/Characters/Enemies/Enemy1.tscn");

        private float elapseTime = 0f;

        public override void _Ready()
		{
			health = 1f;

			base._Ready();

			speed = EnumSpeeds.ENEMY1;
            AreaEntered += OnCollision;
		}

        private void OnCollision(Area2D pArea)
        {
            if (isMoving) DoDamage(health);
            if (pArea is Player) Player.GetInstance().DoDamage(damages);
        }

        public override void _Process(double pDelta)
		{
			base._Process(pDelta);
            
            float lDelta = (float)pDelta;
            
            DoAction(lDelta);
		}

        protected override void Move(float pDelta)
        {
            base.Move(pDelta);

            direction = (player.Position - Position).Normalized();

            float lOscillation = Mathf.Sin(elapseTime * frequence) * sinSize;
            Position += Vector2.Up * lOscillation * pDelta;
            elapseTime += pDelta;
        }

        public static Enemy1 Create(Vector2 pPosition, float pDirection)
        {
            Enemy1 lEnemy = (Enemy1)enemy1Scene.Instantiate();
            lEnemy.sinSize *= pDirection;
            GameManager.enemiesContainer.AddChild(lEnemy);
            lEnemy.Position = pPosition;
            return lEnemy;
        }
    }
}
