using Godot;
using System;
using Com.IsartDigital.SHMUP.Enums;
using System.Collections.Generic;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName
{

	public partial class SoundManager : Node
	{

		#region Singleton
		static private SoundManager instance;

		private SoundManager() { }

		static public SoundManager GetInstance()
		{
			if (instance == null) instance = new SoundManager();
			return instance;

		}
		#endregion

        [Export] private AudioStreamPlayer TickMove;
        [Export] private AudioStreamPlayer TickShoot;
        [Export] private AudioStreamPlayer TickRotate;
        [Export] private AudioStreamPlayer TickSpawn;
        [Export] private AudioStreamPlayer TickNextPhase;

		[Export] private Node sfxContainer;

		private List<AudioStreamPlayer> sfxPlayerShoot = new List<AudioStreamPlayer>();
		private int indexCurrentSfxPlayerShoot = 0;

        private List<AudioStreamPlayer> sfxEnemy0Explosion = new List<AudioStreamPlayer>();
        private int indexCurrentSfxenemy0Explosion = 0;

        private List<AudioStreamPlayer> sfxSF = new List<AudioStreamPlayer>();
        private int indexCurrentSfxSF = 0;

        private const string PATH_SOUNDS = "res://Audio/SFX/";
		private const string EXTENSION = ".ogg";

        public override void _Ready()
		{
			#region Singleton Ready
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(SoundManager) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;
			#endregion

			base._Ready();

			AddAllSounds();
		}

		private void AddAllSounds()
		{
			var lSoundNames = typeof(SoundNames).GetFields();
			string lPath;
			string lSoundName;
			AudioStreamPlayer lAudioStream;

			foreach(var lField in lSoundNames)
			{
				lSoundName = lField.GetValue(null) as string;
                lPath = PATH_SOUNDS + lSoundName + EXTENSION;
				lAudioStream = new AudioStreamPlayer();
				lAudioStream.Stream = (AudioStream)GD.Load(lPath);
				lAudioStream.Name = lSoundName;
				sfxContainer.AddChild(lAudioStream);

				if (lSoundName == SoundNames.PLAYER_SHOOT_0 || lSoundName == SoundNames.PLAYER_SHOOT_1
					|| lSoundName == SoundNames.PLAYER_SHOOT_2 || lSoundName == SoundNames.PLAYER_SHOOT_3)
					sfxPlayerShoot.Add(lAudioStream);

				else if (lSoundName == SoundNames.ENEMY0_EXPLOSION_0 || lSoundName == SoundNames.ENEMY0_EXPLOSION_1
					|| lSoundName == SoundNames.ENEMY0_EXPLOSION_2 || lSoundName == SoundNames.ENEMY0_EXPLOSION_3)
					sfxEnemy0Explosion.Add(lAudioStream);

				else if (lSoundName == SoundNames.SF_SWITCH_0 || lSoundName == SoundNames.SF_SWITCH_1
					|| lSoundName == SoundNames.SF_SWITCH_2)
					{
					sfxSF.Add(lAudioStream);
					lAudioStream.VolumeDb = -2f;
				}

				else if (lSoundName == SoundNames.AMBIENCE_LOOP || lSoundName == SoundNames.UI_LOOP
					|| lSoundName == SoundNames.LEVEL_LOOP || lSoundName == SoundNames.BOSS_LOOP)
					lAudioStream.VolumeDb = 11f;
            }
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			base._Process(pDelta);
		}

		public void StartUI()
		{
            AudioStreamPlayer lAudioStreamPlayer = (AudioStreamPlayer)sfxContainer.GetNode(SoundNames.UI_LOOP);
            lAudioStreamPlayer.Finished += () => lAudioStreamPlayer.Play();
            lAudioStreamPlayer.Play();
        }

		public void StartGame()
		{
			AudioStreamPlayer lAudioStreamPlayer = (AudioStreamPlayer)sfxContainer.GetNode(SoundNames.LEVEL_LOOP);
            lAudioStreamPlayer.Finished += () => lAudioStreamPlayer.Play();
			lAudioStreamPlayer.Play();
        }

        

        public void StartBoss()
		{
            ((AudioStreamPlayer)sfxContainer.GetNode(SoundNames.LEVEL_LOOP)).Stop();
            ((AudioStreamPlayer)sfxContainer.GetNode(SoundNames.BOSS_LOOP)).Play();

            TickMove.Stop();
            TickShoot.Stop();
            TickNextPhase.Stop();
            TickRotate.Stop();
            TickSpawn.Stop();

            TickMove.Play();
			TickShoot.Play();
			TickNextPhase.Play();
			TickRotate.Play();
			TickSpawn.Play();
		}

        public void SingleSfx(string lStreamPlayerName)
        {
			AudioStreamPlayer lStreamPlayer = (AudioStreamPlayer)sfxContainer.GetNode(lStreamPlayerName);
            lStreamPlayer.Stop();
            lStreamPlayer.Play();
        }

        public void PlayerShoot()
		{
			sfxPlayerShoot[indexCurrentSfxPlayerShoot].Stop();
			indexCurrentSfxPlayerShoot = (indexCurrentSfxPlayerShoot + 1) % sfxPlayerShoot.Count;
            sfxPlayerShoot[indexCurrentSfxPlayerShoot].Play();
        }

        public void PlayerSfx()
        {
            sfxSF[indexCurrentSfxSF].Stop();
            indexCurrentSfxSF = (indexCurrentSfxSF + 1) % sfxSF.Count;
            sfxSF[indexCurrentSfxSF].Play();
        }

        public void Enemy0Explosion()
		{
            sfxEnemy0Explosion[indexCurrentSfxenemy0Explosion].Stop();
            indexCurrentSfxenemy0Explosion = (indexCurrentSfxenemy0Explosion + 1) % sfxEnemy0Explosion.Count;
            sfxEnemy0Explosion[indexCurrentSfxenemy0Explosion].Play();
        }

		public void BossExplosion()
		{
            AudioStreamPlayer lStreamPlayer = (AudioStreamPlayer)sfxContainer.GetNode(SoundNames.BOSS_PRE_EXPLOSION);
			AudioStreamPlayer lStreamPlayer2 = (AudioStreamPlayer)sfxContainer.GetNode(SoundNames.BOSS_EXPLOSION);
			lStreamPlayer.Finished += () => lStreamPlayer2.Play();
            lStreamPlayer.Play();
			lStreamPlayer.Finished += () => Boss.GetInstance().Destroy();
			lStreamPlayer.Finished += () => EndScreen.GetInstance().gameFinished(true);
        }

		public void EndGame(bool pIsWin)
		{
            ((AudioStreamPlayer)sfxContainer.GetNode(SoundNames.BOSS_LOOP)).Stop();
            if (pIsWin) ((AudioStreamPlayer)sfxContainer.GetNode(SoundNames.WIN_JINGLE)).Play();
			else ((AudioStreamPlayer)sfxContainer.GetNode(SoundNames.GAMEOVER_JINGLE)).Play();
        }

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			for (int i = AudioServer.BusCount - 1; i >= 1; i--)
			{
				AudioServer.RemoveBus(i);
			}
			base.Dispose(pDisposing);
		}
	}
}
