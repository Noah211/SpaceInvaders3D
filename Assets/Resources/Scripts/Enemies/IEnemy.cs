using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public interface IEnemy
{
    void Start();

    void Update();

    void ChangeDirection(string direction);

    void ChangeSpeed(Vector3 speed);

    void Die();

    void Shoot();

    void OnTriggerEnter(Collider other);
}
