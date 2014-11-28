using UnityEngine;

public class Food : Element
{
	public int globalDimension;
	private float groundDecceleration;

	public Color[] shape;
	public int dimension;
	public int amount;

	public Food (int index_, Vector3 position_) 
	{
		index = index_;
		position = position_;

		globalDimension = Manager.Instance.Game.dimension;

		//velocity = 1f;
		groundDecceleration = Random.Range(1f, 2f);

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
	}

	public void Move (float speedGlobal) 
	{
		float ground = (position.y < globalDimension / 2 ? 1f : 0f);
		//float sky = 1f - ground;
		Vector3 moon = Manager.Instance.GetMoonDirection();

		position += (Vector3.down + moon * ground).normalized * speedGlobal;

		//velocity -= groundDecceleration * ground * Time.deltaTime;
	}

	public bool IsOutOfVelocity ()
	{
		return velocity <= 0f;
	}
}