
using System.Collections.Generic;

class BrickPool
{
    private Stack<Brick> pool = new Stack<Brick>();
    private Vector3i brickDimensions;

    public BrickPool(Vector3i brickDimensions)
    {
        this.brickDimensions = brickDimensions;
    }

    public Brick Catch()
    {
        Brick fish = pool.Count > 0 ? pool.Pop() : new Brick(brickDimensions.x, brickDimensions.y, brickDimensions.z);

        return fish;
    }

    public void Release(Brick fish)
    {
        pool.Push(fish);
    }
}
