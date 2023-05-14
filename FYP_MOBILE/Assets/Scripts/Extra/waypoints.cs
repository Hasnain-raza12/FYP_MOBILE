using UnityEngine;

public class waypoints : MonoBehaviour
{
	public float GizmoSize;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(base.transform.position, GizmoSize);
	}
}
