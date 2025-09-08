using Com.IsartDigital.ProjectName;
using Com.IsartDigital.SHMUP.Enums;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Collectibles
{
	
	public partial class PowerUp : Collectible
	{
        public static PackedScene packedScene = (PackedScene)GD.Load("res://Scenes/SHMUP/GameObjects/Collectibles/SmartBomb.tscn");

        protected override void OnCollision(Area2D pArea)
        {
            Player.GetInstance().Upgrade();
            SoundManager.GetInstance().SingleSfx(SoundNames.POWER_UP_FIRE_UPGRADE);
            base.OnCollision(pArea);
        }
    }
}
