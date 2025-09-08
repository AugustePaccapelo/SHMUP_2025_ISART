using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;
using System.Reflection;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName
{

	public partial class TitleCard : Control
	{
		[Export] private VBoxContainer vButtonContainer;
		[Export] private Button playButton;
        [Export] private Button SettingsButton;
        [Export] private Button CreditsButton;
        [Export] private Button QuitButton;

		[Export] private TextureRect isartLogo;
		[Export] private ColorRect colorRect;
		[Export] private Control settings;
        [Export] private Control credits;

        [Export] private TextureRect helpEnglish;
        [Export] private TextureRect helpFrench;
        [Export] private Button buttonOk;

        private const string CHILD_TO_REMOVE = "TickEvents";

        private float isartLogoTime = 1.5f;

		private PackedScene levelScene = (PackedScene)GD.Load("res://Scenes/SHMUP/MainLevel.tscn");
        private PackedScene soundManagerScene = (PackedScene)GD.Load("res://Scenes/SHMUP/SoundManager.tscn");

        public static AllLanguages currentLanguage = AllLanguages.ENGLISH;

        public override void _Ready()
		{

			base._Ready();

			colorRect.Show();
			isartLogo.Show();

            Tween lTween = CreateTween().Chain();
			lTween.TweenProperty(isartLogo, "modulate", Colors.White, isartLogoTime * 0.5f).From(Colors.Transparent);
            lTween.TweenProperty(isartLogo, "modulate", Colors.Transparent, isartLogoTime * 0.5f);
            lTween.Finished += LTweenFinished;
			lTween.Play();

			CustomMinimumSize = GetWindow().Size;
			vButtonContainer.Position += new Vector2(0, vButtonContainer.Size.Y * 0.5f);
            playButton.Pressed += PlayButtonPressed;
            SettingsButton.Pressed += SettingsButtonPressed;
			CreditsButton.Pressed += CreditsButtonPressed;
            QuitButton.Pressed += QuitButtonPressed;
            buttonOk.Pressed += ButtonOkPressed;

            SoundManager lSoundManager = (SoundManager)soundManagerScene.Instantiate();
            lSoundManager.RemoveChild(lSoundManager.GetNode(CHILD_TO_REMOVE));
            AddChild(lSoundManager);
            lSoundManager.StartUI();
            ChangeLanguage();
		}

        private void ButtonOkPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            GetTree().ChangeSceneToPacked(levelScene);
        }

        public void ChangeLanguage()
        {
            switch (Settings.currentLanguage)
            {
                case AllLanguages.ENGLISH:
                    playButton.Text = LanguageEnglish.PLAY;
                    SettingsButton.Text = LanguageEnglish.SETTINGS;
                    CreditsButton.Text = LanguageEnglish.CREDITS;
                    QuitButton.Text = LanguageEnglish.QUIT;
                    break;
                case AllLanguages.FRENCH:
                    playButton.Text = LanguageFrench.PLAY;
                    SettingsButton.Text = LanguageFrench.SETTINGS;
                    CreditsButton.Text = LanguageFrench.CREDITS;
                    QuitButton.Text = LanguageFrench.QUIT;
                    break;
            }
        }

        private void LTweenFinished()
        {
            isartLogo.Hide();
			colorRect.Hide();
        }

        private void PlayButtonPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            if (currentLanguage == AllLanguages.ENGLISH) helpEnglish.Show();
            else helpFrench.Show();
            buttonOk.Show();
        }

        private void SettingsButtonPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            settings.Show();
        }

        private void CreditsButtonPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            credits.Show();
        }

        private void QuitButtonPressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            GetTree().Quit();
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(pDelta);
		}
	}
}
