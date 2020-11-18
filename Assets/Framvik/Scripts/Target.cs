using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float time;
    public GameObject pieces;
    public Vector3 startPosition;
    public bool destroyed = false;
    public static int counter = 0;
    private void Awake()
    {
        startPosition = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        counter += 1;
    }

    private void OnCollisionEnter(Collision collision)
    { 
        counter -= 1;
        Instantiate(pieces, transform.position, transform.rotation);
        destroyed = true;
        Destroy(gameObject);

    }

    private void OnDestroy()
    {
        destroyed = true;
    }


    Vector3 start, stop;
    float dt = 5f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (dt < 5f)
        {
            transform.position = Vector3.Lerp(start, stop, dt / 5f);
            dt += Time.deltaTime;
        }
        else
        {
            start = transform.position;
            stop = startPosition + Random.onUnitSphere * 6f;
            dt = 0;
        }
    }
}

