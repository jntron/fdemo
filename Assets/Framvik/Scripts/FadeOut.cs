using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame

    float dt = 0;
    void Update()
    {
        Color color = material.color;
        color.a = Mathf.Lerp(1f, 0f, dt/5f); // fade out
        material.color = color;

        dt += Time.deltaTime;
    }
}
