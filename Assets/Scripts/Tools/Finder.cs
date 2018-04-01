using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Finder {


    public static Transform GetNearestPoint(List<Transform> points, Vector2 location) {
        Dictionary<Transform, float> distanceDict = new Dictionary<Transform, float>();

        for (int i = 0; i < points.Count; i++) {
            float distance = Vector2.Distance(location, points[i].position);

            distanceDict.Add(points[i], distance);

        }


        Transform minPos = distanceDict.OrderBy(kvp => kvp.Value).First().Key;


        return minPos;
    }




}
