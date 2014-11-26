using UnityEngine;

public class Droplet {
	public int index;
	public float speed;
	public Vector3 position;
	private float globalDimension;

	public Droplet (int index_, float speed_, Vector3 position_) {
		index = index_;
		speed = speed_;
		position = position_;
		globalDimension = Manager.Instance.Game.dimension;
	}

	public void ApplyGravity (float speedGlobal) 
	{
		bool ground = position.y < globalDimension/2;
		float gravity = Time.deltaTime * speed * (ground ? 0.1f : 1f);
		position += (Time.deltaTime * Manager.Instance.GetMoonDirection() * (ground ? speedGlobal * 40f : 10f) * (Random.Range(0f, 1f) > 0.2 ? 1f : 0f) + new Vector3(0f, -gravity, 0f)).normalized;
	}
}