package oak.ld23.tinyworld
{
	import org.flixel.*;
	
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class Shard extends FlxSprite
	{
		[Embed(source="../../../../rsc/spt/shard.png")]
		private var shardImg:Class;
		
		private var world:World;
		private var heightAcceleration:Number;
		private var elevation:Number;
		
		public function Shard(world:World, angle:Number)
		{
			this.world = world;
			this.angle = angle;
			elevation = 250;
			loadGraphic(shardImg, true);
			heightAcceleration = 0;
			
			x = (FlxG.width / 2) + (world.radius + elevation + origin.y) * Math.cos((angle - 90) * Math.PI / 180) - origin.x;
			y = (FlxG.height / 2) + (world.radius + elevation + origin.y) * Math.sin((angle - 90) * Math.PI / 180) - origin.y;
			
			addAnimation("pulse", [0, 1, 2, 3], 6);
			play("pulse");
		}
		
		override public function update():void
		{
			if (exists)
				heightAcceleration -= 0.01;
			
			
			elevation += heightAcceleration;
			angle += world.rotateSpeed;
			
			if (elevation < 0)
				elevation = 0;
				
			x = (FlxG.width / 2) + (world.radius + elevation + origin.y) * Math.cos((angle - 90) * Math.PI / 180) - origin.x;
			y = (FlxG.height / 2) + (world.radius + elevation + origin.y) * Math.sin((angle - 90) * Math.PI / 180) - origin.y;
			
			super.update();
		}
		
		public function resetShard(angle:Number):void 
		{
			elevation = (world.radius / 2) + 250;
			heightAcceleration = 0;
			this.angle = angle;
			super.reset((FlxG.width / 2) + (world.radius + elevation + origin.y) * Math.cos((angle - 90) * Math.PI / 180) - origin.x, (FlxG.height / 2) + (world.radius + elevation + origin.y) * Math.sin((angle - 90) * Math.PI / 180) - origin.y);
		}
		
		override public function kill():void 
		{
			elevation = 100;
			angle = Math.random() * 360;
			super.kill();
		}
	
	}

}