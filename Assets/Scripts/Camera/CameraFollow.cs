using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [Header("Follow Target")]
    public Controller2D target;

    [Header("Focus Area")]
    public Vector2 focusAreaSize;

    [Header("Movement")]
    public float verticalOffset;
    public float lookAheadDistanceX;
    public float lookSmoothTimeX;
    public float lookSmoothTimeY;

    [Header("Bounds")]
    public Vector2 maxXPos;
    public Vector2 minXPos;
    public Vector2 maxYPos;
    public Vector2 minYPos;

    private FocusArea focusArea;
    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirectionX;
    private float smoothLookVelocityX;
    private float smoothLookVelocityY;

    private bool lookAheadStopped;

    private void Start() {
        focusArea = new FocusArea(target.BoxCollider.bounds, focusAreaSize);
    }

    private void LateUpdate() {
        focusArea.Update(target.BoxCollider.bounds);

        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

        if(focusArea.velocity.x != 0) {
            lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);
            if(Mathf.Sign(target.PlayerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.PlayerInput.x != 0f) {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
            }
            else {
                if(lookAheadStopped == false) {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX) / 4;
                }
            }
        }

        
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothLookVelocityY, lookSmoothTimeY);
        focusPosition += Vector2.right * currentLookAheadX;

        if(focusPosition.y > maxYPos.y) 
            focusPosition.y = maxYPos.y;
        
        if (focusPosition.y < minYPos.y)
            focusPosition.y = minYPos.y;

        if (focusPosition.x > maxXPos.x)
            focusPosition.x = maxXPos.x;

        if (focusPosition.x < minXPos.x)
            focusPosition.x = minXPos.x;

        transform.position = (Vector3)focusPosition + Vector3.forward * -10f;
    }


    private void OnDrawGizmos() {
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }


    private struct FocusArea {
        public Vector2 center;
        public Vector2 velocity;

        private float left, right;
        private float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size) {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds) {
            float shiftX = 0;

            if(targetBounds.min.x < left) {
                shiftX = targetBounds.min.x - left;
            }else if(targetBounds.max.x > right) {
                shiftX = targetBounds.max.x - right;
            }

            left += shiftX;
            right += shiftX;

            float shiftY = 0;

            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }

            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }

    }

}
