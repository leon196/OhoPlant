using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour
{
	private float timeStartGrow;
	private float timeDelayGrow = 2f;

	public void Create (Vector3 position)
	{
		this.transform.position = position;
		this.transform.localScale = Vector3.zero;
		this.timeStartGrow = Time.time;
		StartCoroutine(this.Grow());
	}

	IEnumerator Grow ()
	{
		float growRatio = 0f;
		while (growRatio < 1f) 
		{
			growRatio = Master.Instance.AnimationRatio(this.timeStartGrow, this.timeDelayGrow);
			this.transform.localScale = Vector3.one * growRatio;
			yield return null;
		}
	}

	public void Restarting (float ratio)
	{
		this.transform.localScale = Vector3.one * (1f - ratio);
	}

	public void Clean ()
	{
		GameObject.Destroy(this.gameObject);
	}
}
