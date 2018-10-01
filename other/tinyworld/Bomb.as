package oak.ld23.tinyworld 
{
	import org.flixel.*;
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class Bomb extends FlxSprite
	{
		
		[Embed(source="../../../../rsc/spt/bomb.png")]
		private var bombImg:Class;
		
		private var world:World;
		
		private var elevation:Number;
		private var heightAcceleration:Number;
		
		private var bombTimer:FlxTimer;
		
		private var explodeTime:Number = 10;
		
		
		public function Bomb(world:World, angle:Number)
		{
			this.world = world;
			this.angle = angle;
			loadGraphic(bombImg, true);
			
			elevation = 250;
			heightAcceleration = 0;
			
			x = (FlxG.width / 2) + (world.radius + elevation + origin.y) * Math.cos((angle - 90) * Math.PI / 180) - origin.x;
			y = (FlxG.height / 2) + (world.radius + elevation + origin.y) * Math.sin((angle - 90) * Math.PI / 180) - origin.y;
			
			addAnimation("flash", [0, 1], 2, true);
			play("flash");
			
			bombTimer = new FlxTimer();
			bombTimer.start(5, 1, onTimer);
		}
		
		override public function update():void 
		{
			if (exists)
				heightAcceleration -= 0.005;
				
			elevation += heightAcceleration;
			
			angle += world.rotateSpeed;
			
			if (elevation < 0)
				elevation = 0;
			
			x = (FlxG.width / 2) + (world.radius + elevation + origin.y) * Math.cos((angle - 90) * Math.PI / 180) - origin.x;
			y = (FlxG.height / 2) + (world.radius + elevation + origin.y) * Math.sin((angle - 90) * Math.PI / 180) - origin.y;
			super.update();
		}
		
		private function onTimer(timer:FlxTimer):void
		{
			kill();
		}
		
		override public function kill():void 
		{
			PlayState.playState.createExplosion(x, y);
			elevation = (world.radius / 2) + 250;
			angle = Math.random() * 360;
			super.kill();
		}
		
		override public function revive():void 
		{
			super.revive();
			heightAcceleration = 0;
			bombTimer.start(explodeTime, 1, onTimer);
		}
	}

}