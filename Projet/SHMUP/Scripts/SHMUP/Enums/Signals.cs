using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.Enums {
	
	public partial class Signals : Node
	{
        #region Singleton
        static private Signals instance;

        private Signals() : base() { }

        static public Signals GetInstance()
        {
            if (instance == null) instance = new Signals();
            return instance;
        }
        #endregion

        [Signal] public delegate void ChangeRunningStateEventHandler(bool pState);
        [Signal] public delegate void EnemyDeathEventHandler(Vector2 pPosition);

        public override void _Ready()
        {
            #region Singleton
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(Signals) + " Instance already exist, destroying the last added.");
                return;

            }
            instance = this;
            #endregion


            base._Ready();
        }
    }
}
