using UnityEngine;
using System.Collections;

public class Root 
{
	// GUI
	GameObject lineObject;
	GameObject leafObject;
	LineRenderer lineRenderer;

	// Logic
	Vector3 linePosition;
	Vector3 lineDirection;
	float lineAngle;
	float lineDistance;
	float lineSegmentDistance;
	float lineSegmentLength;
	int lineIndex;
	bool _isAlive;

	float randomSeed;

	// Getters
	public Vector3 LeafPosition {
		get {
			return this.linePosition + this.lineDirection * 0.5f;
		}
	}
	public Vector3 Position { get { return this.linePosition; } }
	public bool IsAlive { get { return this._isAlive; } set { this._isAlive = value; } }

	public void Randomize ()
	{
		this.randomSeed = Master.Instance.GlobalSpeed + Random.Range(1f, 1.5f);
	}

	public GameObject Create (Vector3 position, float angle) 
	{
		this.lineObject = new GameObject("Root");
		this.lineRenderer = this.lineObject.AddComponent<LineRenderer>();	
		this.lineRenderer.material = Master.Instance.MaterialRoot;

		this.linePosition = position;
		this.lineAngle = angle;
		this.lineDirection = new Vector3();
		this.lineDirection.x = Mathf.Cos(this.lineAngle);
		this.lineDirection.y = Master.Instance.RootAscensionScale;
		this.lineDirection.z = Mathf.Sin(this.lineAngle);

		this.leafObject = GameObject.Instantiate(Master.Instance.PrefabLeaf) as GameObject;
		this.leafObject.transform.position = this.linePosition;

		this.lineDistance = 0f;
		this.lineSegmentDistance = 0f;
		this.lineSegmentLength = Master.Instance.LineRendererSegmentLength;

		this.lineRenderer.SetWidth(Master.Instance.LineRendererStartWidth, Master.Instance.LineRendererEndWidth);
		this.lineRenderer.SetVertexCount(2);
		this.lineRenderer.SetPosition(0, this.linePosition);
		this.lineRenderer.SetPosition(1, this.linePosition);
		this.lineIndex = 1;

		this._isAlive = true;

		this.randomSeed = Master.Instance.GlobalSpeed + Random.Range(1f, 1.5f);

		return this.lineObject;
	}

	public void Grow (float speed)
	{
		this.lineDirection.x = Mathf.Cos(this.lineAngle);
		this.lineDirection.z = Mathf.Sin(this.lineAngle);
		this.linePosition += this.lineDirection * speed * this.randomSeed;

		this.lineDistance += this.lineDirection.magnitude * speed;
		this.lineSegmentDistance += this.lineDirection.magnitude * speed;
		if (this.lineSegmentDistance >= this.lineSegmentLength)
		{
			++this.lineIndex;
			this.lineRenderer.SetVertexCount(this.lineIndex + 1);
			this.lineRenderer.SetPosition(this.lineIndex, this.linePosition);
			this.lineSegmentDistance = 0f;
		}
		else
		{
			this.lineRenderer.SetPosition(this.lineIndex, this.linePosition);
		}

		// Leaf
		this.leafObject.transform.position = this.linePosition;
		Vector3 leafRotation = this.leafObject.transform.rotation.eulerAngles;
		leafRotation.y = -this.lineAngle * 180f / Mathf.PI + 90f;
		this.leafObject.transform.rotation = Quaternion.Euler(leafRotation);

		// Shader
		this.lineRenderer.material.SetFloat("_LineDistance", this.lineDistance);
	}

	public void Rotate (float angle)
	{
		this.lineAngle += angle;
	}

	public void Restarting (float ratio)
	{
		ratio = (1f - ratio);
		this.lineRenderer.SetWidth(ratio * Master.Instance.LineRendererStartWidth, ratio * Master.Instance.LineRendererEndWidth);
		this.leafObject.transform.localScale = Vector3.one * ratio;
	}

	public void Clean ()
	{
		GameObject.Destroy(this.lineObject);
		GameObject.Destroy(this.leafObject);
	}
}
