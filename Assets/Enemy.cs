using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    private static bool gemPickedUp = false;
    bool hasGem = false;

    int health;

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform gem;
    [SerializeField] private Transform player;
    private Vector3 spawnPosition;

    private void Start() {
        if (enemyData == null) {
            throw new System.Exception("Uh oh, someone forgot to add data to an enemy... The enemy's name was " + name);
        }

        health = enemyData.startHealth;

        if (gem == null) {
            throw new System.Exception("The enemy " + name + " doesn't have any target, set it to the gem!");
        }

        spawnPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (hasGem) {
            TryMoveTowards(spawnPosition, enemyData.range);
            return;
        }

        if (gemPickedUp) {
            TryMoveTowards(player.position, enemyData.range);
            return;
        }

        if (TryMoveTowards(gem.position, enemyData.range)) {
            return;
        }
        GrabGem();
    }

    bool TryMoveTowards(Vector3 position, float radius) {
        position.y = transform.position.y;
        transform.LookAt(position);
        position = position - transform.position;

        if (position.magnitude > radius) {
            rigidBody.velocity = position.normalized * enemyData.speed;
            return true;
        }

        return false;
    }

    void GrabGem() {
        gemPickedUp = true;
        hasGem = true;

        // Temporary animation
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Material material = new Material(renderer.material);
        material.color = Color.green;
        renderer.material = material;

        gem.SetParent(transform);
    }
}
