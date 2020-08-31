using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public interface IPlayer
{
    void Start();

    void Update();

    void Die();

    void Shoot();

    void OnTriggerEnter(Collider other);
}
