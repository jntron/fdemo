using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class GunFire : MonoBehaviour
{
    InputDevice leftHand;

    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool triggerHit = false;
    // Update is called once per frame
    void Update()
    {
        if(!leftHand.isValid)
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, devices);
            foreach(var device in devices)
            {
                leftHand = device;
            }
        }

        float trigger = 0;
        if (leftHand.TryGetFeatureValue(CommonUsages.trigger, out trigger) && trigger > 0.5f && !triggerHit)
        {
            Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(transform.up * 500f, ForceMode.VelocityChange);
            triggerHit = true;
        }
        else if (triggerHit)
        {
            triggerHit = false;
        }

    }
}
