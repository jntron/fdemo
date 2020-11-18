using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetCreator : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var objects = GameObject.FindGameObjectsWithTag("Target");
  
        if (objects.Length < 5) //Se till att det alltid finns 5 targets
        {
            var go = Instantiate(target, transform.position + Random.insideUnitSphere * 10f, Quaternion.identity);
        }


        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
