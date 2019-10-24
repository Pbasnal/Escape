using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct NumberPair
{
    public int x, y;
    public NumberPair(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public struct BoundingBox
{
    public NumberPair top, bottom;

    public BoundingBox(NumberPair point1, NumberPair point2)
    {
        if (point1.y < point2.y)
        {
            top = point1;
            bottom = point2;
        }
        else
        {
            top = point2;
            bottom = point1;
        }
    }
}

public class BSPNode
{
    public BSPNode left;
    public BSPNode right;

    public BoundingBox area;
}

public class BSPTree
{
    private int[,] space;
    private BSPNode root;

    public BSPTree(int[,] space)
    {
        this.space = space;
    }

    public void GenerateRandomTree()
    {
        root = new BSPNode();
        root.area = new BoundingBox(new NumberPair(0, 0), new NumberPair(space.GetLength(0), space.GetLength(1)));

        Queue<BSPNode> splittingOrder = new Queue<BSPNode>();
        splittingOrder.Enqueue(root);


    }
}
