using Com.IsartDigital.ProjectName;
using Com.IsartDigital.SHMUP.Enums;
using Com.IsartDigital.SHMUP.GameObjects.Movables.Characters;
using Godot;
using System;

// Author : PACCAPELO Auguste

namespace Com.IsartDigital.SHMUP.GameObjects.Collectibles
{
	
	public partial class Heal : Collectible
	{
		[Export] private float bonusHealth = 20f;

        public static PackedScene packedScene = (PackedScene)GD.Load("res://Scenes/SHMUP/GameObjects/Collectibles/Health.tscn");
        
        protected override void OnCollision(Area2D pArea)
        {
			Player.GetInstance().AddHealth(bonusHealth);
            SoundManager.GetInstance().SingleSfx(SoundNames.POWER_UP_LIFE);
            
            CallDeferred(nameof(Animation));
        }

        private void Animation()
        {
            Reparent(GameManager.gameContainer);
            Monitoring = false;

            Tween lTween = CreateTween();

            lTween.TweenProperty(this, "global_position", screenSize * 0.5f / GameManager.parallaxBackground.Scale, 1f);
            lTween.Parallel().TweenProperty(this, "scale", Scale * 1.5f, 1f);
            lTween.Chain().TweenProperty(this, "global_position", HUD.GetInstance().healthBar.Position / GameManager.parallaxBackground.Scale, 1f);
            lTween.Parallel().TweenProperty(this, "scale", Vector2.Zero, 1f);
            lTween.Finished += () => QueueFree();
        }
    }
}
