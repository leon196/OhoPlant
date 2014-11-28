using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Branch : Element
{
	private Leaf leaf;
	public float photosynthesize;

	private Vector3 positionInitial;
	public int depth;

	public Branch (Vector3 position_, Vector3 direction_, int depth_)
	{
		position = position_;
		direction = direction_.normalized;

		positionInitial = position;

		leaf = new Leaf(position, Manager.Instance.GetRandomLeafDirection());
		photosynthesize = 0f;

		// L-System Node Level
		depth = depth_;
	}
	
	public void Grow (float energy, float sunAngle) 
	{
		// Check Energy
		photosynthesize = leaf.direction.z / 360 - sunAngle / (Mathf.PI * 2f);
		photosynthesize = Mathf.Abs(photosynthesize) * 360f;
		photosynthesize = photosynthesize < 10f ? (10f - photosynthesize) / 10f : 0f;

		// Move Branch
		position += direction * photosynthesize;

		// Move Leaf
		leaf.MoveTo(position);

		// 
		leaf.Animate(photosynthesize);
	}

	public float Length ()
	{
		return Vector3.Distance(position, positionInitial);
	}

	public void DetachLeaf ()
	{
		GameObject.Destroy(leaf.gameObject);
		leaf = null;
	}
}
