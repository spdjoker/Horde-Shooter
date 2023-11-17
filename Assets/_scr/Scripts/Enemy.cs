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

    public NetworkManager networkManager;
    private static bool gemPickedUp = false;
    bool hasGem = false;
    bool targetPlayers = true;
    int health;

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform player;
    [SerializeField] private PhotonView enemy;
    private Vector3 spawnPosition;
    private bool destoyed = false;
    private bool invincible = false;
    private Vector3[] playerSpawnPositions;

    private static Queue<Enemy> enemies = new Queue<Enemy>();
    private static int chasers = 0;

    private static readonly int CHASER_COUNT = 1;

    Animator anim;

    private void Start() {
        networkManager = GameObject.Find("/VR_Component/Network Manager").GetComponent<NetworkManager>();
        health = enemyData.startHealth;
        playerSpawnPositions = networkManager.spawnPositions;
        anim = GetComponent<Animator>();
        

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
                if(TryMoveTowards(playerSpawnPositions[0], enemyData.range)){
                    return;
                }
                
            }else{
                if(TryMoveTowards(playerSpawnPositions[1], enemyData.range)){
                    return;
                }
            }
            StartCoroutine(AttackCoroutine());
            
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Finish" || !hasGem){
            
            return;
        }
        print(other.gameObject);
        Lose();
    }

    bool TryMoveTowards(Vector3 position, float radius) {
        if (anim.GetBool("attacking")) {//If attacking is true
            return false;
        }

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
    private IEnumerator AttackCoroutine(){
        anim.SetBool("attacking", true);
        // Wait for the attack animation to complete
        yield return new WaitForSeconds(1.75f);

        anim.SetBool("attacking", false);
        networkManager.PlayerLoseHealth();
        EnemyDeath();
    }
    private IEnumerator DeathCoroutine(){
        anim.SetBool("shot", true);
        // Wait for the death animation to complete
        yield return new WaitForSeconds(1f);

        EnemyDeath();
    }


    public void Damage(int amount)
    {  
        if (photonView.IsMine)
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
                if (!targetPlayers) 
                {
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
                StartCoroutine(DeathCoroutine());
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

    public void EnemyDeath()
    {
        //networkManager.PlayerLoseHealth();
        if(photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void Lose()
    {
        Debug.Log("Lose By Gem Capture");
        networkManager.LoseGame();
    }

    [PunRPC]
    private void RPC_EnemyDeath(){
        
        destoyed = true;
        

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
       
    }
}
