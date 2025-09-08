using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.GameObjects.Collectibles;
using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName {
	
	public partial class LDTestManager : Node2D
	{
		private int numEachCollectibles = 50;
		private float spaceBetweenCollectibles = 50f;

        private PackedScene gameManagerScene = (PackedScene)GD.Load("res://Scenes/SHMUP/GameManager.tscn");
        private PackedScene hudScene = (PackedScene)GD.Load("res://Scenes/SHMUP/Controls/HUD.tscn");

        public override void _Ready()
		{
            base._Ready();

            AddChild(gameManagerScene.Instantiate());

			AddChild(hudScene.Instantiate());

            PlaceAllColelctibles();
			HUD.GetInstance().retryButton.Show();
		}

		private void PlaceAllColelctibles()
		{
			Collectible lCollectible;
			string lNextCollectibleType;
			int lLength = numEachCollectibles * 2;

			for (int i = 0; i < lLength; i++)
			{
				lNextCollectibleType = i % 2 == 0 ? AllCollectibles.HEALTH : AllCollectibles.SMART_BOMB;
				lCollectible = Collectible.Create(lNextCollectibleType);
				lCollectible.Position = (new Vector2(0, lCollectible.textureSize.Y * 1.5f) 
					+ new Vector2(lCollectible.textureSize.X * 1.5f, 0) * (i + 1)
					+ new Vector2(spaceBetweenCollectibles, 0) * i) / GameManager.parallaxBackground.Scale;
			}
			lNextCollectibleType = AllCollectibles.POWER_UP;
			for (int i = 0; i < numEachCollectibles;i++)
			{
				lCollectible = Collectible.Create(lNextCollectibleType);
				lCollectible.Position = (new Vector2(0, GameManager.screenSize.Y - lCollectible.textureSize.Y * 1.5f)
					+ new Vector2(lCollectible.textureSize.X, 0) * (i + 1)
					+ new Vector2(spaceBetweenCollectibles, 0) * i) / GameManager.parallaxBackground.Scale;
			}
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(pDelta);
		}

		protected override void Dispose(bool pDisposing)
		{
			base.Dispose(pDisposing);
		}
	}
}
