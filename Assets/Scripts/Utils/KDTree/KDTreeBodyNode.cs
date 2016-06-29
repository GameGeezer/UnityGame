
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
    protected CreateNodes[] creatNodeFunction = new CreateNodes[2];

    public KDBodyNode()
    {
        nodeFunctions[BODY_NODE_INDEX] = new ChooseNodePointer(ChooseBody);
        nodeFunctions[LEAF_NODE_INDEX] = new ChooseNodePointer(ChooseLeaf);
        nodeFunctions[TEMP_NODE_INDEX] = new ChooseNodePointer(ChooseTemp);

        creatNodeFunction[0] = new CreateNodes(CreateNodesBodyNext);
        creatNodeFunction[1] = new CreateNodes(CreateNodesTwoDifferent);
    }

    public override void Initialize(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree)
    {
        nodes.Sort(start, length, GetComparer());

        int meanIndex = start + (int)(length / 2);

        mean = nodes[meanIndex].position.z;

        int areTwoAndDifferent = Convert.ToInt32(IsThereTwoDifferent(start, length, nodes));

        creatNodeFunction[areTwoAndDifferent](start, length, nodes, tree, meanIndex);
    }

    protected void CreateNodesTwoDifferent(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree, int meanIndex)
    {
        KDTreeNode<T> leftLeafNode = nodeFunctions[LEAF_NODE_INDEX](tree);

        leftLeafNode.Initialize(start, 0, null, null);

        children[0] = leftLeafNode;

        KDTreeNode<T> rightLeafNode = nodeFunctions[LEAF_NODE_INDEX](tree);

        rightLeafNode.Initialize(start + 1, 0, null, null);

        children[1] = rightLeafNode;

        return;
    }

    protected abstract void CreateNodesBodyNext(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree, int meanIndex);

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

    protected abstract bool IsThereTwoDifferent(int start, int length, List<KDTreeEntry<T>> nodes);

    protected abstract IComparer<KDTreeEntry<T>> GetComparer();

    protected abstract KDTreeNode<T> ChooseLeaf(KDTree<T> tree);

    protected abstract KDTreeNode<T> ChooseBody(KDTree<T> tree);

    protected abstract KDTreeNode<T> ChooseTemp(KDTree<T> tree);

    protected delegate KDTreeNode<T> ChooseNodePointer(KDTree<T> tree);

    protected delegate void CreateNodes(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree, int meanIndex);
}

public class KDBodyNodeX<T> : KDBodyNode<T>
{
    private static KDTreeSortXComparerer<T> comparerer = new KDTreeSortXComparerer<T>();

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
        return tree.poolParty.leafNodePool.Catch();
    }

    protected override KDTreeNode<T> ChooseBody(KDTree<T> tree)
    {
        return tree.poolParty.bodyNodeYPool.Catch();
    }

    protected override KDTreeNode<T> ChooseTemp(KDTree<T> tree)
    {
        return tree.TEMP_INSTANCE;
    }

    protected override bool IsThereTwoDifferent(int start, int length, List<KDTreeEntry<T>> nodes)
    {
        return length == 2 && (nodes[start].position.x != nodes[start + 1].position.x);
    }

    protected override IComparer<KDTreeEntry<T>> GetComparer()
    {
        return comparerer;
    }

    public override void Clear(KDTreePoolParty<T> poolParty)
    {
        children[LEFT_INDEX].Clear(poolParty);

        children[RIGHT_INDEX].Clear(poolParty);

        poolParty.bodyNodeXPool.Release(this);

        mean = 0.0f;
    }

    protected override void CreateNodesBodyNext(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree, int meanIndex)
    {
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
    private static KDTreeSortYComparerer<T> comparerer = new KDTreeSortYComparerer<T>();

    public KDBodyNodeY()
    {

    }

    protected override KDTreeNode<T> ChooseLeaf(KDTree<T> tree)
    {
        return tree.poolParty.leafNodePool.Catch();
    }

    protected override KDTreeNode<T> ChooseBody(KDTree<T> tree)
    {
        return tree.poolParty.bodyNodeZPool.Catch();
    }

    protected override KDTreeNode<T> ChooseTemp(KDTree<T> tree)
    {
        return tree.TEMP_INSTANCE;
    }

    protected override bool IsThereTwoDifferent(int start, int length, List<KDTreeEntry<T>> nodes)
    {
        return length == 2 && (nodes[start].position.y != nodes[start + 1].position.y);
    }

    public override int NearestNeighbor(Vector3 position)
    {
        int index = Convert.ToInt32(position.y >= mean);

        return children[index].NearestNeighbor(position);
    }

    protected override IComparer<KDTreeEntry<T>> GetComparer()
    {
        return comparerer;
    }

    public override void Clear(KDTreePoolParty<T> poolParty)
    {
        children[LEFT_INDEX].Clear(poolParty);

        children[RIGHT_INDEX].Clear(poolParty);

        poolParty.bodyNodeYPool.Release(this);

        mean = 0.0f;
    }

    protected override void CreateNodesBodyNext(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree, int meanIndex)
    {
        while (meanIndex > start && nodes[meanIndex - 1].position.y == nodes[meanIndex].position.y)
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
    private static KDTreeSortZComparerer<T> comparerer = new KDTreeSortZComparerer<T>();

    public KDBodyNodeZ()
    {

    }

    protected override KDTreeNode<T> ChooseLeaf(KDTree<T> tree)
    {
        return tree.poolParty.leafNodePool.Catch();
    }

    protected override KDTreeNode<T> ChooseBody(KDTree<T> tree)
    {
        return tree.poolParty.bodyNodeXPool.Catch();
    }

    protected override KDTreeNode<T> ChooseTemp(KDTree<T> tree)
    {
        return tree.TEMP_INSTANCE;
    }

    protected override bool IsThereTwoDifferent(int start, int length, List<KDTreeEntry<T>> nodes)
    {
        return length == 2 && (nodes[start].position.z != nodes[start + 1].position.z);
    }

    protected override IComparer<KDTreeEntry<T>> GetComparer()
    {
        return comparerer;
    }

    public override int NearestNeighbor(Vector3 position)
    {
        int index = Convert.ToInt32(position.z >= mean);

        return children[index].NearestNeighbor(position);
    }

    public override void Clear(KDTreePoolParty<T> poolParty)
    {
        children[LEFT_INDEX].Clear(poolParty);

        children[RIGHT_INDEX].Clear(poolParty);

        poolParty.bodyNodeZPool.Release(this);

        mean = 0.0f;
    }

    protected override void CreateNodesBodyNext(int start, int length, List<KDTreeEntry<T>> nodes, KDTree<T> tree, int meanIndex)
    {
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
