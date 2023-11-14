using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class Enemy : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] EnemyData enemyData;

    [SerializeField] NetworkManager networkManager;
    private static bool gemPickedUp = false;
    bool hasGem = false;
    bool targetPlayers = true;
    int health;

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform player;
    [SerializeField] private PhotonView enemy;
    private Vector3 spawnPosition;
    private bool destoyed = false;

    private Vector3[] playerSpawnPositions;

    private static Queue<Enemy> enemies = new Queue<Enemy>();
    private static int chasers = 0;

    private static readonly int CHASER_COUNT = 3;

    private void Start() {
        health = enemyData.startHealth;
        playerSpawnPositions = networkManager.spawnPositions;

        spawnPosition = transform.position;
        if (enemyData.teamFlags.HasFlag(EnemyData.TargetFlags.Gem))
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

        if (enemyData.teamFlags.HasFlag(EnemyData.TargetFlags.Players))
        {
            int player = TeamManager.Instance.assignedTeam;
            Material mat = (player == 0) ? TeamManager.Instance.redMaterial : TeamManager.Instance.blueMaterial;
            this.player = TeamManager.Instance.player;
            GetComponent<MeshRenderer>().material = mat;
        }

        if(destoyed){
            Debug.Log("Late Destoy");
            PhotonNetwork.Destroy(gameObject);
        }


    }

    void FixedUpdate()
    {
        if (hasGem) {
            TryMoveTowards(spawnPosition, enemyData.range);

            return;
        }

        if (targetPlayers && enemyData.teamFlags.HasFlag(EnemyData.TargetFlags.Players)) {
            //This needs to be per player
            //TryMoveTowards(player.position, enemyData.range);
            if (PhotonNetwork.IsMasterClient){
                TryMoveTowards(playerSpawnPositions[0], enemyData.range);
                return;
            }else{
                TryMoveTowards(playerSpawnPositions[1], enemyData.range);
                return;
            }
            
        }

        if (TryMoveTowards(TeamManager.Instance.gem.position, enemyData.range + (gemPickedUp ? enemyData.range : 0))) {
            return;
        }
        if (!gemPickedUp)
        {
            GrabGem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Finish" || !hasGem){
            
            return;
        }
        print(other.gameObject);
        Lose();
    }

    bool TryMoveTowards(Vector3 position, float radius) {
        position.y = transform.position.y;
        transform.LookAt(position);
        position = position - transform.position;

        if (position.magnitude > radius) {
            //Write the gravity velocity before it's overwritten
            float fallVector = rigidBody.velocity.y;
            Vector3 moveVector = position.normalized * enemyData.speed;

            //Reapply gravity velocity after moveVector is determined
            moveVector.y = fallVector;
            rigidBody.velocity = moveVector;
        
            return true;
        }

        return false;
    }

    void GrabGem() {
        gemPickedUp = true;
        hasGem = true;

        // Temporary animation
        GetComponent<MeshRenderer>().material = TeamManager.Instance.greenMaterial;

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
            if(photonView.IsMine)
            {
                photonView.RPC("RPC_EnemyDeath", RpcTarget.OthersBuffered);
                PhotonNetwork.Destroy(gameObject);
            }
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

    public void Lose()
    {
        Debug.Log("You Lose");
    }

    [PunRPC]
    private void RPC_EnemyDeath(){
        
        destoyed = true;
        

    }
}
