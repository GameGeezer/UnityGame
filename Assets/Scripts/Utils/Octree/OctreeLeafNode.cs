using UnityEngine;
using System.Collections;

public class OctreeLeafNode<T> : OctreeNode<T>
{
    private OctreeEntry<T>[] children = new OctreeEntry<T>[8];

    public OctreeLeafNode<T> ReInitialize(Octree<T> treeBase, Vector3i min)
    {
        Initialize(treeBase, min, 0);

        return this;
    }

    public override void RaycastFind(Ray ray, PriorityQueue<OctreeEntry<T>, float> found)
    {
        // Does the ray intersect this node's bounds?
        if (!worldBounds.IntersectRay(ray))
        {
            return;
        }

        for (int i = 0; i < 8; ++i)
        {
            float distance;
            // Is there a child that the ray intersects?
            if (children[i] == null || !children[i].IntersectRay(ray, out distance))
            {
                continue;
            }
            // Entry found, add it to the priority queue
            found.Enqueue(children[i], BrickConstants.LARGE_FLOAT - distance);
        }
    }

    public override OctreeEntry<T> GetAt(Vector3i point)
    {
        OctreeChild index = ChildRelativeTo(point);

        return children[(int)index];
    }

    public override OctreeEntry<T> SetAt(Vector3i point, T value)
    {
        OctreeChild index = ChildRelativeTo(point);

        return SetChild(index, value);
    }

    public override bool RemoveAt(Vector3i point, out T entry)
    {
        OctreeChild index = ChildRelativeTo(point);
        // There is no child at the index
        if (children[(int)index] == null)
        {
            entry = default(T);

            return HasChildren();
        }

        entry = children[(int)index].entry;

        RemoveChild(index);

        return HasChildren();
    }

    public override void Draw()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (children[i] == null)
            {
                continue;
            }

            children[i].DrawWireFrame(Color.blue);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.extents * 2);
    }

    protected OctreeEntry<T> SetChild(OctreeChild index, T node)
    {
        // WHAT hapens if node is null? perform a remove operation @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        // There is no OctreeEntry at the index
        if (children[(int)index] == null)
        {
            // Fetch a new OctreeEntry
            OctreeEntry<T> fish = treeBase.entryPool.Catch();
            // Find the min of the new entry
            Vector3i min = MinOfChildIndex((int)index);
            // change the min from local to world space
            float minX = min.x * treeBase.leafDimensions.x;
            float minY = min.y * treeBase.leafDimensions.y;
            float minZ = min.z * treeBase.leafDimensions.z;
            // Initialize the new node with world space bounds
            fish.ReInitialize(node, min.x, min.y, min.z, new Vector3(minX, minY, minZ), new Vector3(minX + treeBase.leafDimensions.x, minY + treeBase.leafDimensions.y, minZ + treeBase.leafDimensions.z));
            // Set index to the child
            children[(int)index] = fish;
            // Increase the child counter
            ++childCount;
        }
        // Set the OctryEnty's value to what was passed
        children[(int)index].entry = node;

        return children[(int)index];
    }

    protected void RemoveChild(OctreeChild index)
    {
        // Decrement the number of children if an entry is affected
        if (children[(int)index] != null && children[(int)index].entry != null)
        {
            --childCount;
        }

        children[(int)index].Clean();

        treeBase.entryPool.Release(children[(int)index]);

        children[(int)index] = null;
    }
}
