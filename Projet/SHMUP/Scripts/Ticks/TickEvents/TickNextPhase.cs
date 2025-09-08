using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName {
	
	public partial class TickNextPhase : TickEvent
	{
        protected override void OnBeat()
        {
            base.OnBeat();
            boss.NewPhase();
        }
    }
}
