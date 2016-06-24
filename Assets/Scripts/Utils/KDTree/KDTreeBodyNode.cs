
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class KDBodyNode<T> : KDTreeNode<T>
{
    protected float mean;

    protected KDTreeNode<T>[] children = new KDTreeNode<T>[2];

    protected int Sort(int start, int length, List<KDTreeEntry<T>> nodes, IComparer<KDTreeEntry<T>> comparerer)
    {
        nodes.Sort(start, length, comparerer);

        return (int)(length / 2);
    }
}

public class KDBodyNodeX<T> : KDBodyNode<T>
{
    KDTreeSortXComparerer<T> comparerer = new KDTreeSortXComparerer<T>();

    public override int NearestNeighbor(Vector3 position)
    {
        int index = Convert.ToInt32(position.x >= mean);

        return children[index].NearestNeighbor(position);
    }

    public void Initialize(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {
        int meanIndex = Sort(start, length, nodes, comparerer);

        mean = nodes[start + meanIndex].position.x;

        if (length == 2 && (nodes[start].position.x != nodes[start + 1].position.x))
        {
            KDTreeLeafNode<T> leftLeafNode = tree.leafNodePol.Catch();

            leftLeafNode.Initialize(start);

            children[0] = leftLeafNode;

            KDTreeLeafNode<T> rightLeafNode = tree.leafNodePol.Catch();

            rightLeafNode.Initialize(start + 1);

            children[1] = rightLeafNode;

            return;
        }

        while (meanIndex > 0 && nodes[start + meanIndex - 1].position.x == nodes[start + meanIndex].position.x)
        {
            --meanIndex;
        }

        int leftEnd = start + meanIndex;
        int rightEnd = start + length;

        int leftStart = start;
        int leftLength = leftEnd - leftStart;

        int rightStart = leftEnd;
        int rightLength = rightEnd - rightStart;

        if(leftLength == 1)
        {
            KDTreeLeafNode<T> leafNode = tree.leafNodePol.Catch();

            leafNode.Initialize(leftStart);

            children[0] = leafNode;
        }
        else if (leftLength != 0)
        {
            KDBodyNodeY<T> leftBody = tree.bodyNodeYPool.Catch();

            leftBody.Initialize(leftStart, leftLength, nodes, tree);

            children[0] = leftBody;
        }

        

        if (rightLength == 1)
        {
            KDTreeLeafNode<T> leafNode = tree.leafNodePol.Catch();

            leafNode.Initialize(rightStart);

            children[1] = leafNode;
        }
        else if (rightLength != 0)
        {
            KDBodyNodeY<T> rightBody = tree.bodyNodeYPool.Catch();

            rightBody.Initialize(rightStart, rightLength, nodes, tree);

            children[1] = rightBody;
        }

        if (leftLength == 0)
        {
            children[0] = children[1];
        }

        if (rightLength == 0)
        {
            children[1] = children[0];
        }
    }
}

public class KDBodyNodeY<T> : KDBodyNode<T>
{
    KDTreeSortYComparerer<T> comparerer = new KDTreeSortYComparerer<T>();

    public override int NearestNeighbor(Vector3 position)
    {
        int index = Convert.ToInt32(position.y >= mean);

        return children[index].NearestNeighbor(position);
    }

    public void Initialize(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {
        // Local mean index
        int meanIndex = Sort(start, length, nodes, comparerer);

        mean = nodes[start + meanIndex].position.y;

        if (length == 2 && (nodes[start].position.y != nodes[start + 1].position.y))
        {
            KDTreeLeafNode<T> leftLeafNode = tree.leafNodePol.Catch();

            leftLeafNode.Initialize(start);

            children[0] = leftLeafNode;

            KDTreeLeafNode<T> rightLeafNode = tree.leafNodePol.Catch();

            rightLeafNode.Initialize(start + 1);

            children[1] = rightLeafNode;

            return;
        }
        // Shift the mean index left until all the duplicates are on the right
        while (meanIndex > 0 && nodes[start +  meanIndex - 1].position.y == nodes[start +  meanIndex].position.y)
        {
            --meanIndex;
        }

        int leftEnd = start + meanIndex;
        int rightEnd = start + length;

        int leftStart = start;
        int leftLength = leftEnd - leftStart;

        int rightStart = leftEnd;
        int rightLength = rightEnd - rightStart;

        if (leftLength == 1)
        {
            KDTreeLeafNode<T> leafNode = tree.leafNodePol.Catch();

            leafNode.Initialize(leftStart);

            children[0] = leafNode;
        }
        else if (leftLength != 0)
        {
            KDBodyNodeZ<T> leftBody = tree.bodyNodeZPool.Catch();

            leftBody.Initialize(leftStart, leftLength, nodes, tree);

            children[0] = leftBody;
        }


        if (rightLength == 1)
        {
            KDTreeLeafNode<T> leafNode = tree.leafNodePol.Catch();

            leafNode.Initialize(rightStart);

            children[1] = leafNode;
        }
        else if (rightLength != 0)
        {
            KDBodyNodeZ<T> rightBody = tree.bodyNodeZPool.Catch();

            rightBody.Initialize(rightStart, rightLength, nodes, tree);

            children[1] = rightBody;
        }

        if (leftLength == 0)
        {
            children[0] = children[1];
        }

        if (rightLength == 0)
        {
            children[1] = children[0];
        }
    }
}

public class KDBodyNodeZ<T> : KDBodyNode<T>
{
    KDTreeSortZComparerer<T> comparerer = new KDTreeSortZComparerer<T>();

    public override int NearestNeighbor(Vector3 position)
    {
        int index = Convert.ToInt32(position.z >= mean);

        return children[index].NearestNeighbor(position);
    }

    public void Initialize(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {

        int meanIndex = Sort(start, length, nodes, comparerer);

        mean = nodes[start + meanIndex].position.z;

        if (length == 2 && (nodes[start].position.z != nodes[start + 1].position.z))
        {
            KDTreeLeafNode<T> leftLeafNode = tree.leafNodePol.Catch();

            leftLeafNode.Initialize(start);

            children[0] = leftLeafNode;

            KDTreeLeafNode<T> rightLeafNode = tree.leafNodePol.Catch();

            rightLeafNode.Initialize(start + 1);

            children[1] = rightLeafNode;

            return;
        }

        while (meanIndex > 0 && nodes[start + meanIndex - 1].position.z == nodes[start +  meanIndex].position.z)
        {
            --meanIndex;
        }

        int leftEnd = start + meanIndex;
        int rightEnd = start + length;

        int leftStart = start;
        int leftLength = leftEnd - leftStart;

        int rightStart = leftEnd;
        int rightLength = rightEnd - rightStart;

        if (leftLength == 1)
        {
            KDTreeLeafNode<T> leafNode = tree.leafNodePol.Catch();

            leafNode.Initialize(leftStart);

            children[0] = leafNode;
        }
        else if (leftLength != 0)
        {
            KDBodyNodeX<T> leftBody = tree.bodyNodeXPool.Catch();

            leftBody.Initialize(leftStart, leftLength, nodes, tree);

            children[0] = leftBody;
        }



        if (rightLength == 1)
        {
            KDTreeLeafNode<T> leafNode = tree.leafNodePol.Catch();

            leafNode.Initialize(rightStart);

            children[1] = leafNode;
        }
        else if (rightLength != 0)
        {
            KDBodyNodeX<T> rightBody = tree.bodyNodeXPool.Catch();

            rightBody.Initialize(rightStart, rightLength, nodes, tree);

            children[1] = rightBody;
        }

        if (leftLength == 0)
        {
            children[0] = children[1];
        }

        if (rightLength == 0)
        {
            children[1] = children[0];
        }
    }
}
