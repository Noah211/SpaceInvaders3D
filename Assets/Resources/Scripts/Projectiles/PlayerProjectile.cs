using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

// Author: Noah Logan

// Consider using Raycasting instead of a lineRenderer will a boxCollider.

public class PlayerProjectile : MonoBehaviour, IProjectile
{
    public Material PlayerProjectileTexture;

    private LineRenderer lineRenderer;
    private BoxCollider boxCollider;
    private Vector3 velocity;
    private float width;
    private float length;

    public void Start()
    {
        width = 0.5f;
        length = 7.5f;
        Vector3 initialStartPoint = transform.position + new Vector3(0, 0, -5.4f);
        Vector3 initialEndPoint = initialStartPoint + new Vector3(0, 0, -length);
        velocity = new Vector3(0, 0, -180);
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = PlayerProjectileTexture;
        lineRenderer.widthMultiplier = width;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, initialStartPoint);
        lineRenderer.SetPosition(1, initialEndPoint);
        boxCollider = gameObject.transform.Find("Collider").GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, width, length);
        boxCollider.isTrigger = true;
    }

    public void Update()
    {
        lineRenderer.SetPosition(0, lineRenderer.GetPosition(0) + velocity * Time.deltaTime);
        lineRenderer.SetPosition(1, lineRenderer.GetPosition(1) + velocity * Time.deltaTime);
        transform.position = (lineRenderer.GetPosition(0) + lineRenderer.GetPosition(1)) / 2;

        // If position is further than any enemy position.
        if (transform.position.z < -250)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("EnemyProjectileCollider"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("EnemyProjectileCollider"))
        {
            // To Do: Add Collision Audio
            // To Do: Add Visual Effect
        }
    }
}
