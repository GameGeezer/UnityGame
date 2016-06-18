
using System.Collections.Generic;

class BrickPool
{
    private Stack<Brick> pool = new Stack<Brick>();
    private Vector3i brickResolution;

    public BrickPool(Vector3i brickResolution)
    {
        this.brickResolution = brickResolution;
    }

    public Brick Catch()
    {
        Brick fish;
        lock (pool)
        {
            fish = pool.Count > 0 ? pool.Pop() : new Brick(brickResolution.x, brickResolution.y, brickResolution.z);
        }

        return fish;
    }

    public void Release(Brick fish)
    {
        lock(pool)
        {
            pool.Push(fish);
        }
    }
}
