using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetScript : MonoBehaviour
{

    public Vector2 amount;
    public float lerp = .5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        transform.localEulerAngles = new Vector3(Mathf.LerpAngle(transform.localEulerAngles.x, y * amount.y, lerp), Mathf.LerpAngle(transform.localEulerAngles.y, x * amount.x, lerp), 0);
        
    }
}
