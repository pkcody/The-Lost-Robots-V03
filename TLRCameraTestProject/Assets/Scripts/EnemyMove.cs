using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    public Transform closestPlayer;
    public List<Transform> playerPos = new List<Transform>();

    public Transform patrolRoute;
    public List<Transform> locations;
    public int damage = 1;
    public Slider monsterSlider;


    private int locationIndex = 0;
    private NavMeshAgent agent;
    private int _lives = 5;

    public Animator animator;

    public GameObject mobDrop;

    public GameObject deathParticle;

    public int EnemyLives
    {
        get { return _lives; }
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {

                //closestPlayer.GetComponent<CharacterMovement>().MonsterAttackBoarder.SetActive(false);
                foreach (CharacterMovement cm in FindObjectsOfType<CharacterMovement>())
                {
                    cm.MonsterAttackBoarder.SetActive(false);

                    cm.inRangeMonster = false;
                }
                DeathRollAnimation();
                
                Debug.Log("Enemy down.");
            }
            else
            {
                animator.SetTrigger("takedmg");

            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //player = GameObject.Find("Player").transform;
        foreach (GameObject go in PlayerSpawning.instance.players)
        {
            if (go != null)
            {
                playerPos.Add(go.transform);
            }
        }

        monsterSlider.maxValue = EnemyLives;
        monsterSlider.value = EnemyLives;
       
        //InitializePatrolRoute();
        //MoveToNextPatrolLocation();
    }

    //void Update()
    //{
    //    if (agent.remainingDistance < 0.2f && !agent.pathPending)
    //    {
    //        MoveToNextPatrolLocation();
    //    }

    //}




    void DeathRollAnimation()
    {
        animator.SetTrigger("death");
        DropItem();
        deathParticle.gameObject.SetActive(true);
        Invoke("DestroyEnemy", 3f);
    }

    void DestroyEnemy()
    {
        Destroy(transform.parent.gameObject);
        
    }

    void DropItem()
    {
        //string path = "";
        //if (gameObject.name.Contains("Green")) //aka cyclopes green
        //{
        //    //path = "Res_OBJ/WoodRes_obj";
        //    GameObject drop = Instantiate(greenDrop, transform.parent.position + new Vector3(0f, .5f, 0f), Quaternion.identity, null);
        //}
        //else if (gameObject.name.Contains("Blue")) // aka glise blue
        //{
        //    //path = "Res_OBJ/PaperRes";
        //    GameObject drop = Instantiate(blueDrop, transform.parent.position + new Vector3(0f, .5f, 0f), Quaternion.identity, null);
        //}
        //else if (gameObject.name.Contains("Red")) // still sand red
        //{
        //    //path = "Res_OBJ/MetalRes_obj";
        //    GameObject drop = Instantiate(redDrop, transform.parent.position + new Vector3(0f, .5f, 0f), Quaternion.identity, null);
        //}
        //GameObject drop = Instantiate(Resources.Load(path) as GameObject, transform.parent.position + new Vector3(0f, .5f, 0f), Quaternion.identity, null);
        GameObject drop = Instantiate(mobDrop, transform.parent.position + new Vector3(0f, .5f, 0f), Quaternion.identity, null);
    }


    //void InitializePatrolRoute()
    //{
    //    foreach (Transform child in patrolRoute)
    //    {
    //        locations.Add(child);
    //    }
    //}

    //void MoveToNextPatrolLocation()
    //{
    //    if (locations.Count == 0)
    //        return;

    //    //anim
    //    if (animator.gameObject.name.Contains("Green")) //aka Cyclopes green
    //    {
    //        animator.SetBool("GMwalk", true);
    //    }
    //    else if (animator.gameObject.name.Contains("Blue")) // aka Glise blue
    //    {
    //        animator.SetBool("BMwalk", true);
    //    }
    //    else if (animator.gameObject.name.Contains("Red")) // Still sand red
    //    {
    //        animator.SetBool("RMattack", false);
    //        animator.SetBool("RMtakedmg", false);
    //        animator.SetBool("RMwalk", true);
    //    }

    //    agent.destination = locations[locationIndex].position;
    //    locationIndex = (locationIndex + 1) % locations.Count;
    //}

    public void TakeDamage()
    {
        monsterSlider.value -= damage;
        EnemyLives -= damage;

    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.name.Contains("Player"))
    //    {
    //        agent.destination = other.transform.position;

    //        if (animator.gameObject.name.Contains("Green")) //aka Cyclopes green
    //        {
    //            animator.SetBool("GMwalk", false);
    //            animator.Play("GreenMon_Attack", 0, .5f);
    //        }
    //        else if (animator.gameObject.name.Contains("Blue")) // aka Jelly blue
    //        {
    //            animator.SetBool("BMwalk", false);
    //            animator.Play("BlueMon_Attack", 0, .5f);
    //        }
    //        else if (animator.gameObject.name.Contains("Red")) // Still sand red
    //        {
    //            animator.SetBool("RMtakedmg", false);
    //            animator.SetBool("RMwalk", false);
    //            animator.SetBool("RMattack", true);

    //        }
    //        Debug.Log("Player detected - attack!");
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.name.Contains("Player"))
    //    {
    //        //anim
    //        //if (animator.gameObject.name.Contains("Green")) //aka Cyclopes green
    //        //{
    //        //    animator.SetBool("GMwalk", true);
    //        //}
    //        //else if (animator.gameObject.name.Contains("Blue")) // aka Glise blue
    //        //{
    //        //    animator.SetBool("BMwalk", true);
    //        //}
    //        //else if (animator.gameObject.name.Contains("Red")) // Still sand red
    //        //{
    //        //    animator.SetBool("RMwalk", true);
    //        //}

    //        MoveToNextPatrolLocation();
    //        Debug.Log("Player out of range, resume patrol");
    //    }
    //}


   
}
