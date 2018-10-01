package oak.unusableman;

import static oak.unusableman.Constants.CAMERA_HEIGHT;
import static oak.unusableman.Constants.CAMERA_WIDTH;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.Random;

import org.andengine.engine.camera.SmoothCamera;
import org.andengine.engine.handler.IUpdateHandler;
import org.andengine.engine.options.EngineOptions;
import org.andengine.engine.options.ScreenOrientation;
import org.andengine.engine.options.resolutionpolicy.RatioResolutionPolicy;
import org.andengine.entity.scene.IOnSceneTouchListener;
import org.andengine.entity.scene.Scene;
import org.andengine.entity.util.FPSLogger;
import org.andengine.input.sensor.acceleration.AccelerationData;
import org.andengine.input.sensor.acceleration.IAccelerationListener;
import org.andengine.input.touch.TouchEvent;
import org.andengine.opengl.texture.ITexture;
import org.andengine.opengl.texture.bitmap.BitmapTexture;
import org.andengine.opengl.texture.region.ITextureRegion;
import org.andengine.opengl.texture.region.ITiledTextureRegion;
import org.andengine.opengl.texture.region.TextureRegion;
import org.andengine.opengl.texture.region.TextureRegionFactory;
import org.andengine.opengl.texture.region.TiledTextureRegion;
import org.andengine.ui.activity.SimpleBaseGameActivity;
import org.andengine.util.adt.io.in.IInputStreamOpener;
import org.andengine.util.debug.Debug;

import android.os.Bundle;

public class MainActivity extends SimpleBaseGameActivity implements
		IAccelerationListener {

	private static final float CAMERA_PAN_SPEED = 100f;

	SmoothCamera camera;

	ArrayList<Meteorite> meteorites;

	private TextureRegion planetTextureRegion, buttonTextureRegion;
	private Player player;

	private TiledTextureRegion playerTextureRegion;

	@Override
	public void onAccelerationAccuracyChanged(AccelerationData pAccelerationData) {
		return;
	}

	@Override
	public void onAccelerationChanged(AccelerationData pAccelerationData) {
		int accelerameterSpeedX = (int) pAccelerationData.getX();
		player.move(accelerameterSpeedX);
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		// setContentView(R.layout.activity_main);
	}

	@Override
	public EngineOptions onCreateEngineOptions() {
		camera = new SmoothCamera(0, 0, CAMERA_WIDTH, CAMERA_HEIGHT,
				CAMERA_PAN_SPEED, CAMERA_PAN_SPEED, 3f);
		return new EngineOptions(true, ScreenOrientation.LANDSCAPE_FIXED,
				new RatioResolutionPolicy(CAMERA_WIDTH, CAMERA_HEIGHT), camera);
	}

	@Override
	protected void onCreateResources() {

		enableAccelerationSensor(this);

		try {
			// 1 - Set up bitmap textures
			ITexture worldTexture = new BitmapTexture(this.getTextureManager(),
					new IInputStreamOpener() {

						@Override
						public InputStream open() throws IOException {
							return getAssets().open("gfx/world.png");
						}
					});

			ITexture playerTexture = new BitmapTexture(getTextureManager(),
					new IInputStreamOpener() {

						@Override
						public InputStream open() throws IOException {
							return getAssets().open("gfx/hero.png");
						}
					});

			ITexture buttonTexture = new BitmapTexture(getTextureManager(),
					new IInputStreamOpener() {

						@Override
						public InputStream open() throws IOException {
							return getAssets().open("gfx/button.png");
						}
					});

			ITexture meteoriteTexture = new BitmapTexture(getTextureManager(),
					new IInputStreamOpener() {

						@Override
						public InputStream open() throws IOException {
							return getAssets().open("gfx/meteorite.png");
						}
					});

			// 2 - Load bitmap texture into VRAM
			worldTexture.load();
			playerTexture.load();
			buttonTexture.load();
			meteoriteTexture.load();

			// 3 - Set up texture regions
			planetTextureRegion = TextureRegionFactory
					.extractFromTexture(worldTexture);
			playerTextureRegion = TextureRegionFactory.extractTiledFromTexture(
					playerTexture, playerTexture.getWidth() / 32,
					playerTexture.getHeight() / 48);
			buttonTextureRegion = TextureRegionFactory
					.extractFromTexture(buttonTexture);
			// Meteorite.loadResources(TextureRegionFactory
			// .extractTiledFromTexture(meteoriteTexture, 4, 1),
			// getVertexBufferObjectManager());

		} catch (IOException e) {
			Debug.e(e);
		}
	}

	@Override
	protected Scene onCreateScene() {
		// 1 - Create new scene
		final Scene scene = new Scene();

		// Set up hud
		/*
		 * HUD hud = new HUD();
		 * 
		 * ButtonSprite antiClockwiseMovementButton = new ButtonSprite(0, 0,
		 * buttonTextureRegion, getVertexBufferObjectManager());
		 * antiClockwiseMovementButton.setOnClickListener(new OnClickListener()
		 * {
		 * 
		 * @Override public void onClick(ButtonSprite pButtonSprite, float
		 * pTouchAreaLocalX, float pTouchAreaLocalY) { player.move(-5); } });
		 * scene.registerTouchArea(antiClockwiseMovementButton);
		 * hud.attachChild(antiClockwiseMovementButton);
		 * 
		 * ButtonSprite clockwiseMovementButton = new ButtonSprite(
		 * camera.getWidth() - buttonTextureRegion.getWidth(), 0,
		 * buttonTextureRegion, getVertexBufferObjectManager());
		 * clockwiseMovementButton.setOnClickListener(new OnClickListener() {
		 * 
		 * @Override public void onClick(ButtonSprite pButtonSprite, float
		 * pTouchAreaLocalX, float pTouchAreaLocalY) { player.move(5); } });
		 * scene.registerTouchArea(clockwiseMovementButton);
		 * hud.attachChild(clockwiseMovementButton);
		 * 
		 * camera.setHUD(hud);
		 */

		final Planet planet = new Planet((CAMERA_WIDTH / 2)
				- (planetTextureRegion.getWidth() / 2), (CAMERA_HEIGHT / 2)
				- (planetTextureRegion.getHeight() / 2), planetTextureRegion,
				getVertexBufferObjectManager());
		scene.attachChild(planet);

		player = new Player(playerTextureRegion, getVertexBufferObjectManager());
		scene.attachChild(player);
		camera.setChaseEntity(player);

		Random random = new Random();

		meteorites = new ArrayList<Meteorite>();
		for (int i = 0; i < 1; i++) {
			Meteorite m = new Meteorite(random.nextFloat() * 360, 200,
					random.nextFloat() * 100);
			meteorites.add(m);
			scene.attachChild(m);
		}

		scene.registerUpdateHandler(new FPSLogger());
		scene.registerUpdateHandler(new IUpdateHandler() {

			@Override
			public void onUpdate(float pSecondsElapsed) {
				// Update loop of game

				planet.update(pSecondsElapsed);
				player.update(pSecondsElapsed, planet.getRadius(),
						planet.getRotationSpeed());

				for (Meteorite m : meteorites) {
					m.update(pSecondsElapsed, planet.getRadius(),
							planet.getRotationSpeed());

					if (player.collidesWith(m))
						player.kill();
				}

				// camera.setZoomFactor(Math.min(Player.JUMP_HEIGHT /
				// player.getElevation(), 1));

			}

			@Override
			public void reset() {

			}
		});

		scene.setOnSceneTouchListener(new IOnSceneTouchListener() {

			@Override
			public boolean onSceneTouchEvent(Scene pScene,
					TouchEvent pSceneTouchEvent) {
				player.jump();
				return false;
			}
		});
		return scene;
	}

}
