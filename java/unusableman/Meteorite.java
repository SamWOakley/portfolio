package oak.unusableman;

import org.andengine.opengl.texture.region.TiledTextureRegion;


public class Meteorite extends GameObject {

	public Meteorite(float angle, float elevation, float velocity, TiledTextureRegion textureRegion) {
		super(angle, elevation, velocity, textureRegion);
		animate(50);
	}
	
	public void explode() {
	}


}
