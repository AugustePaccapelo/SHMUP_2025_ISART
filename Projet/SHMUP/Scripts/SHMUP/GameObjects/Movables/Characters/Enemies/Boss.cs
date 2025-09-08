using Com.IsartDigital.ProjectName;
using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;
using System.Transactions;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables.Characters.Enemies
{
	
	public partial class Boss : Enemy
	{

        #region Singleton
        static private Boss instance;

        private Boss() { }

        static public Boss GetInstance()
        {
            if (instance == null) { instance = new Boss(); }
            return instance;
        }
        #endregion

        private Timer timer = new Timer();

        private bool isFiring = false;

        private const float PHASE_1_DURATION = 15f;
        private const float PHASE_2_DURATION = 15f;
        private const float PHASE_3_DURATION = 15f;

        private const string PATH_SHOOT_POS_2 = "ShootPos2";

        private int currentPhase = 0;
        private int maxPhase = 2;

        public override void _Ready()
        {
            #region Singleton Ready
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(Boss) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
            #endregion
            
            base._Ready();
            
            fireRate = 7.5f;
            AreaEntered += OnCollision;

            timer.Timeout += EndTimer;
            timer.WaitTime = PHASE_1_DURATION;
            AddChild(timer);
        }

        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;

            base._Process(pDelta);
            
            DoAction(lDelta);

            if (isFiring) Shoot(EnumSpeeds.BULLET);
        }

        

        protected override void Move(float pDelta)
        {
            base.Move(pDelta);

            if (GlobalPosition.X * GameManager.parallaxBackground.Scale.X <= screenSize.X - textureSize.X * 1.5f * Scale.X * renderer.Scale.X)
            {
                Position += Vector2.Right * EnumSpeeds.SCROLL_SPEED * pDelta;
            }
        }

        public void SetActive()
        {
            timer.Start();
            SoundManager.GetInstance().StartBoss();
        }

        private void EndTimer()
        {
            timer.WaitTime = PHASE_2_DURATION;
            timer.Start();
        }

        public void NewPhase()
        {
            currentPhase++;
            if (currentPhase == 1)
            {
                timer.WaitTime = PHASE_2_DURATION;
            }
            else if (currentPhase == 2)
            {
                timer.WaitTime = PHASE_3_DURATION;
                GetShootMarkers((Node2D)GetNode(PATH_SHOOT_POS_2));
            }
            else 
            {
                SoundManager.GetInstance().BossExplosion();
                if (Player.GetInstance().smartBombNumber >= 3)
                {
                    Player.GetInstance().SmartBomb();
                    Player.GetInstance().AddSmartBomb();
                }
                else
                {
                    Player.GetInstance().AddSmartBomb();
                    Player.GetInstance().SmartBomb();
                }

            }
            renderer.Frame = currentPhase;
        }

        private void OnCollision(Area2D pArea)
        {
            //Damage feedBack
        }

        public void ChangeShoot(bool pIsFiring)
        {
            isFiring = pIsFiring;
        }

        public void ChangeFireRate(float pFireRate)
        {
            fireRate = pFireRate;
        }

        protected override void Dispose(bool disposing)
        {
            instance = null;
            base.Dispose(disposing);
        }
    }
}
