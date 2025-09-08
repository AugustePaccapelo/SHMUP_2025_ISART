using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName
{

	public partial class PauseMenu : Control
	{
        #region Singleton
        static private PauseMenu instance;

        private PauseMenu() : base() { }

        static public PauseMenu GetInstance()
        {
            if (instance == null) instance = new PauseMenu();
            return instance;
        }
        #endregion

        [Export] Control settings;

        [Export] VBoxContainer vButtonContainer;
        [Export] Button returnButton;
        [Export] Button settingsButton;
        [Export] Button mainMenuButton;
        [Export] Button quitButton;
        [Export] Label title;

        private PackedScene titleCardScene = (PackedScene)GD.Load("res://Scenes/SHMUP/Controls/TitleCard.tscn");

        public static AllLanguages currentLanguage = AllLanguages.ENGLISH;

        public override void _Ready()
		{
            #region Singleton
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(PauseMenu) + " Instance already exist, destroying the last added.");
                return;

            }
            instance = this;
            #endregion


            base._Ready();

            vButtonContainer.Position += new Vector2(0, vButtonContainer.Size.Y * 0.5f);

            returnButton.Pressed += ReturnButtonPressed;
            settingsButton.Pressed += SettingsButtonPressed; ;
            mainMenuButton.Pressed += MainMenuButtonPressed;
            quitButton.Pressed += QuitButtonPressed;
            ChangeLanguage();
        }

        public void ChangeLanguage()
        {
            switch (currentLanguage)
            {
                case AllLanguages.ENGLISH:
                    title.Text = LanguageEnglish.CREDITS;
                    returnButton.Text = LanguageEnglish.RETURN;
                    settingsButton.Text = LanguageEnglish.SETTINGS;
                    mainMenuButton.Text = LanguageEnglish.MAIN_MENU;
                    quitButton.Text = LanguageEnglish.QUIT;
                    break;
                case AllLanguages.FRENCH:
                    title.Text = LanguageFrench.CREDITS;
                    returnButton.Text = LanguageFrench.RETURN;
                    settingsButton.Text = LanguageFrench.SETTINGS;
                    mainMenuButton.Text = LanguageFrench.MAIN_MENU;
                    quitButton.Text = LanguageFrench.QUIT;
                    break;
            }
        }

        private void ReturnButtonPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            Hide();
            GameManager.GetInstance().RestartGame();
        }

        private void SettingsButtonPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            settings.Show();
        }

        private void MainMenuButtonPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            GetTree().ChangeSceneToPacked(titleCardScene);
        }

        private void QuitButtonPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            GetTree().Quit();
        }

        protected override void Dispose(bool disposing)
        {
            instance = null;
            base.Dispose(disposing);
        }
    }
}
