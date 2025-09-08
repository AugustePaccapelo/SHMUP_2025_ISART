using Com.IsartDigital.ProjectName;
using Com.IsartDigital.SHMUP.Enums;
using Com.IsartDigital.SHMUP.GameObjects;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Collectibles
{
	public partial class Collectible : GameObject
	{
		public Vector2 position;

		public override void _Ready()
		{
			base._Ready();

			CollisionLayer = (int)CollisionLayers.Collectibles;
			CollisionMask = (int)CollisionLayers.Player;

            AreaEntered += OnCollision;

			Position = position;
		}

        protected virtual void OnCollision(Area2D pArea)
        {
			QueueFree();
        }

		public override void Destroy()
		{
			QueueFree();
		}

		public static Collectible Create(string pCollectibleType, float pXPos = 0, float pYPos = 0)
		{
			Collectible lCollectible = null;
			switch (pCollectibleType)
			{
				case AllCollectibles.HEALTH:
					lCollectible = (Collectible)Heal.packedScene.Instantiate();
					break;
				case AllCollectibles.POWER_UP:
					lCollectible = (Collectible)PowerUp.packedScene.Instantiate();
					break;
				case AllCollectibles.SMART_BOMB:
					lCollectible = (Collectible)SmartBomb.packedScene.Instantiate();
					break;
				default:
					break;
			}

			if (lCollectible != null)
			{
				GameManager.collectiblesContainer.AddChild(lCollectible);
				lCollectible.Position = new Vector2(pXPos, pYPos);
			}

            return lCollectible;
		}
	}
}
