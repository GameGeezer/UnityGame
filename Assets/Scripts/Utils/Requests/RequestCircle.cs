using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RequestCircle
{
    private Queue<RequestHandler> pool = new Queue<RequestHandler>();

    public RequestCircle()
    {

    }

    public RequestCircle(int handlerCount)
    {
        for (int i = 0; i < handlerCount; ++i)
        {
            Add(new RequestHandler());
        }
    }

    public RequestHandler Grab()
    {
        RequestHandler fish = pool.Dequeue();
        pool.Enqueue(fish);

        return fish;
    }

    public void Add(RequestHandler fish)
    {
        pool.Enqueue(fish);
    }

    public void Update()
    {
        foreach(RequestHandler handler in pool)
        {
            handler.Update();
        }
    }
}
