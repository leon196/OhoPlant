using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Root : Element
{
	// 
	private Vector3 torsade;
	private float groundDecceleration;
	public int globalDimension;

	public Root (Vector3 position_) 
	{
		position = position_;
		direction = Manager.GetRandomRootDirection();
		torsade = new Vector3();
		
		globalDimension = Manager.Instance.Game.dimension;
		
		velocity = 1f;
		groundDecceleration = Random.Range(0.2f, 2f);
	}
	
	public void Grow (double translation, double rotation) 
	{
		//float angleTorsade = Mathf.PI * Mathf.Cos(Time.time * 2f) ;
		torsade.x = Mathf.Cos((float)rotation * Mathf.PI * 2f + Time.time);
		torsade.y = Mathf.Sin((float)rotation * Mathf.PI * 2f + Time.time);

		position = position + (direction * (float)translation + torsade).normalized;
		
		float ground = (position.y < globalDimension / 2 ? 1f : 0f);
		velocity -= groundDecceleration * ground * Time.deltaTime;
	}

	public bool IsOutOfVelocity ()
	{
		return velocity <= 0f;
	}
}
