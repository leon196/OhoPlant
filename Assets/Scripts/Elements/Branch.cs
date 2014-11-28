using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Branch : Element
{
	public Leaf leaf;
	public float photosynthesize;

	private float length;

	private Vector3 positionInitial;
	public int depth;

	private float angleIndulgence = 10f;

	public Branch (Vector3 position_, Vector3 direction_, int depth_)
	{
		position = position_;
		direction = direction_.normalized;

		positionInitial = position;

		leaf = new Leaf(position, Manager.Instance.GetRandomLeafDirection());
		photosynthesize = 0f;

		// L-System Node Level
		length = Random.Range(1f, 2f);
		depth = depth_;
	}

	public void Grow (float energy, float sunAngle) 
	{
		// Check Energy
		photosynthesize = leaf.direction.z / 360 - sunAngle / (Mathf.PI * 2f);
		photosynthesize = Mathf.Abs(photosynthesize) * 360f;
		photosynthesize = photosynthesize < angleIndulgence ? (angleIndulgence - photosynthesize) / angleIndulgence : 0f;

		// Move Branch
		position += direction * photosynthesize;

		// Move Leaf
		leaf.MoveTo(position);

		// 
		leaf.Animate(photosynthesize);
	}

	public float Length ()
	{
		return Vector3.Distance(position, positionInitial) * length;
	}

	public void DetachLeaf ()
	{
		GameObject.Destroy(leaf.gameObject);
		leaf = null;
	}
}
