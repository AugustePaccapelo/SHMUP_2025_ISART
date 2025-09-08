using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies;
using Godot;
using System;

// Author : Julien Fournier

namespace Com.IsartDigital.ProjectName {
	
	public partial class TickEvent : AudioStreamPlayer
	{
        protected AudioEffectSpectrumAnalyzerInstance spectrum;

        private const float FREQ_MAX = 11050.0f;
        private const float MIN_DB = 60f;
        protected int busIndex;

        protected Boss boss;
        protected Vector2 bossSize;

        //AnimationPlayer anim;
        protected RandomNumberGenerator rand = new RandomNumberGenerator();

        // timer pour éviter de trigger des events plusieurs fois pendant la durée d'un click
        // un genre de cooldown
        private float timer = 0.0f;

        // Seuil de décibel auquel doit être déclenché l'event
        private const float ENERGY_THRESHOLD = 0.25f;

        // quand le son dépase ENERGY_THRESOLD (seuil de décibel)
        // réglez TIMER_INTERVAL pour qu'il n'écoute plus le dépassement de db avant
        // avant TIMER_INTERVAL secondes 
        // attention si vous avez certain clicks trop rapprochés certains pourraient
        // être ignoré avec un interval trop long
        // En revanche un interval trop court générera des events non désirés
        private const float TIMER_INTERVAL = 0.25f;

        protected int count = 0;

        public override void _Ready()
        {
            rand.Randomize();

            boss = Boss.GetInstance();
            bossSize = boss.textureSize;

            AudioServer.AddBus(AudioServer.BusCount);
            busIndex = AudioServer.BusCount - 1;
            AudioServer.SetBusName(busIndex, Name.ToString());
            AudioServer.SetBusMute(busIndex, true);
            AudioServer.AddBusEffect(busIndex, new AudioEffectSpectrumAnalyzer());
            
            spectrum = AudioServer.GetBusEffectInstance(busIndex, 0) as AudioEffectSpectrumAnalyzerInstance;
            
            Bus = Name.ToString();

            // empêche le bug d'energy résiduelle.
            Finished += QueueFree;
        }

        public override void _Process(double pDelta)
        {
            base._Process(pDelta);

            Vector2 magnitude = spectrum.GetMagnitudeForFrequencyRange(0, FREQ_MAX, (int)AudioEffectSpectrumAnalyzerInstance.MagnitudeMode.Average);
            
            float energy = Mathf.Clamp((MIN_DB + Linear2Db(magnitude.Length())) / MIN_DB, 0, 1);

            //printez ici l'energy pour savoir à combien caler le THRESHOLD
            /*if (energy != 0)
            {
                GD.Print(Name, energy);
            }*/

            if (energy > ENERGY_THRESHOLD && timer >= TIMER_INTERVAL)
            {
                timer = 0;
                OnBeat();
            }

            timer += (float)pDelta;
        }

        protected virtual void OnBeat()
        {
            //GD.Print(Name + " Beat");
        }


        private float Linear2Db(float linear)
        {
            // Conversion de l'échelle linéaire en dB
            if (linear <= 0.0001f) return -MIN_DB; // Retourne une valeur de dB très basse si 'linear' est presque nul.
            return 20.0f * Mathf.Log(linear) / Mathf.Log(10.0f);
        }
    }
}
