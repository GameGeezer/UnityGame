using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
TODO, what if a thread is stopped?
*/

public class RequestCircle
{
    private Queue<RequestHandler> pool = new Queue<RequestHandler>();

    public RequestCircle(int handlerCount, int sleepMiliSeconds)
    {
        for (int i = 0; i < handlerCount; ++i)
        {
            Add(new RequestHandler(sleepMiliSeconds));
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
