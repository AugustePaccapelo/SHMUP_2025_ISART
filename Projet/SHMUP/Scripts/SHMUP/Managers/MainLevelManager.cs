using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName
{

	public partial class MainLevelManager : Node2D
	{

		#region Singleton
		static private MainLevelManager instance;

		private MainLevelManager() { }

		static public MainLevelManager GetInstance()
		{
			if (instance == null) instance = new MainLevelManager();
			return instance;

		}
        #endregion

        private PackedScene gameManagerScene = (PackedScene)GD.Load("res://Scenes/SHMUP/GameManager.tscn");
        private PackedScene hudScene = (PackedScene)GD.Load("res://Scenes/SHMUP/Controls/HUD.tscn");

        public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(MainLevelManager) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

			base._Ready();

            AddChild(gameManagerScene.Instantiate());

            AddChild(hudScene.Instantiate());
        }

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(pDelta);
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
