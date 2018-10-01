package oak.ld23.tinyworld 
{
	import org.flixel.*;
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class MenuState extends FlxState
	{
		
		private var titleText:FlxText = new FlxText(FlxG.width / 2, 0, 160, "UNUSABLE MAN AND THE ORBS OF EXPONENTIAL GROWTH \n LD23");
		private var titlePulse:Boolean = false;
		
		private var startText:FlxText = new FlxText(0, 80, 100, "START <");
		private var aboutText:FlxText = new FlxText(0, 100, 100, "ABOUT");
		private var captionText:FlxText = new FlxText(0, 200, 100, "WARNING! DO NOT TAKE THIS GAME TOO SERIOUSLY!");
		private var authorText:FlxText = new FlxText(FlxG.width - 100, 200, 100, "BY SAM W OAKLEY");
		
		private var selectedEntry:Number = 0;
		
		private var world:World;
		private var character:Player;
		
		[Embed(source="../../../../rsc/msc/TitleTheme.mp3")]
		private var titleSong:Class;
		public static var song:FlxSound;
		
		[Embed(source="../../../../rsc/spt/background.png")]
		private var backgroundImg:Class;
		public static var background:FlxSprite;
		
		[Embed(source = "../../../../rsc/snd/select.mp3")]
		private var selectSnd:Class;
		private var selectSound:FlxSound;
		
		override public function create():void 
		{
			background = new FlxSprite();
			background.loadGraphic(backgroundImg);
			background.x = -background.frameWidth / 2 ;
			background.y = -background.frameHeight / 2 ;
			background.scrollFactor = new FlxPoint(0.5, 0.5);
			add(background);
			
			//titleText.size = 16;
			add(titleText);
			
			add(startText);
			add(aboutText);
			add(authorText);
			
			add(captionText);
			
			world = new World(1);
			add(world)
			
			character = new Player(world, false);
			character.angularVelocity = 50;
			add(character);
			
			if (song == null)
			{
				song = new FlxSound();
				song.loadEmbedded(titleSong, true);
				song.play();
			} else if (!song.active)
				song.play();
			
			selectSound = new FlxSound();
			selectSound.loadEmbedded(selectSnd);
		}
		
		override public function destroy():void 
		{
			//song.stop();
			super.destroy();
		}
		
		override public function update():void 
		{
			if (titlePulse)
			{
				titleText.scale.x += 0.01;
				titleText.scale.y -= 0.005;
			}
			else 
			{
				titleText.scale.x -= 0.01;
				titleText.scale.y += 0.005;
			}
			
			if (titleText.scale.x < 0.9 || titleText.scale.x > 1.1)
				titlePulse = !titlePulse;
			
			if (FlxG.keys.justPressed("UP") || FlxG.keys.justPressed("DOWN"))
			{
				selectedEntry++;
				selectSound.play();
			}
				
			if (selectedEntry > 1)
				selectedEntry = 0;
				
			
			if (selectedEntry == 0)
			{
				startText.color = FlxG.RED;
				startText.text = "START <";
				aboutText.color = FlxG.WHITE;
				aboutText.text = "ABOUT";
			}
			else
			{
				startText.color = FlxG.WHITE;
				startText.text = "START";
				aboutText.color = FlxG.RED;
				aboutText.text = "ABOUT <";
			}
			
			if (FlxG.keys.justPressed("SPACE") || FlxG.keys.justPressed("ENTER"))
			{
				switch (selectedEntry)
				{
					case 0:
						FlxG.switchState(new IntroState());
						break;
					case 1:
						FlxG.switchState(new AboutState());
						break;
				}
			}

			super.update();
		}
		
	}

}