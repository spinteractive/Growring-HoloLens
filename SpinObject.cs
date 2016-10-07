using UnityEngine;
using System.Collections;

public class SpinObject : MonoBehaviour {

    bool rotate;
    Collider m_SpinTarget;

    // Use this for initialization
    void Start () {

    }

    // Called by SpeechManager when the user says the "Spin Object" command
    void StartRotate() 
    {

        rotate = true;
    }

    // Called by SpeechManager when the user says the "Stop Spinning" command
    void StopRotate()
    {
        rotate = false;
    }

    // Update is called once per frame
    void Update () {

            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
            {
                // Rotate Object
                m_SpinTarget = hitInfo.collider;
            }


        if (rotate)
        {
            m_SpinTarget.gameObject.transform.Rotate(Vector3.forward, Time.deltaTime * 50);
        }
    }
}
