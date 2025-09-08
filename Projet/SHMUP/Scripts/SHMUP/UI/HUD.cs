using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName
{

	public partial class HUD : Control
	{
        #region Singleton
        static private HUD instance;

        private HUD() : base() { }

        static public HUD GetInstance()
        {
            if (instance == null) instance = new HUD();
            return instance;
        }
        #endregion

        [Export] public ProgressBar healthBar;
		[Export] private Label scoreLabel;
		[Export] public HBoxContainer smartBombContainer;
		[Export] public Label godModLabel;
		[Export] public Button retryButton;

        private int score = 0;
		private const string SCORE_TEXT = "Score : ";

		private int numSmartBomb = 0;

        Vector2 scoreBaseScale;
        public override void _Ready()
		{
            #region Singleton
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(HUD) + " Instance already exist, destroying the last added.");
                return;

            }
            instance = this;
            #endregion

            base._Ready();

			CustomMinimumSize = GetWindow().Size;
			AnchorsPreset = (int)Control.LayoutPreset.TopLeft;

			foreach (TextureRect lSmartBomb in smartBombContainer.GetChildren()) lSmartBomb.Hide();

            UpdateSmartBomb();
            retryButton.Pressed += RestartLdTest;
            godModLabel.Hide();
            scoreBaseScale = scoreLabel.Scale;
        }

        private void RestartLdTest()
        {
            GetTree().ReloadCurrentScene();
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(pDelta);
		}

		public void UpdateHealth(float pHealth)
		{
			healthBar.Value = pHealth;
		}

		public void AddScore(int pScore)
		{
			score += pScore;
			scoreLabel.Text = SCORE_TEXT + score.ToString();
            Tween lTween = CreateTween();
            lTween.TweenProperty(scoreLabel, "scale", scoreBaseScale * 1.5f, 0.5f);
            lTween.Chain().TweenProperty(scoreLabel, "scale", scoreBaseScale, 0.75f);
        }

		public void UpdateSmartBomb()
		{
			numSmartBomb = Player.GetInstance().smartBombNumber;
			int lLength = smartBombContainer.GetChildCount();
			for (int i = 0; i < lLength; i++)
			{
				if (i < numSmartBomb) ((TextureRect)smartBombContainer.GetChild(i)).Show();
				else ((TextureRect)smartBombContainer.GetChild(i)).Hide();
            }
		}

        protected override void Dispose(bool disposing)
        {
            instance = null;
            base.Dispose(disposing);
        }
    }
}
