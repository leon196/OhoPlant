using UnityEngine;

public class Droplet {
	public int index;
	public float speed;
	public Vector3 position;

	public Droplet (int index_, float speed_, Vector3 position_) {
		index = index_;
		speed = speed_;
		position = position_;
	}

	public void ApplyGravity () {
		position.y = (position.y - Time.deltaTime * speed);
	}
}