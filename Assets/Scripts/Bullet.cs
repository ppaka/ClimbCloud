﻿using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int atk;

    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        
    }
}