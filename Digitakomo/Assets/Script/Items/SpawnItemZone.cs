using UnityEngine;

public class SpawnItemZone : MonoBehaviour
{
    public Vector3 center;
    public Vector3 size;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0);
        Gizmos.DrawWireCube(transform.localPosition + center, size);
    }
}
