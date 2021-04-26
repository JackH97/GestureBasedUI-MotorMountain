using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class CarController : MonoBehaviour
{
    public float fuel = 1;
    public float fuelconsumption = 0.1f;
    public Rigidbody2D frontTire;
    public Rigidbody2D backTire;
    public Rigidbody2D carRigidbody;
    public float speed = 50;
    public float carTorque = -100;
    private float movement;
    public UnityEngine.UI.Image image;

    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;




    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //movement = Input.GetAxis("Horizontal");
        if(Input.GetKey(KeyCode.RightArrow))
        {
            movement = 1;
            Debug.Log("Right Arrow");
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            movement = -1;
            Debug.Log("Left Arrow");
        }
        else
        {
            movement = 0;
        }
        
        // Access the ThalmicMyo component attached to the Myo game object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

        if (thalmicMyo.pose != _lastPose) {

            if(thalmicMyo.pose == Pose.WaveIn)
            {
                movement = 1;
                Debug.Log("WaveIn");
                
            }
            else if(thalmicMyo.pose == Pose.WaveOut)
            {
                movement = -1;
                Debug.Log("WaveOut");
            }
            else
            {
                movement = 0;
                
            }
        }
        image.fillAmount = fuel;
        
    }

    private void FixedUpdate()
    {
        if(fuel>0)
        {
            backTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
            frontTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
            carRigidbody.AddTorque(-movement * carTorque * Time.fixedDeltaTime);
        }

       fuel -= fuelconsumption * Mathf.Abs(movement) * Time.fixedDeltaTime;
    }

    void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard) {
            myo.Unlock (UnlockType.Timed);
        }

        myo.NotifyUserAction ();
    }
}
