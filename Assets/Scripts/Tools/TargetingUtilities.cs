using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetingUtilities {



    public static Quaternion SmoothRotation(Vector3 targetPos, Transform myTransform, float rotateSpeed, float error = 0f) {
        Quaternion newRotation = new Quaternion();

        Vector2 direction = (targetPos - myTransform.position);

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f + error;

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        newRotation = Quaternion.Slerp(myTransform.rotation, q, Time.deltaTime * rotateSpeed);

        return newRotation;
    }

    public static float GetRotationAngle(Vector3 targetPos, Transform myTransform) {

        Vector2 direction = (targetPos - myTransform.position);

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;

        return angle;
    }

    public static Vector2 DegreeToVector2(float degree) {
        return (Vector2)(Quaternion.Euler(0f, 0f, degree) * Vector2.right);
    }

    public static Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal) {
        //if (angleIsGlobal == false) {
        //    angleInDegrees += transform.eulerAngles.z + 90;
        //}

        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }




}
