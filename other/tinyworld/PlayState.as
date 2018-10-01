package oak.ld23.tinyworld
{
	import org.flixel.*
	import org.flixel.plugin.photonstorm.FlxCollision;
	
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class PlayState extends FlxState
	{
		
		[Embed(source="../../../../rsc/spt/background.png")]
		private var backgroundImg:Class;
		
		[Embed(source="../../../../rsc/spt/explosion.png")]
		private var explosionImg:Class;
		
		[Embed(source="../../../../rsc/msc/HappyTheme.mp3")]
		private var themeSong:Class;
		
		[Embed(source = "../../../../rsc/snd/explode.mp3")]
		private var explodeSnd:Class;
		
		[Embed(source = "../../../../rsc/snd/grow.mp3")]
		private var shardSnd:Class;
		
		private var background:FlxSprite;
		
		private var world:World;
		private var player:Player;
		
		private var shards:FlxGroup;
		private var bombs:FlxGroup;
		
		private var shard:Shard;
		private var gameTimer:FlxTimer;
		
		private var scoreText:FlxText;
		
		public static var playState:PlayState;
		
		private var explosions:FlxGroup;
		
		private var song:FlxSound;
		private var shardPickupSound:FlxSound;
		private var explosionSound:FlxSound;
		
		private var healthBar:FlxSprite;
		
		private var gameTime:Number;
		
		override public function create():void
		{
			
			MenuState.song.stop();
			
			playState = this;
			
			background = new FlxSprite();
			background.loadGraphic(backgroundImg);
			background.x = -background.frameWidth / 2 ;
			background.y = -background.frameHeight / 2 ;
			background.scrollFactor = new FlxPoint(0.5, 0.5);
			add(background);
			
			world = new World(0.5);
			add(world);
			
			player = new Player(world, true);
			add(player);
			
			shards = new FlxGroup();
			
			var i:uint;
			
			for (i = 0; i < 3; i++)
			{
				shard = new Shard(world, Math.random() * 360);
				shard.exists = false;
				shards.add(shard);
			}
			add(shards);
			
			bombs = new FlxGroup();
			
			for (i = 0; i < 5; i++)
			{
				var bomb:Bomb = new Bomb(world, Math.random() * 360);
				bomb.exists = false;
				bombs.add(bomb);
			}
			
			add(bombs);
			
			explosions = new FlxGroup();
			for (i = 0; i < 5; i++)
			{
				var explosion:Explosion = new Explosion(0, 0);
				explosion.loadGraphic(explosionImg, true);
				explosion.exists = false;
				explosion.addAnimation("explode", [0, 1, 2], 6, false);
				explosion.addAnimation("none", [0]);
				explosions.add(explosion);
			}
			
			add(explosions);
			
			
			
			song = new FlxSound();
			song.loadEmbedded(themeSong, true);
			song.play();
			
			explosionSound = new FlxSound();
			explosionSound.loadEmbedded(explodeSnd);
			
			shardPickupSound = new FlxSound();
			shardPickupSound.loadEmbedded(shardSnd);
			
			gameTime = 0;
			
			scoreText = new FlxText(0, 0, 50, world.radius.toString());
			scoreText.scrollFactor.x = scoreText.scrollFactor.y = 0;
			add(scoreText);
			
			healthBar = new FlxSprite(FlxG.width - 50, 0);
			healthBar.makeGraphic(10, 8, 0xffff0000);
			healthBar.scrollFactor = new FlxPoint(0, 0);
			healthBar.scale.x = player.health;
			add(healthBar);
			
			FlxG.camera.follow(player);
		}
		
		override public function destroy():void 
		{
			song.stop();
			super.destroy();
		}
		
		override public function update():void
		{
			gameTime += FlxG.elapsed;
			
			if (!player.alive)
				FlxG.switchState(new LoseState(world.radius, gameTime));
			
			
			for each (var collectable:Shard in shards.members)
			{
				if (FlxCollision.pixelPerfectCheck(player, collectable) && collectable.exists)
					collectShard(player, collectable);
			}
			
			for each (var bomb:Bomb in bombs.members)
			{
				if (FlxCollision.pixelPerfectCheck(player, bomb) && bomb.exists)
					hitBomb(player, bomb);
			}
			
			if (Math.random() > 1 - world.radius / 2000)
			{
				if (shards.getFirstAvailable() != null)
					shards.getFirstAvailable().revive();
				else
				{
					shard = new Shard(world, Math.random() * 360);
					shards.add(shard);
				}
			}
			
			if (Math.random() > 1 - world.radius / 2500)
			{
				if (bombs.getFirstAvailable() != null)
					bombs.getFirstAvailable().revive();
				else
				{
					bomb = new Bomb(world, Math.random() * 360);
					bombs.add(bomb);
				}
			}
			
			explosions.callAll("updateFinished");
			scoreText.text = int(world.radius).toString();
			
			super.update();
		
		}
		
		private function collectShard(player:Player, shard:Shard):void
		{
			shardPickupSound.play();
			shard.kill();
			world.increaseSize();
			
		}
		
		private function hitBomb(player:Player, bomb:Bomb):void
		{
			bomb.kill();
			player.hurt(1);
			healthBar.scale.x = player.health;
			world.shrink();
			FlxG.shake();
		}
		
		public function createExplosion(x:Number, y:Number):void
		{
			explosionSound.play();
			if (explosions.getFirstAvailable() != null)
			{
				var explosion:Explosion = explosions.getFirstAvailable() as Explosion;
				
				explosion.revive();
				explosion.x = x;
				explosion.y = y;
				explosion.play("none");
				explosion.play("explode");
			}
		}
	
	}

}