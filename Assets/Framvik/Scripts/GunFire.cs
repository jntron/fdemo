using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GunFire : MonoBehaviour
{

    public GameObject bullet;

    InputDevice leftHand;

    // Start is called before the first frame update
    void Start()
    {
    }

    void GetLeftHandDevice()
    {
        if(!leftHand.isValid)
        {
            List<InputDevice> devices = new List<InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, devices);
            if (devices.Count > 0)
                leftHand = devices[0];
        }
    }

    // Update is called once per frame
    bool hasBeenFired = false;
    void Update()
    {
        GetLeftHandDevice();
        float trigger = 0;
        leftHand.TryGetFeatureValue(CommonUsages.trigger, out trigger);
        if(!hasBeenFired && trigger > 0.5f)
        {
            var theBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            var bulletBody = theBullet.GetComponent<Rigidbody>();
            bulletBody.AddForce(transform.up * 250, ForceMode.VelocityChange);
            hasBeenFired = true;
            Destroy(theBullet, 5.0f);
        }
        else if(hasBeenFired && trigger <= 0.5f)
        {
            hasBeenFired = false;
        }
    }
}
