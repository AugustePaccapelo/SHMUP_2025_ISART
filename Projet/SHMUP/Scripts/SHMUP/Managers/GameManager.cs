using Com.IsartDigital.ProjectName;
using Com.IsartDigital.SHMUP.Enums;
using Com.IsartDigital.SHMUP.GameObjects;
using Com.IsartDigital.SHMUP.GameObjects.Collectibles;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies;
using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP
{

	public partial class GameManager : Node2D
	{
		#region Singleton
		static private GameManager instance;

		private GameManager() : base() { }

		static public GameManager GetInstance()
		{
			if (instance == null) instance = new GameManager();
			return instance;
		}
        #endregion

        private PackedScene soundManagerScene = (PackedScene)GD.Load("res://Scenes/SHMUP/SoundManager.tscn");
		private PackedScene particulesSmokeScene = (PackedScene)GD.Load("res://Scenes/SHMUP/Particules/ExplosionSmoke.tscn");
        private PackedScene particulesDebritsScene = (PackedScene)GD.Load("res://Scenes/SHMUP/Particules/ExplosionDebrits.tscn");

        private static Node2D main;
		public static ParallaxBackground parallaxBackground;
		public static Node2D gameContainer;
		public static Node2D ammoContainer;
        public static Node2D obstaclesContainer;
        public static Node2D enemiesContainer;
        public static Node2D collectiblesContainer;
        public static Node2D bossPositionsContainer;

        private const string PATH_MAIN = "/root/Main";
		private const string PATH_PARALLAX_BACKGROUND = "ParallaxBackground";
        private const string PATH_GAME_CONTAINER = "GameLayer/GameContainer";
        private const string PATH_AMMO_CONTAINER = "AmmoContainer";
        private const string PATH_OBSTACLES_CONTAINER = "ObstaclesContainer";
        private const string PATH_ENEMIES_CONTAINER = "EnemiesContainer";
        private const string PATH_COLLECTIBLES_CONTAINER = "CollectiblesContainer";
        private const string PATH_BOSS_POSITIONS_CONTAINER = "BossPositions";

        private List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();
		private List<float> parallaxSpeeds = new List<float>() { 0.25f, 0.5f, 0.75f, 1f, 1.5f };
		public static float scrollSpeed;
		public static Vector2 screenSize;
		public static Player player;

		private List<Enemy> activeEnemies = new List<Enemy>();

		private float distanceActivation = 100f;

		private GameLayers currentGameLayer = GameLayers.ENEMIES;

		private Vector2 defaultResolution = new Vector2(1280, 720);

		public bool isGameRunning = true;
		Signals signals;

        public override void _Ready()
		{
			#region Singleton
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(GameManager) + " Instance already exist, destroying the last added.");
				return;

			}
			instance = this;
			#endregion

			screenSize = GetWindow().Size;

            scrollSpeed = EnumSpeeds.SCROLL_SPEED;

            main = (Node2D)GetNode(PATH_MAIN);
			parallaxBackground = (ParallaxBackground)main.GetNode(PATH_PARALLAX_BACKGROUND);

			parallaxBackground.Scale = screenSize / defaultResolution;

			gameContainer = (Node2D)parallaxBackground.GetNode(PATH_GAME_CONTAINER);
			ammoContainer = (Node2D)gameContainer.GetNode(PATH_AMMO_CONTAINER);
            collectiblesContainer = (Node2D)gameContainer.GetNode(PATH_COLLECTIBLES_CONTAINER);
            enemiesContainer = (Node2D)gameContainer.GetNode(PATH_ENEMIES_CONTAINER);
            obstaclesContainer = (Node2D)gameContainer.GetNode(PATH_OBSTACLES_CONTAINER);
            bossPositionsContainer = (Node2D)enemiesContainer.GetNode(PATH_BOSS_POSITIONS_CONTAINER);

            player = Player.Create(gameContainer);

			int lLength = parallaxBackground.GetChildCount();
			for (int i = 0; i < lLength; i++) parallaxLayers.Add((ParallaxLayer)parallaxBackground.GetChild(i));

			signals = Signals.GetInstance();
            signals.EnemyDeath += SpawnExplosio;
			GetParent().AddChild(soundManagerScene.Instantiate());
			SoundManager.GetInstance().StartGame();
		}

        private void SpawnExplosio(Vector2 pPosition)
        {
            GpuParticles2D lParticule = (GpuParticles2D)particulesSmokeScene.Instantiate();
			lParticule.Position = pPosition;
			lParticule.Finished += lParticule.QueueFree;
			lParticule.Emitting = true;
			gameContainer.AddChild(lParticule);

			lParticule = (GpuParticles2D)particulesDebritsScene.Instantiate();
			lParticule.Position = pPosition;
			lParticule.Finished += lParticule.QueueFree;
			lParticule.Emitting = true;
            gameContainer.AddChild(lParticule);
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			MoveLayers(lDelta);
			SetEnemies();
        }

        private void MoveLayers(float pDelta)
		{
			int lLength = parallaxLayers.Count;
			for (int i = 0; i < lLength; i++) parallaxLayers[i].MotionOffset += Vector2.Left * scrollSpeed * parallaxSpeeds[i] * pDelta;	
		}

		public void StopGame()
		{
			scrollSpeed = 0;
			isGameRunning = false;
			signals.EmitSignal(nameof(signals.ChangeRunningState), isGameRunning);
			Player.GetInstance().Monitorable = false;
		}

		public void RestartGame()
		{
			scrollSpeed = EnumSpeeds.SCROLL_SPEED;
			isGameRunning = true;
            signals.EmitSignal(nameof(signals.ChangeRunningState), isGameRunning);
            Player.GetInstance().Monitorable = true;
        }

		private void SetEnemies()
		{
			Enemy lEnemy;
			for (int i = Enemy.allEnemies.Count - 1; i > -1; i--)
			{
				lEnemy = Enemy.allEnemies[i];
				if (IsInstanceValid(lEnemy) && lEnemy.GlobalPosition.X * GameManager.parallaxBackground.Scale.X <= -lEnemy.textureSize.X)
				{
					if (lEnemy is Boss) continue;
					activeEnemies.Remove(lEnemy);
					lEnemy.Destroy();
					continue;
				}
				if (IsInstanceValid(lEnemy) && lEnemy.GlobalPosition.X * GameManager.parallaxBackground.Scale.X <= lEnemy.textureSize.X + screenSize.X + distanceActivation)
				{
					activeEnemies.Add(lEnemy);
                    if (lEnemy is Boss && !lEnemy.isMoving) ((Boss)lEnemy).SetActive();
                    lEnemy.isMoving = true;
					continue;
				}
			}
		}

		public void ChangeLayer(int pLayer)
		{
			currentGameLayer = (GameLayers)pLayer;

			if (currentGameLayer == GameLayers.ENEMIES) enemiesContainer.Modulate = Colors.White;
			else enemiesContainer.Modulate = new Color(1f, 1f, 1f, 0.1f);

			if (currentGameLayer == GameLayers.COLLECTIBLES) collectiblesContainer.Modulate = Colors.White;
			else collectiblesContainer.Modulate = new Color(1f, 1f, 1f, 0.1f);
            
			Obstacle.SetVisibility(currentGameLayer == GameLayers.OBSTACLES);
        }

        protected override void Dispose(bool disposing)
        {
			instance = null;
            base.Dispose(disposing);
        }
    }
}
