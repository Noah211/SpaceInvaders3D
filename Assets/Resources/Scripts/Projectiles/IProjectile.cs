using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public interface IProjectile
{
    void Start();

    void Update();

    void OnTriggerEnter(Collider other);
}
