
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class KDBodyNode<T> : KDTreeNode<T>
{
    public const int LEFT_INDEX = 0;
    public const int RIGHT_INDEX = 1;

    public const int BODY_NODE_INDEX = 0;
    public const int LEAF_NODE_INDEX = 1;
    public const int TEMP_NODE_INDEX = 2;

    protected float mean;

    protected KDTreeNode<T>[] children = new KDTreeNode<T>[2];

    protected ChooseNodePointer[] nodeFunctions = new ChooseNodePointer[3];

    public KDBodyNode()
    {
        nodeFunctions[BODY_NODE_INDEX] = new ChooseNodePointer(ChooseBody);
        nodeFunctions[LEAF_NODE_INDEX] = new ChooseNodePointer(ChooseLeaf);
        nodeFunctions[TEMP_NODE_INDEX] = new ChooseNodePointer(ChooseTemp);
    }

    protected int Sort(int start, int length, List<KDTreeEntry<T>> nodes, IComparer<KDTreeEntry<T>> comparerer)
    {
        nodes.Sort(start, length, comparerer);

        return start + (int)(length / 2);
    }

    protected void CreateSubNodes(int leftStart, int leftLength, int rightStart, int rightLength, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {
        int isLeftLengthZero = Convert.ToInt32(leftLength == 0);

        int isRightLengthZero = Convert.ToInt32(rightLength == 0);

        // Left Node

        KDTreeNode<T> leftNode;

        int leftNodeIndex = Convert.ToInt32(leftLength == 1) + (isLeftLengthZero * TEMP_NODE_INDEX);

        leftNode = nodeFunctions[leftNodeIndex](tree);

        leftNode.Initialize(leftStart, leftLength, nodes, tree);

        children[LEFT_INDEX] = leftNode;

        // Right node

        KDTreeNode<T> rightNode;

        int rightNodeIndex = Convert.ToInt32(rightLength == 1) + (isRightLengthZero * TEMP_NODE_INDEX);

        rightNode = nodeFunctions[rightNodeIndex](tree);

        rightNode.Initialize(rightStart, rightLength, nodes, tree);

        children[RIGHT_INDEX] = rightNode;

        // Share Node

        children[LEFT_INDEX] = children[isLeftLengthZero];

        children[isRightLengthZero] = children[LEFT_INDEX];
    }

    protected abstract KDTreeNode<T> ChooseLeaf(KDTree<T> tree);

    protected abstract KDTreeNode<T> ChooseBody(KDTree<T> tree);

    protected abstract KDTreeNode<T> ChooseTemp(KDTree<T> tree);

    protected delegate KDTreeNode<T> ChooseNodePointer(KDTree<T> tree);
}

public class KDBodyNodeX<T> : KDBodyNode<T>
{
    KDTreeSortXComparerer<T> comparerer = new KDTreeSortXComparerer<T>();

    public KDBodyNodeX()
    {

    }

    public override int NearestNeighbor(Vector3 position)
    {
        int index = Convert.ToInt32(position.x >= mean);

        return children[index].NearestNeighbor(position);
    }

    protected override KDTreeNode<T> ChooseLeaf(KDTree<T> tree)
    {
        return tree.leafNodePol.Catch();
    }

    protected override KDTreeNode<T> ChooseBody(KDTree<T> tree)
    {
        return tree.bodyNodeYPool.Catch();
    }

    protected override KDTreeNode<T> ChooseTemp(KDTree<T> tree)
    {
        return tree.tempNodePool.Catch();
    }

    public override void Initialize(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {
        int meanIndex = Sort(start, length, nodes, comparerer);

        mean = nodes[meanIndex].position.x;

        if (length == 2 && (nodes[start].position.x != nodes[start + 1].position.x))
        {
            KDTreeNode<T> leftLeafNode = nodeFunctions[LEAF_NODE_INDEX](tree);

            leftLeafNode.Initialize(start, 0, null, null);

            children[0] = leftLeafNode;

            KDTreeNode<T> rightLeafNode = nodeFunctions[LEAF_NODE_INDEX](tree);

            rightLeafNode.Initialize(start + 1, 0, null, null);

            children[1] = rightLeafNode;

            return;
        }

        while (meanIndex > start && nodes[meanIndex - 1].position.x == nodes[meanIndex].position.x)
        {
            --meanIndex;
        }

        int leftEnd = meanIndex;
        int rightEnd = start + length;

        int leftStart = start;
        int leftLength = leftEnd - leftStart;

        int rightStart = leftEnd;
        int rightLength = rightEnd - rightStart;

        CreateSubNodes(leftStart, leftLength, rightStart, rightLength, nodes, tree);
    }
}

public class KDBodyNodeY<T> : KDBodyNode<T>
{
    KDTreeSortYComparerer<T> comparerer = new KDTreeSortYComparerer<T>();

    public KDBodyNodeY()
    {

    }

    protected override KDTreeNode<T> ChooseLeaf(KDTree<T> tree)
    {
        return tree.leafNodePol.Catch();
    }

    protected override KDTreeNode<T> ChooseBody(KDTree<T> tree)
    {
        return tree.bodyNodeZPool.Catch();
    }

    protected override KDTreeNode<T> ChooseTemp(KDTree<T> tree)
    {
        return tree.tempNodePool.Catch();
    }

    public override int NearestNeighbor(Vector3 position)
    {
        int index = Convert.ToInt32(position.y >= mean);

        return children[index].NearestNeighbor(position);
    }

    public override void Initialize(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {
        // Local mean index
        int meanIndex = Sort(start, length, nodes, comparerer);

        mean = nodes[meanIndex].position.y;

        if (length == 2 && (nodes[start].position.y != nodes[start + 1].position.y))
        {
            KDTreeNode<T> leftLeafNode = nodeFunctions[LEAF_NODE_INDEX](tree);

            leftLeafNode.Initialize(start, 0, null, null);

            children[0] = leftLeafNode;

            KDTreeNode<T> rightLeafNode = nodeFunctions[LEAF_NODE_INDEX](tree);

            rightLeafNode.Initialize(start + 1, 0, null, null);

            children[1] = rightLeafNode;

            return;
        }
        // Shift the mean index left until all the duplicates are on the right
        while (meanIndex > start && nodes[meanIndex - 1].position.y == nodes[ meanIndex].position.y)
        {
            --meanIndex;
        }

        int leftEnd = meanIndex;
        int rightEnd = start + length;

        int leftStart = start;
        int leftLength = leftEnd - leftStart;

        int rightStart = leftEnd;
        int rightLength = rightEnd - rightStart;

        CreateSubNodes(leftStart, leftLength, rightStart, rightLength, nodes, tree);
    }
}

public class KDBodyNodeZ<T> : KDBodyNode<T>
{
    KDTreeSortZComparerer<T> comparerer = new KDTreeSortZComparerer<T>();

    public KDBodyNodeZ()
    {

    }

    protected override KDTreeNode<T> ChooseLeaf(KDTree<T> tree)
    {
        return tree.leafNodePol.Catch();
    }

    protected override KDTreeNode<T> ChooseBody(KDTree<T> tree)
    {
        return tree.bodyNodeXPool.Catch();
    }

    protected override KDTreeNode<T> ChooseTemp(KDTree<T> tree)
    {
        return tree.tempNodePool.Catch();
    }

    public override int NearestNeighbor(Vector3 position)
    {
        int index = Convert.ToInt32(position.z >= mean);

        return children[index].NearestNeighbor(position);
    }

    public override void Initialize(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {

        int meanIndex = Sort(start, length, nodes, comparerer);

        mean = nodes[meanIndex].position.z;

        if (length == 2 && (nodes[start].position.z != nodes[start + 1].position.z))
        {
            KDTreeNode<T> leftLeafNode = nodeFunctions[LEAF_NODE_INDEX](tree);

            leftLeafNode.Initialize(start, 0, null, null);

            children[0] = leftLeafNode;

            KDTreeNode<T> rightLeafNode = nodeFunctions[LEAF_NODE_INDEX](tree);

            rightLeafNode.Initialize(start + 1, 0, null, null);

            children[1] = rightLeafNode;

            return;
        }

        while (meanIndex > start && nodes[meanIndex - 1].position.z == nodes[meanIndex].position.z)
        {
            --meanIndex;
        }

        int leftEnd = meanIndex;
        int rightEnd = start + length;

        int leftStart = start;
        int leftLength = leftEnd - leftStart;

        int rightStart = leftEnd;
        int rightLength = rightEnd - rightStart;

        CreateSubNodes(leftStart, leftLength, rightStart, rightLength, nodes, tree);
    }
}
