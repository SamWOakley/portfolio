package oak.unusableman;

import org.andengine.entity.sprite.AnimatedSprite;
import org.andengine.opengl.texture.region.ITiledTextureRegion;
import org.andengine.opengl.vbo.VertexBufferObjectManager;
import org.andengine.util.math.MathUtils;

import android.os.DeadObjectException;
import android.os.Handler;
import android.os.Message;

public class Player extends AnimatedSprite {

	enum State {
		Dead, Jumping, Standing, Walking
	}

	static final int JUMP_HEIGHT = 35;
	private static final float MAX_VELOCITY = 250;
	private static final int WALK_ANIM_SPEED = 50;
	private State currentState;
	private float elevation = 0, elevationVelocity;
	private float velocity = 0;

	public Player(ITiledTextureRegion textureRegion,
			VertexBufferObjectManager vertexBufferObjectManager) {

		super(0, 0, textureRegion, vertexBufferObjectManager);
		currentState = State.Standing;
	}

	public float getElevation() {
		return elevation;
	}

	public State getState() {
		return currentState;
	}

	public void jump() {
		if (currentState != State.Jumping && currentState != State.Dead) {
			elevationVelocity = (float) Math.sqrt(2 * Constants.GRAVITY
					* JUMP_HEIGHT);
			currentState = State.Jumping;
			stopAnimation();
			animate(new long[] { 1 }, new int[] { 4 });
		}
	}

	public void kill() {
		if (currentState != State.Dead) {
			currentState = State.Dead;
			stopAnimation();
			animate(new long[] { 100, 1000 }, new int[] { 5, 6 }, false);
		}
	}

	public void move(float movementFactor) {

		if (Math.abs(movementFactor) < 2) {

			if (Math.abs(velocity) < MAX_VELOCITY * 0.1f)
				velocity = 0;
			else
				velocity += MAX_VELOCITY * 0.1f * -Math.signum(velocity);

		} else {
			velocity = MAX_VELOCITY * movementFactor / 10;
			velocity = Math.min(Math.abs(velocity), MAX_VELOCITY)
					* Math.signum(velocity);
		}
	}

	private void revive() {
		currentState = State.Standing;
		// TODO lives
	}

	public void update(float delta, int worldRadius, float planetRotateSpeed) {

		if (currentState == State.Dead) {
			if (!isAnimationRunning()) {
				revive();
			} else {
				return;
			}
		}

		setRotation((float) (getRotation() + (planetRotateSpeed + ((180 * velocity) / (Math.PI * worldRadius)))
				* delta));

		setX((float) ((Constants.CAMERA_WIDTH / 2)
				+ (worldRadius + elevation + mRotationCenterY)
				* Math.sin(MathUtils.degToRad(getRotation())) - mRotationCenterX));
		setY((float) ((Constants.CAMERA_HEIGHT / 2)
				+ (worldRadius + elevation + mRotationCenterY)
				* -Math.cos(MathUtils.degToRad(getRotation())) - mRotationCenterY));

		long walkAnimTime = (long) Math.min(
				Math.abs((WALK_ANIM_SPEED * MAX_VELOCITY / velocity)), 500);

		// Set animation

		if (Math.abs(velocity) > 0)
			setFlippedHorizontal((velocity < 0));

		if (elevation > 0)
			currentState = State.Jumping;

		if (currentState == State.Jumping) {
			if (!isAnimationRunning()) {
				animate(new long[] { 1 }, new int[] { 4 });
			}
			elevationVelocity -= Constants.GRAVITY;
			if (elevation < 0) {
				elevation = 0;
				stopAnimation();
				currentState = State.Standing;
				elevationVelocity = 0;
			}

			elevation += elevationVelocity;

		} else if (currentState == State.Standing) {
			if (velocity == 0) {
				if (!isAnimationRunning())
					animate(new long[] {
							(long) (3000 + Constants.RANDOM.nextFloat() * 2000),
							100 }, new int[] { 0, 1 }, 1);
			} else {
				currentState = State.Walking;
				stopAnimation();

				animate(new long[] { walkAnimTime, walkAnimTime }, new int[] {
						2, 3 }, 1);
			}
		} else if (currentState == State.Walking) {
			if (Math.abs(velocity) > 0) {
				if (!isAnimationRunning()) {
					animate(new long[] { walkAnimTime, walkAnimTime },
							new int[] { 2, 3 }, 1);
				}
			} else {
				currentState = State.Standing;
				stopAnimation();
				animate(new long[] { (long) (Math.random() * 10000), 100 },
						new int[] { 0, 1 }, 1);
			}
		}

	}
}
