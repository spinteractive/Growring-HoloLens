using UnityEngine;
using System.Collections;

public class DiamondCommands : MonoBehaviour
{
    bool spinObject = false;
    bool moveObject = false;
    bool sizeObject = false;
    bool isRotating = false;
    bool incubatorOnce = false;

    //Fade Object
    //public GameObject fadingObject;

    AudioSource audioSource = null;
    AudioClip tapSound = null;
    AudioClip ringBoxSound = null;
    AudioClip moveHoldSound = null;
    AudioClip largerSound = null;
    AudioClip smallerSound = null;
    AudioClip pinkDiamond = null;
    AudioClip incubator = null;
    AudioClip treeSound = null;
    AudioClip girlSound = null;
    AudioClip incubatorNext = null;
    AudioClip bigRing = null;
    AudioClip bloodSound = null;

    private AudioSource[] allAudioSources;
    private GameObject[] allRotatingObjets;

    GameObject myParticles;
    ParticleSystem part1;

    void Start()
    {
        // Add an AudioSource component and set up some defaults
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Custom;

        // Load the Sphere sounds from the Resources folder
        tapSound = Resources.Load<AudioClip>("Select13");
        ringBoxSound = Resources.Load<AudioClip>("SlotPrize2");
        pinkDiamond = Resources.Load<AudioClip>("voice5a");
        moveHoldSound = Resources.Load<AudioClip>("Select17");
        smallerSound = Resources.Load<AudioClip>("Shrink2");
        largerSound = Resources.Load<AudioClip>("Grow");
        incubator = Resources.Load<AudioClip>("voice3a");
        treeSound = Resources.Load<AudioClip>("voice7ab");
        girlSound = Resources.Load<AudioClip>("IVONAa");
        bigRing = Resources.Load<AudioClip>("IVONA2a");
        incubatorNext = Resources.Load<AudioClip>("INC");
        bloodSound = Resources.Load<AudioClip>("sociallyresponsible");

        //particles
        myParticles = GameObject.FindGameObjectWithTag("Particles");
        part1 = myParticles.GetComponent<ParticleSystem>();
        part1.Stop();

        //Fade in Object
        //Fader.Instance.FadeIn(fadingObject).Pause(1).FadeOut(fadingObject, 0.5f);

        

    }

    void StopAllAudio() {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach( AudioSource audioS in allAudioSources) {
         audioS.Stop();
        }
    }

    void StopAllRotating()
    {
        allRotatingObjets = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject gObject in allRotatingObjets)
        {
            gObject.transform.parent.Rotate(0,0,0,0);
        }
    }

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        moveObject = !moveObject;

        audioSource.clip = tapSound;
        audioSource.Play();

        if (moveObject) {
            SpatialMapping.Instance.DrawVisualMeshes = true;
            part1.Play();

        } else {
            SpatialMapping.Instance.DrawVisualMeshes = false;
            part1.Stop();
        }
    }

    // Called by GazeGestureManager when the user performs a Hold
    void TellMe()
    {
        StopAllAudio();
        spinObject = !spinObject;
        SpatialMapping.Instance.DrawVisualMeshes = false;
        audioSource.clip = moveHoldSound;
        audioSource.Play();

        //play special sound
        if (this.CompareTag("RingBox"))
        {
            audioSource.clip = ringBoxSound;
            audioSource.Play();

            //Do a special spin
            StartCoroutine(RotateForSeconds());
        }

        //play special sound
        if (this.CompareTag("pinkDiamond"))
            audioSource.clip = pinkDiamond;
            audioSource.Play();

        if (!incubatorOnce)
        {
            //play special sound
            if (this.CompareTag("INC_Tag"))
                audioSource.clip = incubator;
                audioSource.Play();
                incubatorOnce = true;
        }
        else
        {
            //play special sound
            if (this.CompareTag("INC_Tag"))
                audioSource.clip = incubatorNext;
                audioSource.Play();
                incubatorOnce = true;
        }

        //play special sound
        if (this.CompareTag("Tree"))
            audioSource.clip = treeSound;
            audioSource.Play();

        //play special sound
        if (this.CompareTag("BloodDiamond"))
            audioSource.clip = bloodSound;
            audioSource.Play();

        //play special sound
        if (this.CompareTag("Girl"))
            audioSource.clip = girlSound;
            audioSource.Play();

        //play special sound
        if (this.CompareTag("BigRing"))
            audioSource.clip = bigRing;
            audioSource.Play();

    }

    // Called by GazeGestureManager when the user performs a Hold
    void StopSpin()
    {
        spinObject = !spinObject;
        SpatialMapping.Instance.DrawVisualMeshes = false;
        audioSource.clip = moveHoldSound;
        audioSource.Play();

    }

    // Called by SpeechManager when the user says the command
    void Grow()
    {
        this.transform.parent.localScale += new Vector3(.5F, .5F, .5F);
        audioSource.clip = largerSound;
        audioSource.Play();
    }

    // Called by SpeechManager when the user says the command
    void Shrink()
    {
        this.transform.parent.localScale += new Vector3(-.5F, -.5F, -.5F);
        audioSource.clip = smallerSound;
        audioSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        if (moveObject)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;
            RaycastHit hitInfo;

            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo, 30.0f, SpatialMapping.PhysicsRaycastMask))
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                this.transform.parent.position = hitInfo.point;

            //if ((this.CompareTag("RingBox")))
            //{
                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.parent.rotation = toQuat;
            //}

                myParticles.transform.position = hitInfo.point;
        }

        //return if we are not spinning
        if (!spinObject)
        {
            return;
        }

        //if its not a ringbox, spin it * 50 for all other objects
        if (!this.CompareTag("RingBox"))
            this.transform.parent.Rotate(Vector3.up, Time.deltaTime * 50);

    }


    //This spin the ringbox
    IEnumerator RotateForSeconds() //Call this method with StartCoroutine(RotateForSeconds());
    {
        float time = 2;     //How long will the object be rotated?

        while (time > 0)     //While the time is more than zero...
        {
            transform.parent.Rotate(Vector3.up, Time.deltaTime * 800);     //...rotate the object.
            time -= Time.deltaTime;     //Decrease the time- value one unit per second.

            yield return null;     //Loop the method.
        }

        Quaternion toQuat = Camera.main.transform.localRotation;
        toQuat.x = 0;
        toQuat.z = 0;
        this.transform.parent.rotation = toQuat;

    }
}
