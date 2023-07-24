using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [SerializeField] float m_CheckDistance = 6.0f, m_DownwardForce = 20.0f;

    private void OnCollisionStay(Collision collision)
    {
        RaycastForward();
        RaycastBackward();
    }

    void RaycastForward()
    {
        RaycastHit hitInfo;
        
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, m_CheckDistance)) {
            if (hitInfo.collider.tag == "Player")
                hitInfo.collider.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, -m_DownwardForce, 0.0f), ForceMode.Impulse);
        }

        Debug.DrawLine(transform.position, transform.position + transform.forward * m_CheckDistance, Color.red, 1.0f);
    }

    void RaycastBackward()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, -transform.forward, out hitInfo, m_CheckDistance)) {
            if (hitInfo.collider.tag == "Player")
                hitInfo.collider.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, -m_DownwardForce, 0.0f), ForceMode.Impulse);
        }

        Debug.DrawLine(transform.position, transform.position - transform.forward * m_CheckDistance, Color.red, 1.0f);
    }
}
