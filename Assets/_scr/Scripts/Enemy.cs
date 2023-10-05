using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyData enemyData;
    private static bool gemPickedUp = false;
    bool hasGem = false;
    bool targetPlayers = true;
    int health;

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform gem;
    [SerializeField] private Transform player;
    private Vector3 spawnPosition;

    private static Queue<Enemy> enemies = new Queue<Enemy>();
    private static int chasers = 0;

    private static readonly int CHASER_COUNT = 3;

    private void Start() {
        if (enemyData == null) {
            throw new System.Exception("Uh oh, someone forgot to add data to an enemy... The enemy's name was " + name);
        }

        health = enemyData.startHealth;

        if (gem == null) {
            throw new System.Exception("The enemy " + name + " doesn't have any target, set it to the gem!");
        }

        spawnPosition = transform.position;
        if (enemyData.teamFlags.HasFlag(EnemyData.TargetFags.Gem))
        {
            if (chasers < CHASER_COUNT)
            {
                chasers++;
                targetPlayers = false;
            }
            else
            {
                enemies.Enqueue(this);
            }
        }

        if (enemyData.teamFlags.HasFlag(EnemyData.TargetFags.Players))
        {
            int player = Random.Range(0, 2);
            Material mat = (player == 0) ? TeamManager.Instance.redMaterial : TeamManager.Instance.blueMaterial;
            GetComponent<MeshRenderer>().material = mat;
        }


    }

    void FixedUpdate()
    {
        if (hasGem) {
            TryMoveTowards(spawnPosition, enemyData.range);
            return;
        }

        if (targetPlayers) {
            TryMoveTowards(player.position, enemyData.range);
            return;
        }

        if (TryMoveTowards(gem.position, enemyData.range + (gemPickedUp ? enemyData.range : 0))) {
            return;
        }
        if (!gemPickedUp)
        {
            GrabGem();
        }
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
        GetComponent<MeshRenderer>().material = TeamManager.Instance.greenMaterial;

        gem.SetParent(transform);
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            // Drop gem;
            if (hasGem)
            {
                gem.SetParent(null);
                gemPickedUp = false;
            }
            if (!targetPlayers && enemies.Count > 0) {
                Enemy e = enemies.Dequeue();
                e.targetPlayers = false;
            }
            Drop();
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        throw new System.NotImplementedException();
    }

    public void Drop()
    {
        // Drop coins
    }
}
