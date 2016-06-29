using UnityEngine;
using System.Collections;

public class OctreeLeafNode<T> : OctreeNode<T>
{
    private OctreeEntry<T>[] children = new OctreeEntry<T>[8];

    public OctreeLeafNode<T> Initialize(Octree<T> treeBase, int xMinimum, int yMinimum, int zMinimum)
    {
        BaseInitialize(treeBase, xMinimum, yMinimum, zMinimum, 0);

        for (int i = 0; i < OctreeConstants.NUMBER_OF_CHILDREN; ++i)
        {
            children[i] = null;
        }

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

    public override OctreeEntry<T> SetAt(int x, int y, int z, T value)
    {
        int index = ChildRelativeTo(x, y, z);

        return SetChild(index, value);
    }

    public override void DrawGizmos()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (children[i] == null)
            {
                continue;
            }

            children[i].DrawGizmos(Color.blue);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.extents * 2);
    }

    protected OctreeEntry<T> SetChild(int index, T node)
    {
        // WHAT hapens if node is null? perform a remove operation @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        // There is no OctreeEntry at the index
        if (children[(int)index] == null)
        {
            // Fetch a new OctreeEntry
            OctreeEntry<T> fish = treeBase.entryPool.Catch();
 
            // Find the new child min
            int xMinimum, yMinimum, zMinimum;
            MinOfChildIndex(index, out xMinimum, out yMinimum, out zMinimum);
            // change the min from local to world space
            float minX = xMinimum * treeBase.leafDimensions.x;
            float minY = yMinimum * treeBase.leafDimensions.y;
            float minZ = zMinimum * treeBase.leafDimensions.z;
            // Initialize the new node with world space bounds
            fish.Initialize(node, xMinimum, yMinimum, zMinimum, new Vector3(minX, minY, minZ), new Vector3(minX + treeBase.leafDimensions.x, minY + treeBase.leafDimensions.y, minZ + treeBase.leafDimensions.z));
            // Set index to the child
            children[(int)index] = fish;
            // Increase the child counter
            ++childCount;
        }
        // Set the OctryEnty's value to what was passed
        children[(int)index].entry = node;

        return children[(int)index];
    }
}
