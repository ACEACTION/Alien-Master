using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;
    [SerializeField] LayerMask deadEnemyMask;

	//public List<Transform> visibleTargets = new List<Transform>();
	public Transform player;
	public Transform deadEnemy;

	public float meshResolution;
	public int edgeResolveIterations;
	public float edgeDstThreshold;

	public MeshFilter viewMeshFilter;
	Mesh viewMesh;

	void Start()
	{
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;

		StartCoroutine("FindTargetsWithDelay", .2f);
	}


	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
			FindDeadEnemy();
		}
	}

	void LateUpdate()
	{
		DrawFieldOfView();
	}


	Collider[] playerInViewRadius;
	Transform pl;
	Vector3 dirToPlayer;
	float dstToPlayer;
	void FindVisibleTargets()
	{
		//visibleTargets.Clear();
		player = null;
		playerInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for (int i = 0; i < playerInViewRadius.Length; i++)
		{
			pl = playerInViewRadius[i].transform;
			dirToPlayer = (pl.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
			{
				dstToPlayer = Vector3.Distance(transform.position, pl.position);
				if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
				{
					//visibleTargets.Add(target);
					this.player = pl;
				}
			}
		}
	}


	Collider[] deadEnemiesInViewRadius;
	Transform enemy;
	Vector3 dirToDeadEnemy;
	float dstToDeadEnemy;
	void FindDeadEnemy()
	{
		//visibleTargets.Clear();
		deadEnemy = null;
		deadEnemiesInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, deadEnemyMask);

		for (int i = 0; i < deadEnemiesInViewRadius.Length; i++)
		{
			enemy = deadEnemiesInViewRadius[i].transform;
			dirToDeadEnemy = (enemy.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToDeadEnemy) < viewAngle / 2)
			{
				dstToDeadEnemy = Vector3.Distance(transform.position, enemy.position);
				if (!Physics.Raycast(transform.position, dirToDeadEnemy, dstToDeadEnemy, obstacleMask))
				{
					//visibleTargets.Add(target);
					deadEnemy = enemy;
				}
			}
		}
	}

	int stepCount;
	float stepAngleSize;
	List<Vector3> viewPoints;
	ViewCastInfo oldViewCast;
	float angle;
	ViewCastInfo drawFowNewViewCast;
	bool drawFowDstThresholdExceeded;
	EdgeInfo edge;
	int vertexCount;
	Vector3[] vertices;
	int[] triangles;
	void DrawFieldOfView()
	{
		stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		stepAngleSize = viewAngle / stepCount;
		viewPoints = new List<Vector3>();
		oldViewCast = new ViewCastInfo();
		for (int i = 0; i <= stepCount; i++)
		{
			angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			drawFowNewViewCast = ViewCast(angle);

			if (i > 0)
			{
				drawFowDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - drawFowNewViewCast.dst) > edgeDstThreshold;
				if (oldViewCast.hit != drawFowNewViewCast.hit || (oldViewCast.hit && drawFowNewViewCast.hit && drawFowDstThresholdExceeded))
				{
					edge = FindEdge(oldViewCast, drawFowNewViewCast);
					if (edge.pointA != Vector3.zero)
					{
						viewPoints.Add(edge.pointA);
					}
					if (edge.pointB != Vector3.zero)
					{
						viewPoints.Add(edge.pointB);
					}
				}

			}


			viewPoints.Add(drawFowNewViewCast.point);
			oldViewCast = drawFowNewViewCast;
		}

		vertexCount = viewPoints.Count + 1;
		vertices = new Vector3[vertexCount];
		triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

			if (i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear();

		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}


	float minAngle;
	float maxAngle;
	Vector3 minPoint;
	Vector3 maxPoint;
	float edgeAngle;
	ViewCastInfo edgeNewViewCast;
	EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
	{
		minAngle = minViewCast.angle;
		maxAngle = maxViewCast.angle;
		minPoint = Vector3.zero;
		maxPoint = Vector3.zero;
		bool edgeDstThresholdExceeded;
		for (int i = 0; i < edgeResolveIterations; i++)
		{
			edgeAngle = (minAngle + maxAngle) / 2;
			edgeNewViewCast = ViewCast(edgeAngle);

			edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - edgeNewViewCast.dst) > edgeDstThreshold;
			if (edgeNewViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
			{
				minAngle = edgeAngle;
				minPoint = edgeNewViewCast.point;
			}
			else
			{
				maxAngle = edgeAngle;
				maxPoint = edgeNewViewCast.point;
			}
		}

		return new EdgeInfo(minPoint, maxPoint);
	}


	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
		{
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
		}
	}

	public struct EdgeInfo
	{
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
		{
			pointA = _pointA;
			pointB = _pointB;
		}
	}

}
