using Com.IsartDigital.ProjectName;
using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.Enums;
using Com.IsartDigital.SHMUP.GameObjects.Movables;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Ammos;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies;
using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables.Characters
{
	
	public partial class Character : Movable
	{
		[Export] protected float fireRate = 1f;
		[Export] protected float health = 100f;

		protected Node2D shootPos;
		protected List<Marker2D> shootPosMarkers = new List<Marker2D>();

		private const string PATH_SHOOT_POS = "ShootPos";

		protected Marker2D currentShootPos;

		private Bullet lastBullet;
		
        protected bool isAlly;

		public override void _Ready()
		{
            base._Ready();

            if (isAlly)
            {
                CollisionLayer = (int)CollisionLayers.Player;
                CollisionMask = (int)CollisionLayers.BulletEnemy | (int)CollisionLayers.Enemy
                    | (int)CollisionLayers.Obstacle | (int)CollisionLayers.Collectibles;
            }
            else
            {
                CollisionLayer = (int)CollisionLayers.Enemy;
                CollisionMask = (int)CollisionLayers.BulletPlayer | (int)CollisionLayers.Player;
            }

            shootPos = (Node2D)GetNode(PATH_SHOOT_POS);

			GetShootMarkers(shootPos);
        }

		public override void _Process(double pDelta)
		{
            base._Process(pDelta);

            float lDelta = (float)pDelta;
        }

		protected void GetShootMarkers(Node2D pPosContainer)
		{
			shootPosMarkers.Clear();
			currentShootPos = null;
            foreach (Marker2D lMarker in pPosContainer.GetChildren())
            {
                shootPosMarkers.Add(lMarker);
            }
            if (shootPosMarkers.Count > 0) currentShootPos = shootPosMarkers[0];
        }

		private bool CanShoot(float pDistance)
		{
            if (!IsInstanceValid(lastBullet)) return true;
            return (lastBullet.GlobalPosition - currentShootPos.GlobalPosition).Length() > pDistance;
        }

		protected void Shoot(float pAmmoSpeed)
		{
			float lDistance = pAmmoSpeed / (fireRate * shootPosMarkers.Count);
			
            if (currentShootPos != null && CanShoot(lDistance))
			{
                currentShootPos = shootPosMarkers[(shootPosMarkers.IndexOf(currentShootPos) + 1) % shootPosMarkers.Count];
                Vector2 lBulletPosition = GameManager.ammoContainer.ToLocal(currentShootPos.GlobalPosition);
				lastBullet = Bullet.Create(lBulletPosition, isAlly, currentShootPos.GlobalRotation);
				if (isAlly) SoundManager.GetInstance().PlayerShoot();
				else if (this is Boss) SoundManager.GetInstance().SingleSfx(SoundNames.BOSS_SHOOT);
				else SoundManager.GetInstance().SingleSfx(SoundNames.ENEMY_SHOOT);
			}
		}

		public virtual void DoDamage(float pDamage)
		{
			health -= pDamage;
			
			if (health <= 0)
			{
				Destroy();
			}
		}
	}
}
