package oak.unusableman;

import org.andengine.entity.sprite.AnimatedSprite;
import org.andengine.entity.sprite.vbo.ITiledSpriteVertexBufferObject;
import org.andengine.opengl.texture.region.ITiledTextureRegion;
import org.andengine.opengl.texture.region.TiledTextureRegion;
import org.andengine.opengl.vbo.VertexBufferObjectManager;
import org.andengine.util.math.MathUtils;

public abstract class GameObject extends AnimatedSprite {

	/*
	 * private static ITiledTextureRegion textureRegion; private static
	 * VertexBufferObjectManager vertexBuffer; public static void
	 * loadResources(ITiledTextureRegion textureRegion,
	 * VertexBufferObjectManager vertexBuffer) { GameObject.textureRegion =
	 * textureRegion; GameObject.vertexBuffer = vertexBuffer; }
	 */
	private float elevation, elevationVelocity, velocity;

	public GameObject(float angle, float elevation, float velocity, TiledTextureRegion textureRegion) {
		super(0, 0, textureRegion, vertexBuffer);

		setRotation(angle);
		this.elevation = elevation;
		elevationVelocity = 0;
		this.velocity = velocity;
	}

	public void update(float delta, int worldRadius, float planetRotateSpeed) {
		setRotation((float) (getRotation() + (planetRotateSpeed + ((180 * velocity) / (Math.PI * worldRadius)))
				* delta));

		if (elevation > 0) {
			elevationVelocity -= Constants.GRAVITY;
			elevation += elevationVelocity;
		} else {
			elevation = 0;
		}

		setX((float) ((Constants.CAMERA_WIDTH / 2)
				+ (worldRadius + elevation + mRotationCenterY)
				* Math.sin(MathUtils.degToRad(getRotation())) - mRotationCenterX));
		setY((float) ((Constants.CAMERA_HEIGHT / 2)
				+ (worldRadius + elevation + mRotationCenterY)
				* -Math.cos(MathUtils.degToRad(getRotation())) - mRotationCenterY));
	}

}
