using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName
{

	public partial class EndScreen : Control
	{

		#region Singleton
		static private EndScreen instance;

		private EndScreen() { }

		static public EndScreen GetInstance()
		{
			if (instance == null) instance = new EndScreen();
			return instance;

		}
		#endregion

		[Export] private Label title;
        [Export] private Button retry;
        [Export] private Button mainMenu;
        [Export] private Button Quit;

		private PackedScene titleCard = (PackedScene)GD.Load("res://Scenes/SHMUP/Controls/TitleCard.tscn");
        public static AllLanguages currentLanguage = AllLanguages.ENGLISH;

        public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(EndScreen) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

			base._Ready();

            retry.Pressed += RetryPressed;
            mainMenu.Pressed += MainMenuPressed;
            Quit.Pressed += QuitPressed;
		}

        public void ChangeLanguage()
        {
            switch (currentLanguage)
            {
                case AllLanguages.ENGLISH:
                    retry.Text = LanguageEnglish.RETRY;
                    mainMenu.Text = LanguageEnglish.MAIN_MENU;
                    Quit.Text = LanguageEnglish.QUIT;
                    break;
                case AllLanguages.FRENCH:
                    retry.Text = LanguageFrench.RETRY;
                    mainMenu.Text = LanguageFrench.MAIN_MENU;
                    Quit.Text = LanguageFrench.QUIT;
                    break;
            }
        }

        public void gameFinished(bool pIsWin)
        {
            Show();
            GameManager.GetInstance().StopGame();
            switch (currentLanguage)
            {
                case AllLanguages.ENGLISH:
                    if (pIsWin) title.Text = LanguageEnglish.GAME_OVER_WIN;
                    else title.Text = LanguageEnglish.GAME_OVER_LOST;
                    break;
                case AllLanguages.FRENCH:
                    if (pIsWin) title.Text = LanguageFrench.GAME_OVER_WIN;
                    else title.Text = LanguageFrench.GAME_OVER_LOST;
                    break;
            }
        }

        private void QuitPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            GetTree().Quit();
        }

        private void MainMenuPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            GetTree().ChangeSceneToPacked(titleCard);
        }

        private void RetryPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            GetTree().ReloadCurrentScene();
        }

        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
