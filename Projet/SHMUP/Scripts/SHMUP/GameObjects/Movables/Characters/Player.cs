using Com.IsartDigital.ProjectName;
using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.Enums;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Ammos;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies;
using Com.IsartDigital.Utils.Effects;
using Godot;
using System;
using System.Reflection.Metadata;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables.Characters
{
	
	public partial class Player : Character
	{
        #region Singleton
        static private Player instance;

        private Player() { }

        static public Player GetInstance()
        {
            if (instance == null) { instance = new Player(); }
            return instance;
        }
        #endregion

        private static PackedScene scenePlayer = (PackedScene)GD.Load("res://Scenes/SHMUP/GameObjects/Movables/Characters/Player.tscn");

        private const string PATH_ACTION_LEFT = "MoveLeft";
        private const string PATH_ACTION_RIGHT = "MoveRight";
        private const string PATH_ACTION_UP = "MoveUp";
        private const string PATH_ACTION_DOWN = "MoveDown";
        private const string PATH_ACTION_SHOOT = "Shoot";
        private const string PATH_ACTION_SMART_BOMB = "SmartBomb";
        private const string PATH_ACTION_SPECIAL = "Special";
        private const string PATH_ACTION_PAUSE = "Pause";
        private const string PATH_ACTION_GOD_MODE = "GodMode";
        private const string PATH_SHOOT_POS_LVL = "ShootPosLvl";

        [Export] private AnimatedSprite2D shadow;
        [Export] private Shaker shaker;

        private float maxHealth = 100f;

        private bool isFiring = false;

        public int smartBombNumber = 1;
        private int maxSmartBomb = 3;

        private int currentUpgrade = 0;
        private int minUpgrade = 0;
        private int maxUpgrade = 2;

        private int currentFilter = 0;
        private int numFilter = 3;

        private float minFireRate = 5f;

        public bool isGodMod = false;

        private bool canLooseUpgrade = true;
        private float invisibleTime = 0.5f;
        Timer timer = new Timer();

        private Vector2 startScale;

        public override void _Ready()
        {
            #region Singleton Ready
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(Player) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
            #endregion

            startScale = Scale;

            isAlly = true;

            base._Ready();

            speed = EnumSpeeds.PLAYER;

            GlobalPosition = new Vector2(textureSize.X * 1.5f, screenSize.Y * 0.5f) / GameManager.parallaxBackground.Scale;
            UpdateCurrentUpgrade();
            GameManager.GetInstance().ChangeLayer(currentFilter);

            timer.WaitTime = invisibleTime;
            timer.Timeout += TimerTimeout;
            AddChild(timer);
        }

        private void TimerTimeout()
        {
            canLooseUpgrade = true;
        }

        public override void _Process(double pDelta)
		{
            base._Process(pDelta);

            float lDelta = (float)pDelta;

            Position += Vector2.Right * GameManager.scrollSpeed * lDelta;

			DoAction(lDelta);
        }

        public static Player Create(Node2D pParent)
        {
            Player lPlayer = (Player)scenePlayer.Instantiate();
            pParent.AddChild(lPlayer);
            return lPlayer;
        }

        protected override void DoAction(float pDelta)
        {
            base.DoAction(pDelta);
            
            if (isFiring) Shoot(EnumSpeeds.BULLET);
        }

        private void UpdateCurrentUpgrade()
        {
            renderer.Frame = currentUpgrade;
            shadow.Frame = currentUpgrade;

            string lContainerName = PATH_SHOOT_POS_LVL + (currentUpgrade + 1).ToString();

            GetShootMarkers((Node2D)FindChild(lContainerName));

            fireRate = minFireRate * shootPosMarkers.Count * 0.5f;
        }

        public override void DoDamage(float pDamage)
        {
            if (isGodMod) return;
            base.DoDamage(pDamage);
            if (canLooseUpgrade)
            {
                currentUpgrade--;
                if (currentUpgrade < minUpgrade) currentUpgrade = minUpgrade;
                UpdateCurrentUpgrade();
                canLooseUpgrade = false;
                timer.Start();
                SoundManager.GetInstance().SingleSfx(SoundNames.LOSE_LIFE);
                Tween lTween = CreateTween();

                lTween.TweenProperty(shadow, "modulate", Colors.Red, 0.5f);
                lTween.Chain().TweenProperty(shadow, "modulate", Colors.Transparent, 0.5f);

                shaker.Start();
            }
            HUD.GetInstance().UpdateHealth(health);
        }

        public override void Destroy()
        {
            SoundManager.GetInstance().SingleSfx(SoundNames.PLAYER_EXPLOSION);
            Hide();
            CallDeferred(MethodName.SetMonitoring, false);
            GameManager.GetInstance().StopGame();
            EndScreen.GetInstance().gameFinished(false);
        }

        public void SmartBomb()
        {
            if (smartBombNumber <= 0) return;
            smartBombNumber--;

            SoundManager.GetInstance().SingleSfx(SoundNames.BOMB);

            Node2D lContainer = GameManager.enemiesContainer;
            Enemy lEnemy;
            for (int i = lContainer.GetChildCount() - 1;  i > 0; i--)
            {
                if (lContainer.GetChild(i) is not Enemy) continue;
                lEnemy = (Enemy)lContainer.GetChild(i);
                if (lEnemy is not Boss && lEnemy.isMoving) lEnemy.Destroy();
            }

            lContainer = GameManager.ammoContainer;
            for (int i = lContainer.GetChildCount() - 1;i > 0;i--)
            {
                ((Ammo)lContainer.GetChild(i)).Destroy();
            }
            HUD.GetInstance().UpdateSmartBomb();
        }

        public override void _Input(InputEvent pEvent)
        {
            base._Input(pEvent);
            
            direction = new Vector2(
                Input.GetActionStrength(PATH_ACTION_RIGHT) - Input.GetActionStrength(PATH_ACTION_LEFT),
                Input.GetActionStrength(PATH_ACTION_DOWN) - Input.GetActionStrength(PATH_ACTION_UP));

            if (isFiring != Input.IsActionPressed(PATH_ACTION_SHOOT)) isFiring = Input.IsActionPressed(PATH_ACTION_SHOOT);
            if (Input.IsActionJustPressed(PATH_ACTION_SMART_BOMB)) SmartBomb();
            if (Input.IsActionJustPressed(PATH_ACTION_SPECIAL)) ChangeFilter();
            if (Input.IsActionJustPressed(PATH_ACTION_PAUSE)) Pause();
            if (Input.IsActionJustPressed(PATH_ACTION_GOD_MODE)) ChangeGodMod();
        }

        private void Pause()
        {
            PauseMenu.GetInstance().Show();
            GameManager.GetInstance().StopGame();
        }

        public void AddHealth(float pHealth)
        {
            health += pHealth;
            if (health > maxHealth) health = maxHealth;
            HUD.GetInstance().UpdateHealth(health);
        }

        public void AddSmartBomb()
        {
            smartBombNumber++;
            if (smartBombNumber > maxSmartBomb) smartBombNumber = maxSmartBomb;
            HUD.GetInstance().UpdateSmartBomb();
        }

        public void Upgrade()
        {
            currentUpgrade++;
            if (currentUpgrade > maxUpgrade) currentUpgrade = maxUpgrade;
            else
            {
                UpdateCurrentUpgrade();
                Tween lTween = CreateTween();

                lTween.TweenProperty(shadow, "modulate", Colors.White, 0.5f);
                lTween.Parallel().TweenProperty(this, "scale", Scale * 1.5f, 0.5f);
                lTween.Chain().TweenProperty(shadow, "modulate", Colors.Transparent, 0.5f);
                lTween.Parallel().TweenProperty(this, "scale", startScale, 0.5f);
            }
        }

        private void ChangeFilter()
        {
            currentFilter = (currentFilter + 1) % numFilter;
            GameManager.GetInstance().ChangeLayer(currentFilter);
            SoundManager.GetInstance().PlayerSfx();
        }

        private void ChangeGodMod()
        {
            isGodMod = !isGodMod;
            HUD.GetInstance().godModLabel.Visible = isGodMod;
        }

        protected override void Dispose(bool pDisposing)
		{
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
