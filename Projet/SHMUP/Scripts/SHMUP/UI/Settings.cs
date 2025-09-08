using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName
{

	public partial class Settings : Control
	{

		#region Singleton
		static private Settings instance;

		private Settings() { }

		static public Settings GetInstance()
		{
			if (instance == null) instance = new Settings();
			return instance;

		}
		#endregion

		[Export] Button quitButton;
		[Export] Button frenchLanguage;
        [Export] Button englishLanguage;
		[Export] HSlider soundSlider;
		[Export] Label title;

		public static AllLanguages currentLanguage = AllLanguages.ENGLISH;

        public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(Settings) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

			base._Ready();

			quitButton.Pressed += Pressed;
            soundSlider.ValueChanged += SoundSliderValueChanged;
            frenchLanguage.Pressed += FrenchLanguagePressed;
            englishLanguage.Pressed += EnglishLanguagePressed;
			ChangeLanguage();
        }

        private void Pressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            Hide();
        }

        private void EnglishLanguagePressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            PauseMenu.currentLanguage = Credits.currentLanguage = TitleCard.currentLanguage = currentLanguage = AllLanguages.ENGLISH;
            ChangeAllLanguges();
        }

        private void FrenchLanguagePressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            PauseMenu.currentLanguage = Credits.currentLanguage = TitleCard.currentLanguage = currentLanguage = AllLanguages.FRENCH;
            ChangeAllLanguges();
        }

        private void ChangeAllLanguges()
        {
            ChangeLanguage();

            if (GetParent() is TitleCard)
            {
                ((TitleCard)GetParent()).ChangeLanguage();
                Credits.GetInstance().ChangeLanguage();
            }
            else
            {
                PauseMenu.GetInstance().ChangeLanguage();
                EndScreen.GetInstance().ChangeLanguage();
            }
        }

        public void ChangeLanguage()
        {
            switch (currentLanguage)
            {
                case AllLanguages.ENGLISH:
                    title.Text = LanguageEnglish.SETTINGS;
                    break;
                case AllLanguages.FRENCH:
                    title.Text = LanguageFrench.SETTINGS;
                    break;
            }
        }

        private void SoundSliderValueChanged(double value)
        {
            AudioServer.SetBusVolumeDb(0, (float)value);
        }

        protected override void Dispose(bool disposing)
        {
            instance = null;
            base.Dispose(disposing);
        }
    }
}
