using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.Enums;
using Com.IsartDigital.SHMUP.GameObjects;
using Godot;
using System;
using System.Net;
using static Com.IsartDigital.SHMUP.GameManager;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables
{
	
	public partial class Movable : GameObject
	{
		public float speed;

        public Vector2 direction;

		protected float scrollSpeed;
		

		public override void _Ready()
		{
			base._Ready();
	
			scrollSpeed = GameManager.scrollSpeed;
			Signals.GetInstance().ChangeRunningState += ChangeIsGameRunnig;
		}

		public override void _Process(double pDelta)
		{
            base._Process(pDelta);

			float lDelta = (float)pDelta;
        }

		private void ChangeIsGameRunnig(bool pState)
		{
            SetProcess(pState);
		}

		protected virtual void DoAction(float pDelta)
		{
			Position += direction * speed * pDelta;
		}

        protected override void Dispose(bool disposing)
        {
			Signals.GetInstance().ChangeRunningState -= ChangeIsGameRunnig;
            base.Dispose(disposing);
        }
    }
}
