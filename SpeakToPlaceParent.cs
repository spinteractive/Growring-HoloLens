using UnityEngine;

public class SpeakToPlaceParent : MonoBehaviour
{
    bool move = false;

    AudioSource audioSource = null;
    AudioClip tapSound = null;

    void Start()
    {
        // Add an AudioSource component and set up some defaults
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Custom;

        // Load the Sphere sounds from the Resources folder
        tapSound = Resources.Load<AudioClip>("Impact");
    }

    // Called by SpeechManager when the user says the "Spin Object" command
    void Move()
    {
        move = true;
        SpatialMapping.Instance.DrawVisualMeshes = true;
        audioSource.clip = tapSound;
        audioSource.Play();
    }

    // Called by SpeechManager when the user says the "Stop Spinning" command
    void Place()
    {
        move = false;
        SpatialMapping.Instance.DrawVisualMeshes = false;
        audioSource.clip = tapSound;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.

        if (move)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                this.transform.parent.position = hitInfo.point;

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.parent.rotation = toQuat;
            }
        }
    }
}
