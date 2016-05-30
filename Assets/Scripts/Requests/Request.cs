using UnityEngine;
using System.Collections;

public interface Request {

    void PrePerformance();

    void Perform();

    void PostPerformance();
}
