using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {

    protected const float SKIN_WIDTH = 0.015f;
    protected const float DISTANCE_BETWEEN_RAYS = 0.25f;

    [Header("Mask")]
    public LayerMask collisionMask;



    //[Header("Ray Count")]
    protected int horizontalRayCount;
    protected int verticalRayCount;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    public BoxCollider2D BoxCollider { get; protected set; }
    protected RaycastOrigins raycastOrigins;

    protected virtual void Awake() {
        BoxCollider = GetComponent<BoxCollider2D>();
        
    }

    protected virtual void Start() {
        CalculateRaySpacing();
    }


    protected void UpdateRaycastOrigins() {
        Bounds bounds = GetBounds();

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    protected void CalculateRaySpacing() {
        Bounds bounds = GetBounds();

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / DISTANCE_BETWEEN_RAYS);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / DISTANCE_BETWEEN_RAYS);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    protected Bounds GetBounds() {
        Bounds bounds = BoxCollider.bounds;
        bounds.Expand(SKIN_WIDTH * -2);

        return bounds;
    }

    protected float GetSign(float value) {
        return Mathf.Sign(value);
    }

    protected struct RaycastOrigins {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }
}
