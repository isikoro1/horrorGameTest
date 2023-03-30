using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCon : MonoBehaviour
{
    public GameObject targetObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(targetObject.transform);
    }
}
