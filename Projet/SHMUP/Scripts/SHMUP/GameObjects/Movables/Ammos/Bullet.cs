using Com.IsartDigital.SHMUP;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Ammos;
using Com.IsartDigital.SHMUP.Enums;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Movables.Ammos
{

	public partial class Bullet : Ammo
	{
        private static PackedScene sceneBullet = (PackedScene)GD.Load("res://Scenes/SHMUP/GameObjects/Movables/Ammos/Bullet.tscn");

        public override void _Ready()
		{
            base._Ready();
			speed = EnumSpeeds.BULLET;
        }

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);

			float lDelta = (float)pDelta;
		}

		public static Bullet Create(Vector2 pPosition, bool pIsAlly, float pRotation)
		{
            Bullet lBullet = sceneBullet.Instantiate() as Bullet;
			lBullet.isAlly = pIsAlly;
            GameManager.ammoContainer.AddChild(lBullet);
            lBullet.Position = pPosition;
			lBullet.Rotation = pRotation;
			lBullet.direction = lBullet.direction.Rotated(pRotation);
			return lBullet;
        }
	}
}
