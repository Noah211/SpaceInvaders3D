using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class SwitchCameras : MonoBehaviour
{
    private LinkedList<GameObject> cameras;
    private LinkedListNode<GameObject> currentCamera;

    void Start()
    {
        GameObject playerCamera = GameObject.Find("PlayerCamera");
        GameObject perspectiveCamera = GameObject.Find("PerspectiveCamera");
        GameObject TopCamera = GameObject.Find("TopCamera");
        cameras = new LinkedList<GameObject>();
        cameras.AddLast(playerCamera);
        cameras.AddLast(perspectiveCamera);
        cameras.AddLast(TopCamera);
        currentCamera = cameras.First;
        perspectiveCamera.GetComponent<Camera>().enabled = false;
        perspectiveCamera.GetComponent<AudioListener>().enabled = false;
        TopCamera.GetComponent<Camera>().enabled = false;
        TopCamera.GetComponent<AudioListener>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.F2))
        {
            currentCamera.Value.GetComponent<Camera>().enabled = false;
            currentCamera.Value.GetComponent<AudioListener>().enabled = false;

            if (currentCamera.Next != null)
            {
                currentCamera = currentCamera.Next;
            }
            else
            {
                currentCamera = cameras.First;
            }

            currentCamera.Value.GetComponent<Camera>().enabled = true;
            currentCamera.Value.GetComponent<AudioListener>().enabled = true;
        }
    }

    /*
     * Call this before deleting the player, duplicates the camera attached to the player before the player is destroyed.
     */
    public void UpdateFirstCamera()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject originalFirstCamera = cameras.First.Value;
        Vector3 newPosition = new Vector3(player.transform.position.x, originalFirstCamera.transform.position.y, originalFirstCamera.transform.position.z);
        Quaternion newRotation = Quaternion.Euler(10, 180, 0);
        GameObject newFirstCamera = Instantiate(originalFirstCamera, newPosition, newRotation);
        newFirstCamera.GetComponent<Camera>().enabled = false;
        newFirstCamera.GetComponent<AudioListener>().enabled = false;
        cameras.First.Value.GetComponent<Camera>().enabled = false;
        cameras.First.Value.GetComponent<AudioListener>().enabled = false;

        if (currentCamera == cameras.First)
        {
            newFirstCamera.GetComponent<Camera>().enabled = true;
            newFirstCamera.GetComponent<AudioListener>().enabled = true;
            cameras.RemoveFirst();
            cameras.AddFirst(newFirstCamera);
            currentCamera = cameras.First;
        }
        else
        {
            cameras.RemoveFirst();
            cameras.AddFirst(newFirstCamera);
        }
    }
}
