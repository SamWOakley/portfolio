package oak.ld23.tinyworld 
{
	import org.flixel.*;
	
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class LoseState extends FlxState 
	{
		private var score:Number;
		private var time:Number;
		
		[Embed(source="../../../../rsc/msc/LoseTheme.mp3")]
		private var loseSound:Class;
		
		[Embed(source="../../../../rsc/spt/background.png")]
		private var backgroundImg:Class;
		
		public static var background:FlxSprite;
		
		private var song:FlxSound;
		
		public function LoseState(score:Number, time:Number)
		{
			this.score = score;
			this.time = time;
			super();
		}
		
		override public function destroy():void 
		{
			song.stop();
			super.destroy();
		}
		
		
		override public function create():void 
		{

			background = new FlxSprite();
			background.loadGraphic(backgroundImg);
			background.x = -background.frameWidth / 2 ;
			background.y = -background.frameHeight / 2 ;
			background.scrollFactor = new FlxPoint(0.5, 0.5);
			add(background);
			
			var world:World = new World(1);
			add(world)
			add(new Player(world, false));
			var gameOverText:FlxText = new FlxText(0, 0, 240, "GAME OVER \n" + "You made the world " + score.toString() + " metres wide \n" + "and lasted " + (int(time * 10)/10).toString() + " seconds \n     press space to play again \n            or press enter to go back to the menu");
			gameOverText.color = FlxG.RED;
			add(gameOverText);
			
			song = new FlxSound();
			song.loadEmbedded(loseSound, true);
			song.play();
		}
		
		override public function update():void 
		{
			if (FlxG.keys.justPressed("SPACE"))
				FlxG.switchState(new PlayState());
			else if (FlxG.keys.justPressed("ENTER"))
				FlxG.switchState(new MenuState());
			
			super.update();
		}
		
	}

}