using Godot;
using System;
using Com.IsartDigital.SHMUP.Enums;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName
{

	public partial class Credits : Control
	{

		#region Singleton
		static private Credits instance;

		private Credits() { }

		static public Credits GetInstance()
		{
			if (instance == null) instance = new Credits();
			return instance;

		}
        #endregion

        [Export] Button quitButton;
		[Export] Label title;

        public static AllLanguages currentLanguage = AllLanguages.ENGLISH;

        public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(Credits) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

			base._Ready();

			quitButton.Pressed += Pressed;
            ChangeLanguage();
        }

        private void Pressed()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.CLICK);
            Hide();
        }

        public void ChangeLanguage()
        {
            switch (currentLanguage)
            {
                case AllLanguages.ENGLISH:
                    title.Text = LanguageEnglish.CREDITS;
                    break;
                case AllLanguages.FRENCH:
                    title.Text = LanguageFrench.CREDITS;
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            instance = null;
            base.Dispose(disposing);
        }
    }
}
