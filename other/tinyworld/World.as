package oak.ld23.tinyworld
{
	import org.flixel.*;
	
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class World extends FlxSprite
	{
		
		
		[Embed(source="../../../../rsc/spt/world.png")]
		private var mainWorldImg:Class;
		
		public var radius:Number;
		public var targetRadius:Number;
		public var rotateSpeed:Number = 0.1;
		
		public var gravity:Number;
		
		public function World(size:Number)
		{
			super(0, 0);
			loadGraphic(mainWorldImg);
			scale = new FlxPoint(size, size);
			x = FlxG.width / 2 - width / 2;
			y = FlxG.height / 2 - height / 2;
			immovable = true;
			radius = (width / 2) * size;
			targetRadius = radius;
			gravity = 4 / radius;
		}
		
		override public function update():void
		{
			angle += rotateSpeed;
			if (radius < targetRadius)
				radius += 0.5;
			else if (radius > targetRadius)
				radius = targetRadius;
			scale = new FlxPoint(radius / 64, radius / 64);
			super.update();
			
			rotateSpeed += 0.0005;
		}
		
		public function increaseSize():void
		{
			targetRadius += 5;
		}
		
		public function shrink():void
		{
			targetRadius -= 15;
			if (targetRadius < 30)
				targetRadius = 30;
		}
	
	}

}