using System.Collections.Generic;
using UnityEngine;

public interface Grid3DRaycastSelector<T>
{
    void Select(Ray ray, Grid3D<T> brick, Vector3 brickPosition, List<T> whiteList, PriorityQueue<Vector3i, float> out_found);
}
