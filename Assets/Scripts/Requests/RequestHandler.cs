using System.Threading;
using System.Collections.Generic;

public class RequestHandler {

    private Thread thread;

    private Queue<Request> newRequests = new Queue<Request>();

    private ThreadedQueue<Request> requests = new ThreadedQueue<Request>();

    private ThreadedQueue<Request> completedRequests = new ThreadedQueue<Request>();

    private bool running = true;

    public RequestHandler()
    {
        thread = new Thread(PerformRequest);

        thread.Start();
    }

    public void Stop()
    {
        running = false;
    }

    public void QueueRequest(Request request)
    {
        request.PrePerformance();

        requests.Enqueue(request);
    }

    public void Update()
    {
        while(newRequests.Count > 0)
        {
            Request request = newRequests.Dequeue();
            request.PrePerformance();
            requests.Enqueue(request);
        }

        while(completedRequests.Count() > 0)
        {
            Request request = null;
            bool complete = completedRequests.TryDequeue(out request);
            if(request != null)
            {
                request.PostPerformance();
            }
        }
    }

    private void PerformRequest()
    {
        while (true)
        {
            Request request;
            bool completed = requests.TryDequeue(out request);

            if (completed)
            { 
                request.Perform();
            }

            completedRequests.Enqueue(request);

            Thread.Sleep(1);
        }
        
    }
}
