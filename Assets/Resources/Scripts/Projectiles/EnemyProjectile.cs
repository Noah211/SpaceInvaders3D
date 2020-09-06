using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

// Consider using Raycasting instead of a lineRenderer will a boxCollider.

public class EnemyProjectile : MonoBehaviour, IProjectile
{
    public Material EnemyProjectileTexture;

    private LineRenderer lineRenderer;
    private BoxCollider boxCollider;
    private Vector3 velocity;
    private Vector3 offset;
    private float speed;
    private float width;
    private float length;

    public void Start()
    {
        width = 0.5f;
        length = 7.5f;
        speed = 60;
        offset = new Vector3(Random.Range(-20f, 20f), 0, 0);
        GameObject player = GameObject.Find("Player");
        Vector3 initialStartPoint = transform.position + new Vector3(0, 0, 5.4f);
        Vector3 initialEndPoint = initialStartPoint + (((player.transform.position + offset) - initialStartPoint).normalized * length);
        velocity = ((player.transform.position + offset) - initialStartPoint).normalized * speed;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = EnemyProjectileTexture;
        lineRenderer.widthMultiplier = width;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, initialStartPoint);
        lineRenderer.SetPosition(1, initialEndPoint);
        float angle = Vector3.SignedAngle((player.transform.position + offset) - initialStartPoint, Vector3.forward, Vector3.down);
        GameObject collider = gameObject.transform.Find("Collider").gameObject;
        collider.transform.Rotate(new Vector3(0, angle, 0));
        boxCollider = collider.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, width, length);
        boxCollider.isTrigger = true;

        if ((angle >= 90f) || (angle <= -90))
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        lineRenderer.SetPosition(0, lineRenderer.GetPosition(0) + (velocity * Time.deltaTime));
        lineRenderer.SetPosition(1, lineRenderer.GetPosition(1) + (velocity * Time.deltaTime));
        transform.position = (lineRenderer.GetPosition(0) + lineRenderer.GetPosition(1)) / 2;

        // If position is further than the player position and off-screen.
        if (transform.position.z > 50)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("EnemyCollider") && other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
