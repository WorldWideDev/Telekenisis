using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekenesis : MonoBehaviour {

    public float maxGrabDistance = 20f;
    public float objectBuffer = 2;
    public float smoothness = 4;
    public float throwForce = 60;

    Camera cam;
    bool isCarrying = false;
    GameObject carried;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

	void Update()
    {
        if (!isCarrying)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Grab();
            }
        }
        else
        {
            if(carried != null)
            {
                Carry(carried);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Drop();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    ReadyThrow();
                }
            }
            
        }
    }

    void Grab()
    {
        RaycastHit hit;
        Vector3 origin = cam.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        if(Physics.Raycast(origin, cam.transform.forward, out hit, maxGrabDistance))
        {
            if(hit.collider.tag == "Interactable")
            {
                carried = hit.collider.gameObject;
                isCarrying = true;
            }
        }
    }

    void Carry(GameObject go)
    {
        go.GetComponent<Rigidbody>().isKinematic = true;
        go.transform.position = Vector3.Lerp(go.transform.position, cam.transform.position + cam.transform.forward * objectBuffer, Time.deltaTime * smoothness);
    }

    void Drop()
    {
        isCarrying = false;
        carried.GetComponent<Rigidbody>().isKinematic = false;
        carried = null;
    }

    void ReadyThrow()
    {
        isCarrying = false;
        Rigidbody rb = carried.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        StartCoroutine(Throw(rb, cam.transform.forward));
    }

    IEnumerator Throw(Rigidbody rb, Vector3 launchAngle)
    {
        rb.AddForce(launchAngle * throwForce);
        yield return new WaitForSeconds(1);
    }

}
