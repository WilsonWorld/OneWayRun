using System.Collections;
using UnityEngine;

public class MoveOverTime : MonoBehaviour
{
    [SerializeField] Player PlayerRef;
    [SerializeField] Transform StartPoint, EndPoint;
    [SerializeField] float MoveTime = 30.0f;
    bool bIsMoving = false;


    void Update()
    {
        if (bIsMoving)
            transform.position += (EndPoint.position - StartPoint.position) / MoveTime * Time.deltaTime;
    }

    IEnumerator StartMoving(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        bIsMoving = true;
        StartCoroutine(StopMoving());
        PlayerRef.EnableControls(true);
    }

    IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(MoveTime);
        bIsMoving = false;
        PlayerRef.EnableControls(false);
    }
    
    public void CancelMovement()
    {
        bIsMoving = false;
        PlayerRef.EnableControls(true);
    }

    public void StartMovementWithDelay(float delayTime)
    {
        StartCoroutine(StartMoving(delayTime));
    }
}
