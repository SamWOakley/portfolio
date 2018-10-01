package oak.ld23.tinyworld
{
	import org.flixel.*;
	
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class Explosion extends FlxSprite
	{
		
		public function Explosion(x:Number, y:Number) 
		{
			super(x, y);
		}
		
		public function updateFinished():void
		{
			if (_curFrame == 2)
				kill();
		}
	
	}

}