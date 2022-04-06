using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Enemy : MonoBehaviour
{
    public Player player;
    [SerializeField]
    private float speed = 1f;
    private int rank = 1;
    public bool isStronger = false;
    public bool isObserved = false;
    private BehaviorTree.Tree<Enemy> _tree;
    public Vector3 Dir;
    public bool isChasing = false;
    


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        _tree = new Tree<Enemy>
            (
                new Sequence<Enemy>
                (
                    new isSee(),
                    new Selector<Enemy>
                        (
                            new Sequence<Enemy>
                            (
                                new isRanked(),
                                new beginChasing()
                            ),
                            new beginRunaway()
                        )
                )
            );
    }

    // Update is called once per frame
    void Update()
    {
        Rank();
        dirCal();
        _tree.Update(this);

        //Chase();
        //Observe();
    }

    public void dirCal() 
    {
        Dir =  player.GetComponentInParent<Transform>().position - this.transform.position ;

    }

    public void Chase() 
    {
        
        this.transform.Translate(Dir * speed * Time.deltaTime,Space.World);
        
    }



    public void Runaway() 
    {
        this.transform.Translate(-1f*Dir * speed * Time.deltaTime,Space.World);

    }

    public class beginRunaway : BehaviorTree.Node<Enemy> 
    {
        public override bool Update(Enemy context)
        {
            context.Runaway();
            return true;
        }
    }
    public class beginChasing : BehaviorTree.Node<Enemy> 
    {
        public override bool Update(Enemy context)
        {
            context.Chase();
            return true;
        }
    }


    //if this is stronger than player
    public bool Rank() 
    {
        if (rank > player.rank)
            isStronger = true;
        else
            isStronger = false;
        return isStronger;
    }

   /* 
    private bool isRankedCondition(Enemy context) 
    {
        return Rank();
    }
   */

    public class isRanked : BehaviorTree.Node<Enemy> 
    {
        public override bool Update(Enemy context)
        {
            return context.isStronger;
        }
    }

    /*
    private bool isObservedCondition(Enemy context) 
    {
        return isObserved;
    }
    */

    public class isSee : BehaviorTree.Node<Enemy> 
    {
        public override bool Update(Enemy context)
        {
            return context.isObserved;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player") 
        {
            isObserved = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player") 
        {
            isObserved = false;
        }
    }
}
