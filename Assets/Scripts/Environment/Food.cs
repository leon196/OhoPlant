using UnityEngine;

public class Food {
	public int index;
	public float speed;
	public Vector3 position;
	public Color[] shape;
	public int dimension;

	public Food (int index_, float speed_, Vector3 position_) {
		index = index_;
		speed = speed_;
		position = position_;
		dimension = 3;
		shape = new Color[dimension*dimension];
		for (int i = 0; i < dimension*dimension; ++i) {
			shape[i] = Random.Range(0f, 1f) > 0.5f ? Color.red : Color.clear;
		}
	}

	public void Move (float speedGlobal) 
	{
		//bool ground = position.y < 128;
		//float gravity = Time.deltaTime * speed * (ground ? 0.1f : 1f);
		position += Manager.GetMoonDirection() * speed * speedGlobal;
		if (position.x < 0f) {
			position.x = 255f;
		} else if (position.x > 255f) {
			position.x = 0f;
		}
		if (position.y < 0f) {
			position.y = 127f;
		} else if (position.y > 127f) {
			position.y = 0f;
		}
	}
}