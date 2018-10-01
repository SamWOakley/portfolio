package oak.ld23.tinyworld
{
	import org.flixel.*;
	
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class IntroState extends FlxState
	{
		
		private var introText:FlxText;
		private var introString:String = "In the year 2012, something astronomical happened... literally. \n Combined factors of the aging of the earth, and increasing body mass of human population resulted in... \n A TINY WORLD \n Scientists have found a special energy that will make the earth big again, and this seems to be magically falling from the sky... along with bombs, no doubt sent by some unnamed villain. It's up to you, the hero to save the Earth!! Avoid the bombs, and collect blue orby things! Good luck! \n \n \n Ps. I apologise for the goddawful story...";
		private var beginText:FlxText = new FlxText(0, FlxG.height / 2, FlxG.width, "PRESS SPACE TO BEGIN");
		
		[Embed(source="../../../../rsc/spt/background.png")]
		private var backgroundImg:Class;
		
		public static var background:FlxSprite;
		
		private var world:World;
		private var shrunkWorld:Boolean;
		
		override public function create():void
		{
			
			background = new FlxSprite();
			background.loadGraphic(backgroundImg);
			background.x = -background.frameWidth / 2 ;
			background.y = -background.frameHeight / 2 ;
			background.scrollFactor = new FlxPoint(0.5, 0.5);
			add(background);
			
			world = new World(2);
			add(world);
			shrunkWorld = false;
			
			introText = new FlxText(0, FlxG.height, FlxG.width, introString);
			add(introText);
			
			beginText.exists = false;
			add(beginText);
		
		}
		
		override public function update():void
		{
			if (introText.y > 0)
				introText.y -= 0.5;
			else
			{
				if (!beginText.exists)
					beginText.exists = true;
				if (!shrunkWorld)
					world.radius -= 15;
					world.targetRadius = world.radius;
					if (world.radius < 5)
					{
						world.radius = 5;
						shrunkWorld = true;
					}
				
			}
			
			if (FlxG.keys.justPressed("SPACE"))
				FlxG.switchState(new PlayState());

			
			super.update();
		}
	
	}

}