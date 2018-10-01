package oak.ld23.tinyworld
{
	import org.flixel.*;
	
	/**
	 * ...
	 * @author Sam Oakley
	 */
	public class Player extends FlxSprite
	{
		[Embed(source="../../../../rsc/spt/unusableman.png")]
		private var playerImg:Class;
		
		[Embed(source="../../../../rsc/snd/jump.mp3")]
		private var jumpSnd:Class;
		
		private var world:World;
		private var elevationAcceleration:Number;
		
		private var elevation:Number;
		private var jumping:Boolean;
		
		private var controllable:Boolean;
		
		private var jumpSound:FlxSound;
		
		private var hurtTimer:FlxTimer;
		private var invulnerable:Boolean;
		
		//public var health:Number = 3;
		
		public function Player(world:World, controllable:Boolean)
		{
			this.world = world;
			loadGraphic(playerImg, true, true, 32, 48);
			angle = 0;
			health = 3;
			elevation = 0;
			this.controllable = controllable;
			
			invulnerable = false;
			
			jumpSound = new FlxSound();
			jumpSound.loadEmbedded(jumpSnd);
			
			facing = FlxObject.RIGHT;
			
			maxAngular = 100;
			angularDrag = maxAngular * 4;
			
			maxVelocity = new FlxPoint(1000, 1000);
			drag = new FlxPoint(maxVelocity.x * 4, maxVelocity.y * 4);
			elevationAcceleration = 0;
			
			x = (FlxG.width / 2) + (world.radius + elevation + origin.y) * Math.cos((angle - 90) * Math.PI / 180) - origin.x;
			y = (FlxG.height / 2) + (world.radius + elevation + origin.y) * Math.sin((angle - 90) * Math.PI / 180) - origin.y;
			
			addAnimation("standing", [0, 0, 0, 0, 0, 0, 0, 0, 1], 6, true);
			addAnimation("running", [2, 3], 6, true);
			addAnimation("jumping", [4]);
			play("standing");
			
			hurtTimer = new FlxTimer();
		}
		
		override public function update():void
		{
			elevationAcceleration -= world.gravity;
			
			if (!jumping)
				angle += world.rotateSpeed;
			
			if (controllable)
			{
				if (FlxG.keys.RIGHT)
				{
					angularAcceleration = maxAngular * 4;
					facing = FlxObject.RIGHT;
					if (FlxG.keys.justPressed("RIGHT") && !jumping)
						play("running");
				}
				else if (FlxG.keys.LEFT)
				{
					angularAcceleration = -maxAngular * 4;
					facing = FlxObject.LEFT;
					if (FlxG.keys.justPressed("LEFT") && !jumping)
						play("running");
				}
				else
				{
					angularAcceleration = 0;
					if (!jumping)
						play("standing");
				}
				
				//angularAcceleration /= world.radius;
				
				if ((FlxG.keys.UP || FlxG.keys.Z || FlxG.keys.SPACE) && !jumping)
					jump();
			}
			else
			{
				play("running");
				angularAcceleration = maxAngular * 4;
			}
			
			super.update();
			
			elevation += elevationAcceleration;
			
			if (elevation < 0)
			{
				elevation = 0;
				if (jumping)
					jumping = false;
				if (angularAcceleration != 0)
					play("running");
			}
			
			x = (FlxG.width / 2) + (world.radius + elevation + origin.y) * Math.cos((angle - 90) * Math.PI / 180) - origin.x;
			y = (FlxG.height / 2) + (world.radius + elevation + origin.y) * Math.sin((angle - 90) * Math.PI / 180) - origin.y;
		
		}
		
		private function jump():void
		{
			play("jumping");
			jumpSound.play();
			elevationAcceleration = 3;
			jumping = true;
		}
		
		override public function hurt(Damage:Number):void
		{
			if (!invulnerable)
			{
				health -= Damage;
				if (health <= 0)
					kill();
					
				invulnerable = true;
				flicker(0.5);
				hurtTimer.start(0.5, 1, makeVulnerable);
			}
		}
		
		private function makeVulnerable(timer:FlxTimer):void
		{
			invulnerable = false;
		}
	
	}

}