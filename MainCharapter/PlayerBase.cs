using UnityEngine;
using System.Collections;

public enum StartRotation
    {
        LEFT,
        RIGHT
    }

public enum CurrentRotation
{
    FaceLEFT  =-1,
    FaceRIGHT = 1
}

public enum WeaponRotation
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public abstract class PlayerBase : MonoBehaviour {

    [HideInInspector]
    public WeaponsList weapons;                //Оружия.
    protected Rigidbody2D _rigidbody2D;        //Ригидбоди.
    protected SpriteRenderer _spriteRenderer;  //Спрайтрендер.
    protected Collider2D _collider2D;          //Коллайдер.
    protected Animator _animator;              //Аниматор.
    protected GroundCheck _groundChecker;        //Граундчекер.

    private bool onMovingPlatform;           //Находится на платформе?


    [Header("MOVE SETTING")]
    [SerializeField]
    private int moveForce;                     //Сила для движения.
    [SerializeField]
    protected float horizontalSpeed;           //Скорость передвижения по x.
    [SerializeField]
    public Vector2 velosity;

    private bool canMove;                    //Может двигаться?
    [SerializeField]
    private int mDirection;                  //Направление движения.
    private bool MOVING;                       //Флаг для движения.
 
    [Header("JUMP SETTING")]
    [SerializeField]
    protected LayerMask ground;                //Лайрмаск дял земли.
    [SerializeField]
    protected float jumpPower;                 //Сила прыжка.
    [SerializeField]
    protected bool jumpEnable;                 //Вкл. прыжка.
    [SerializeField]
    protected bool multipleJumpEnable;         //Вкл. двойного прыжка.
    [SerializeField]
    protected int avilableJump;                
    [SerializeField]
    protected int secondJumpPower;

    //protected int verticalSpeed;             //Скорость передвижения по y.
    private bool JUMPING;                      //Флаг для пыржка.
    private bool MULTIPLEJUMPING;                //Флаг для двойного прыжка.
    private bool canDoubleJump;
    private bool canJump;
    private int jumpCount;
    private float maxVerticalVelo;

    [Header("ATTACK AND WEAPON SETTING")]
    protected float delayBetwenAttack;
    protected WeaponRotation weaponRotation;
    protected Vector2 weaponsPosition;

    private bool ATTACKING;

    [Header("OTHER")]
    [SerializeField]
    protected float invisibleLenght;           //Кадры неуязвимости после столкнавения с врагом.


    [Header("FLIP")]
    [SerializeField]
    protected bool flip;                       //Разворачивать спарйт при движении по Х?
    [SerializeField]
    protected StartRotation startRotation;     //Куда изнасчально развернут спрайт по Х?

    protected CurrentRotation currentRotation; //Куда смотрит спрайт?

    //DEFOULT
    protected PlayerBase()
    {   
        canMove              = true;
        moveForce            = 50;
        horizontalSpeed      = 5;

        jumpPower            = 500;
        multipleJumpEnable     = false;
        secondJumpPower      = 400;
        avilableJump         = 2;
        jumpCount            = avilableJump;
        maxVerticalVelo      = horizontalSpeed * 2;

        invisibleLenght      = 3f;
        
        flip                 = true;
        startRotation        = StartRotation.RIGHT;
    }


    public virtual void Awake () {
        //INICIALIZATION
        #region
        if (this.GetComponent<Rigidbody2D>())
        {
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.Log(this.gameObject + " " + "LOST RIGIDBODY2D");
        }

        if (this.GetComponent<SpriteRenderer>())
        {
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.Log(this.gameObject + " " + "LOST SPRITERENDERER");
        }

        if (this.GetComponent<Collider2D>())
        {
           _collider2D = this.GetComponent<Collider2D>();
        }
        else
        {
            Debug.Log(this.gameObject + " " + "LOST COLLIDER2D");
        }

        if (this.GetComponent<Animator>())
        {
            _animator = this.GetComponent<Animator>();
        }
        else
        {
            Debug.Log(this.gameObject + " " + "LOST ANIMATOR");
        }

        if (this.transform.FindChild("GroundChecker")){
            _groundChecker = this.transform.FindChild("GroundChecker").gameObject.GetComponent<GroundCheck>();
        }
        else
        {
            Debug.Log(this.gameObject + " " + "LOST GROUNDCHECKER");
        }

        if (this.transform.FindChild("Weapons"))
        {
            weapons = this.transform.FindChild("Weapons").gameObject.GetComponent<WeaponsList>();
            weaponsPosition = weapons.transform.localPosition;
        }
        else
        {
            Debug.Log("LOST WEAPONS GAMEOBJECT");
        }

        if(startRotation == StartRotation.LEFT)
        {
            currentRotation = CurrentRotation.FaceLEFT;
        }
        else if(startRotation == StartRotation.RIGHT)
        {
            currentRotation = CurrentRotation.FaceRIGHT;
        }    
        #endregion
    }

    //СУКА, НЕ ПИХАЙ ИНПУТ И ОПРОСЫ В FixedUpdate()
    public virtual void Update()
    {
        MovingPlatform();

        //mDirection = (int)Input.GetAxis("Horizontal");
        //Вращение оружия.
        WeaponRotate(weapons.gameObject, (int)Input.GetAxis("Horizontal"));
        //Флип, если нужен.
        if (flip && Input.GetAxis("Horizontal") != 0)
        {
            Flip(Input.GetAxis("Horizontal"));
        }
        //Огроничение по скорсоти.
        if (MOVING)
        {
            ClampSpeed();
        }
        //Можно ли прыгать и сброс прыжков.
        if(_groundChecker.onGround && !canJump)
        {
            canJump = true;
            if (multipleJumpEnable)
            {
                jumpCount = avilableJump;
            }
        }
    }
	
    public virtual void FixedUpdate()
    {
        velosity = _rigidbody2D.velocity;

        //ДВИЖЕНИЕ ПО Х:
        if (MOVING)
        {
            PlayerRun();
        }
        //ПРЫЖОК:
        if (JUMPING && jumpEnable)
        {
            Jump();
            JUMPING = false;
        }
        //АТАКА:
        if (ATTACKING && Time.time > delayBetwenAttack)
        {
            Attack(weapons.currentWeapon.GetComponent<WeaponsBase>());
        }
    }

    //JUMP
    #region
    protected void Jumping()
    {
        JUMPING = true;
    }

    private void Jump()
    {
        if (canJump)
        {
            var force = Force();

            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            _rigidbody2D.AddForce(force);

            if (multipleJumpEnable)
            {
                jumpCount -= 1;
                if (jumpCount == 0)
                {
                    canJump = false;
                }
            }
            else
            {
                canJump = false;
            }     
        }
    }
    #endregion

    //MOVE
    #region
    protected void Moving(int Direction)
    {
        if(Direction != 0)
        {
            MOVING = true;
        }
        else
        {
            MOVING = false;
        }
    }

    private void PlayerRun()
    {
        if (canMove)
        {
            _rigidbody2D.AddRelativeForce(Vector2.right * (int)Input.GetAxis("Horizontal") * moveForce * horizontalSpeed);
        }       
    }
    #endregion

    //ATTACK
    #region
    protected void Attacking(bool Enable)
    {
        if (Enable)
        {
            ATTACKING = true;
        }
        else
        {
            ATTACKING = false;
        }
    }

    private void Attack(WeaponsBase Weapon)
    {
        delayBetwenAttack = Time.time + Weapon.AttackRate;
        Vector2 point = (Vector2)weapons.transform.position;
        var yDir = Input.GetAxis("Vertical");
        var xDir = Input.GetAxis("Horizontal");

        Rigidbody2D clone = Instantiate(Weapon.projectile, point, Quaternion.identity) as Rigidbody2D;
        Physics2D.IgnoreCollision(_collider2D, clone.gameObject.GetComponent<Collider2D>());
        if (yDir == 0)
        {
            clone.velocity = new Vector2((int)currentRotation, 0) * Weapon.speed;
        }
        else
        {
            clone.velocity = new Vector2(xDir, yDir) * Weapon.speed;
        }
        Destroy(clone.gameObject, 2.5f);

    }
    #endregion

    //OTHER
    #region
    private void Flip(float Direction)
    {
        if (startRotation == StartRotation.RIGHT)
        {
            if (Direction > 0 && _spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = false;
                currentRotation = CurrentRotation.FaceRIGHT;
                //weaponRotation = WeaponRotation.RIGHT;
                
                
            }
            if (Direction < 0 && !_spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = true;
                currentRotation = CurrentRotation.FaceLEFT;
                //weaponRotation = WeaponRotation.LEFT;
            }
        }
        else if(startRotation == StartRotation.LEFT)
        {
            if (Direction < 0 && _spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = false;
                currentRotation = CurrentRotation.FaceLEFT;
                //weaponRotation = WeaponRotation.LEFT;
            }
            if (Direction > 0 && !_spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = true;
                currentRotation = CurrentRotation.FaceRIGHT;
                //weaponRotation = WeaponRotation.RIGHT;
            }
        }
    }

    private void FreeRoad(int dir)
    {
        float sizeX = _collider2D.bounds.size.x*1.015f;
        float sizeY = _collider2D.bounds.size.y*1.015f;

        float rayCount = sizeY / 10;
        float startPoint = -sizeY / 2;
        float lastPoint = sizeY / 2;

        int layerMask = 1 << 9;
        layerMask = ~layerMask;
        RaycastHit2D hit;

        for (var i = startPoint; i <= lastPoint; i+= rayCount) {
            Vector2 point = (Vector2)this.transform.position + new Vector2(sizeX / 2 * dir, i);
            hit = Physics2D.Raycast(point, Vector2.right * dir, 0.1f, layerMask);
            Debug.DrawRay(point, Vector2.right * dir*0.1f, Color.red);

            if (hit.collider != null)
            {
                canMove = false;
                break;
            }
            else
            {
                canMove = true;
            }
        }
    }
    
    private void WeaponRotate(GameObject WeaponsPosition, int Direction)
    {
        if (Direction > 0 && weaponRotation != WeaponRotation.RIGHT)
        {
            weapons.transform.localPosition = weaponsPosition;
            weaponRotation = WeaponRotation.RIGHT;
        }

        if (Direction < 0 && weaponRotation != WeaponRotation.LEFT)
        {        
            weapons.transform.localPosition = new Vector2(weapons.transform.localPosition.x * -1, weapons.transform.localPosition.y);
            weaponRotation = WeaponRotation.LEFT;
        }
    }
    
    private void ClampSpeed()
    {
        if(Mathf.Abs(_rigidbody2D.velocity.x) > horizontalSpeed)
        {
            _rigidbody2D.velocity = new Vector2(Mathf.Clamp(_rigidbody2D.velocity.x, -horizontalSpeed, horizontalSpeed),
                                                _rigidbody2D.velocity.y);
        }

        if (Mathf.Abs(_rigidbody2D.velocity.y) > maxVerticalVelo)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,
                                                Mathf.Clamp(_rigidbody2D.velocity.y, -maxVerticalVelo, maxVerticalVelo));
        }
    }

    private Vector2 Force()
    {
        if(jumpCount == avilableJump)
        {
            return new Vector2(0, jumpPower);
        }
        else
        {
            return new Vector2(0, secondJumpPower);
        }
    }

    private void MovingPlatform()
    {
        if (_groundChecker.GetComponent<GroundCheck>().onMovingPlatform)
        {
            if(_groundChecker.GetComponent<GroundCheck>().currentPlatform != null)
            {
                this.transform.parent = _groundChecker.GetComponent<GroundCheck>().currentPlatform.transform;
            }
        }
        else
        {
            this.transform.parent = null;
        }
    }
    #endregion
}



