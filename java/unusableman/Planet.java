package oak.unusableman;

import org.andengine.entity.sprite.Sprite;
import org.andengine.opengl.texture.region.ITextureRegion;
import org.andengine.opengl.vbo.VertexBufferObjectManager;

public class Planet extends Sprite {

	private static float rotationSpeed = 1f;

	public Planet(float x, float y, ITextureRegion worldTextureRegion,
			VertexBufferObjectManager vertexBufferObjectManager) {
		super(x, y, worldTextureRegion, vertexBufferObjectManager);
	}
	
	public void update(float delta) {
		setRotation(this.getRotation() + (rotationSpeed * delta));
		rotationSpeed += 0.01f;
	}

	public int getRadius() {
		return (int) (getScaleX() * (getHeight() / 2));
	}
	
	private void increaseRadius(float increasePercent) {
		setScale(getScaleX() * increasePercent);
	}
	
	public float getRotationSpeed() {
		return rotationSpeed;
	}
}
