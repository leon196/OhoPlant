using UnityEngine;

public class Food {
	public int index;
	public float speed;
	public Vector3 position;
	public Color[] shape;
	public int dimension;
	public int globalDimension;
	public int amount;

	public Food (int index_, float speed_, Vector3 position_) {
		index = index_;
		speed = speed_;
		position = position_;
		dimension = 2;
		shape = new Color[dimension*dimension];
		amount = 0;
		for (int i = 0; i < dimension*dimension; ++i) {
			Color color;
			if (Random.Range(0f, 1f) > 0.5f) {
				color = Color.red;
				++amount;
			} else {
				color = Color.clear;
			}
			shape[i] = color;
		}

		globalDimension = Manager.Instance.Game.dimension;
	}

	public void Move (float speedGlobal) 
	{
		//bool ground = position.y < 128;
		//float gravity = Time.deltaTime * speed * (ground ? 0.1f : 1f);
		position += Manager.Instance.GetMoonDirection() * speed * speedGlobal;
		if (position.x < 0f) {
			position.x = globalDimension - 1;
		} else if (position.x >= globalDimension) {
			position.x = 0f;
		}
		if (position.y < 0f) {
			position.y = globalDimension/2;
		} else if (position.y > globalDimension/2) {
			position.y = 0f;
		}
	}
}