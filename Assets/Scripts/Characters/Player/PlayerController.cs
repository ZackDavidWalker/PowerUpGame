﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private float _currentHealth;
    private float _timerHelper_regenCooldown;
    private Vector2 _lookDirection;

    public float movementSpeed = 5f;
    public float maxHealth = 10;
    public float regenSpeed = 1; // in units per second
    public float regenCooldown = 3;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // health init
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        FaceMouse();

        // health regeneration
        if (_currentHealth < maxHealth)
        {
            _timerHelper_regenCooldown += Time.deltaTime;
            if (_timerHelper_regenCooldown >= regenCooldown)
                AddHealth(regenSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Seeker>();
        if (enemy != null)
        {
            RemoveHealth(enemy.strength);
        }
    }

    private void AddHealth(float amount)
    {
        _currentHealth = Math.Min(_currentHealth + amount, maxHealth);
        if (Mathf.Approximately(_currentHealth, maxHealth))
            _currentHealth = maxHealth;
        UIHealthBar.Instance.SetValue(_currentHealth / maxHealth);
    }

    private void RemoveHealth(float amount)
    {
        _currentHealth = Math.Max(_currentHealth - amount, 0);
        _timerHelper_regenCooldown = 0;
        UIHealthBar.Instance.SetValue(_currentHealth / maxHealth);
    }

    private void UpdatePosition()
    {
        var pos = _rigidbody2D.position;
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");
        pos.x += horizontalMovement * movementSpeed * Time.deltaTime;
        pos.y += verticalMovement * movementSpeed * Time.deltaTime;
        _rigidbody2D.MovePosition(pos);
    }

    private void FaceMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _lookDirection = (Vector2)mousePosition - _rigidbody2D.position;
        _lookDirection.Normalize();
        transform.up = _lookDirection;
    }

    public Vector2 GetLookDirection()
    {
        return _lookDirection;
    }
}