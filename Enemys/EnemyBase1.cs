using UnityEngine;
using System.Collections;

public abstract class Enemys : MonoBehaviour {

    public enum LookAt
    {
        right,
        left
    }
    public bool isDead;
    //private float delayBeforDestroy = 0.1f;


    public string mName;
    public int mHealth;
    public int mDamage;

    public float mSpeed;
    public int direction;
    public LookAt lookAt;
    public bool flip;
    private float moveForce = 350;
    public GameObject LeftBorder;
    public GameObject RightBorder;

    public Animator anim;
    public Rigidbody2D rb2D;
    public Transform tran;

    public Enemys()
    {
        mName = "DEFAULT";
        isDead = false;
        flip = true;
        mHealth = 1;
        mDamage = 1;
        mSpeed = 1;
        direction = 1;
    }

    public void Start()
    {
        anim = this.GetComponent<Animator>();
        rb2D = this.GetComponent<Rigidbody2D>();
        tran = this.GetComponent<Transform>();
    }

    public void Update()
    {
        if (mHealth <= 0 && isDead == false)
        {
            isDead = true;
        }
    }

    public void FixedUpdate()
    {
        if (isDead)
        {
            if (!anim.GetBool("Die"))
            {
                anim.SetBool("Die", true);
                Destroy(transform.parent.gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
            }

        }
    }

    public void Move()
    {
        if(!anim.GetBool("Move"))
        {
            anim.SetBool("Move", true);
            anim.speed = mSpeed/3;
        }

        if (flip)
        {
            Flip(direction);
        }

        if (direction * rb2D.velocity.x <= mSpeed)
        {
            rb2D.AddForce(Vector2.right * direction * moveForce);
        }

        if (Mathf.Abs(rb2D.velocity.x) > mSpeed)
        {
            rb2D.velocity = new Vector2(Mathf.Sign(rb2D.velocity.x) * mSpeed, rb2D.velocity.y);
        }
    }

    public void ChangeDirection()
    {
       if(this.tran.position.x <= LeftBorder.transform.position.x && direction == -1)
        {
            direction = 1;
        }
       if(this.tran.position.x >= RightBorder.transform.position.x && direction == 1)
        {
            direction = -1;
        }
    }

    public void Flip(int Direction)
    {
        if (lookAt == LookAt.right)
        {
            if (direction > 0 && this.GetComponent<SpriteRenderer>().flipX)
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (direction < 0 && !this.GetComponent<SpriteRenderer>().flipX)
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            if (direction < 0 && this.GetComponent<SpriteRenderer>().flipX)
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (direction > 0 && !this.GetComponent<SpriteRenderer>().flipX)
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }


    void OnCollisionEnter2D(Collision2D Other)
    {
        if (Other.gameObject.CompareTag("Player")){
            //БЛА-БЛА-БЛА
            
        }
    }
}




