using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviourPun, IDamageable
{
    [SerializeField] EnemyData enemyData;
    private static bool gemPickedUp = false;
    bool hasGem = false;
    bool targetPlayers = true;
    int health;

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform player;
    private Vector3 spawnPosition;

    private static Queue<Enemy> enemies = new Queue<Enemy>();
    private static int chasers = 0;

    private static readonly int CHASER_COUNT = 3;

    private void Start() {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        health = enemyData.startHealth;

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

        }


    }

    void FixedUpdate()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        if (hasGem) {
            TryMoveTowards(spawnPosition, enemyData.range);
            return;
        }

        if (targetPlayers && enemyData.teamFlags.HasFlag(EnemyData.TargetFags.Players)) {
            TryMoveTowards(player.position, enemyData.range);
            return;
        }

        if (TryMoveTowards(TeamManager.Instance.gem.position, enemyData.range + (gemPickedUp ? enemyData.range : 0))) {
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
        //GetComponent<MeshRenderer>().material = TeamManager.Instance.greenMaterial;

        TeamManager.Instance.gem.SetParent(transform);
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            // Drop gem;
            if (hasGem)
            {
                TeamManager.Instance.gem.SetParent(null);
                gemPickedUp = false;
            }
            if (!targetPlayers) {
                Enemy e = null;
                while (!e && enemies.Count > 0)
                {
                    e = enemies.Dequeue();
                }
                if (e)
                {
                    e.targetPlayers = false;
                } else
                {
                    chasers--;
                }
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
