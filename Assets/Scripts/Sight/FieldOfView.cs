using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    [Header("View Paramaters")]
    public float viewRadius;
    [Range(0f, 360f)]
    public float viewAngle;
    

    [Header("Masks")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("Mesh")]
    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDistanceThreshold;
    public float maskCutawayDistance = 0.1f;

    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    [HideInInspector]
    public List<Transform> visableTargets = new List<Transform>();

    private void Awake() {
        viewMeshFilter = GetComponentInChildren<MeshFilter>();
    }

    private void Start() {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    private void LateUpdate() {
        DrawFieldOfView();
    }


    private IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisableTargets();
        }
    }

    private void FindVisableTargets() {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        visableTargets.Clear();
        int count = targetsInViewRadius.Length;

        for (int i = 0; i < count; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 direcitonToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, direcitonToTarget) < viewAngle / 2) {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direcitonToTarget, distanceToTarget, obstacleMask);

                if (hit.collider == null) {
                    visableTargets.Add(target);
                }

            }
        }
    }

    private void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector2> viewPoints = new List<Vector2>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++) {
            float angle = (transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i) + 90f;
            //Debug.DrawLine(transform.position, transform.position + (Vector3)DirectionFromAngle(angle, true) * viewRadius, Color.red);
            ViewCastInfo newViewCast = ViewCast(angle);

            if(i > 0) {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if(oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector2.zero)
                        viewPoints.Add(edge.pointA);

                    if (edge.pointB != Vector2.zero)
                        viewPoints.Add(edge.pointB);
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] verticies = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        verticies[0] = Vector3.zero;
        normals[0] = new Vector3(0f, 0f, -1f);
        int count = vertexCount - 1;

        for (int i = 0; i < count; i++) {
            verticies[i + 1] = transform.InverseTransformPoint(viewPoints[i])/* + (Vector3)(Vector2.up * maskCutawayDistance)*/;
            normals[i + 1] = new Vector3(0f, 0f, -1f);

            if (i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        System.Array.Reverse(triangles);
        viewMesh.Clear();

        viewMesh.vertices = verticies;
        viewMesh.normals = normals;
        viewMesh.triangles = triangles;
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;

        Vector2 minPoint = Vector2.zero;
        Vector2 maxPoint = Vector2.zero;

        for(int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && edgeDistanceThresholdExceeded == false) {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }

        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    private ViewCastInfo ViewCast(float globalAngle) {
        Vector2 direction = DirectionFromAngle(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewRadius, obstacleMask);

        if (hit.collider != null) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else {
            return new ViewCastInfo(false, (Vector2)transform.position + direction * viewRadius, viewRadius, globalAngle);
        }
    }


    public Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (angleIsGlobal == false) {
            angleInDegrees += transform.eulerAngles.z + 90;
        }

        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }


    public struct ViewCastInfo {
        public bool hit;
        public Vector2 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector2 point, float distance, float angle) {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }

    public struct EdgeInfo {
        public Vector2 pointA;
        public Vector2 pointB;

        public EdgeInfo(Vector2 pointA, Vector2 pointB) {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }


}
