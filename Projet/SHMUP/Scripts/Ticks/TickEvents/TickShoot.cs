using Godot;
using System;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.ProjectName
{
	public partial class TickShoot : TickEvent
    {
        private bool isShooting = false;

        protected override void OnBeat()
        {
            base.OnBeat();

            if (isShooting)
            {
                isShooting = false;
                boss.ChangeShoot(isShooting);
            }
            else
            {
                isShooting = true;
                boss.ChangeShoot(isShooting);
            }
        }
    }
}

