package oak.ld23.tinyworld
{
	import org.flixel.*;
	
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class AboutState extends FlxState
	{
		
		override public function create():void
		{
			add(new FlxText(0, 32, FlxG.width, "THIS GAME WAS MADE IN (LESS THAN) 48 HOURS FOR LD48 23 WITH THE THEME OF 'TINY WORLD' \n \n BY SAM OAKLEY, who celebrated his 16th sitting at a computer for 48 hours! \n \n ULOVEG \n \n Everything is original, right down to the brain numbing music... \n but I lied... the planet growth is linear... \n \n PRESS SPACE TO GO BACK"));
		}
		
		override public function update():void
		{
			
			if (FlxG.keys.justPressed("SPACE"))
				FlxG.switchState(new MenuState());
			super.update();
		}
	
	}

}