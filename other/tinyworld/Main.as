package oak.ld23.tinyworld
{
	import org.flixel.*;
	[SWF(width="640", height="480", backgroundColor="#ffffff")]
	
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class Main extends FlxGame 
	{	
		public function Main():void 
		{
			super(320, 240, MenuState, 2);
		}
		
	}
	
}