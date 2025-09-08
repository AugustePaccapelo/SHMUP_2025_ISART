using Godot;
using System;
using System.Collections.Generic;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.ProjectName {
	
	public partial class TickRotate : TickEvent
	{
        private List<float> allRotations = new List<float>() { -90f, -180f, -45f, 0f};
        private float travelTime = 0.5f;
        private bool isMoving = false;
        private float elapesTime = 0f;
        private float rotationSpeed;

        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;

            base._Process(pDelta);

            if (isMoving)
            {
                elapesTime += lDelta;
                if (elapesTime >= travelTime)
                {
                    isMoving = false;
                    elapesTime = 0f;
                    boss.Rotation = Mathf.DegToRad(allRotations[count - 1]);
                    return;
                }
                if (boss.Rotation > Mathf.DegToRad(allRotations[count - 1])) boss.Rotation += rotationSpeed * lDelta;
                else boss.Rotation -= rotationSpeed * lDelta;
            }
        }

        protected override void OnBeat()
        {
            base.OnBeat();
            isMoving = true;
            rotationSpeed = (Mathf.DegToRad(allRotations[count]) - boss.Rotation) / travelTime;
            count++;
        }
    }
}
