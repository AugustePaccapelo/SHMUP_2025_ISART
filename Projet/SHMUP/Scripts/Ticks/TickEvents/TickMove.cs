using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

// Author : Auguste Paccapelo

namespace Com.IsartDigital.ProjectName
{
    public partial class TickMove : TickEvent
    {
        private Node2D startPositionsContainer;
        private Node2D destinationsContainer;

        private List<Vector2> startPositions = new List<Vector2>();
        private List<Vector2> destinations = new List<Vector2>();
        
        private float traveltime = 0.5f;
        
        private float elapseTime = 0f;
        private bool isMoving = false;

        private Vector2 velocity;
        private Vector2 baseScale;

        private const string PATH_START_POSITIONS = "StartPositions";
        private const string PATH_DESTINATIONS = "Destinations";

        public override void _Ready()
        {
            base._Ready();
            startPositionsContainer = (Node2D)GameManager.bossPositionsContainer.GetNode(PATH_START_POSITIONS);
            destinationsContainer = (Node2D)GameManager.bossPositionsContainer.GetNode(PATH_DESTINATIONS);
            GetPositions();
        }

        private void GetPositions()
        {
            int lLength = startPositionsContainer.GetChildCount();
            for (int i = 0; i < lLength; i++)
            {
                startPositions.Add(((Marker2D)startPositionsContainer.GetChild(i)).Position);
            }
            lLength = destinationsContainer.GetChildCount();
            for (int i = 0;i < lLength; i++)
            {
                destinations.Add(((Marker2D)destinationsContainer.GetChild(i)).Position);
            }
        }

        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;

            base._Process(pDelta);
            if (isMoving)
            {
                elapseTime += lDelta;
                if (elapseTime > traveltime)
                {
                    isMoving = false;
                    boss.direction = Vector2.Zero;
                }
            }
        }

        protected override void OnBeat()
        {
            base.OnBeat();
            isMoving = true;
            boss.GlobalPosition = startPositions[count];
            Vector2 lVector = destinations[count] - startPositions[count];
            boss.speed = lVector.Length() / traveltime;
            boss.direction = lVector.Normalized();
            elapseTime = 0;

            if (count == 0)
            {
                boss.RotationDegrees = -90;
                baseScale = boss.Scale;
                boss.Scale = new Vector2(boss.Scale.X * 1.5f, boss.Scale.Y * 0.75f);
            }
            else if (count == 1)
            {
                boss.RotationDegrees = 180;
            }
            else if (count == 2)
            {
                boss.RotationDegrees = 0;
                boss.Scale = baseScale;
            }

            count++;
        }
    }
}
