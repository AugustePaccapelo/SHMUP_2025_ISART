using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.Enums {
	
	public enum CollisionLayers
	{
		Player = 1<<0,
		Enemy = 1<<1,
		Obstacle = 1<<2,
		BulletPlayer = 1<<3,
		BulletEnemy = 1<<4,
		Collectibles = 1<<5
	}
}
