using UnityEngine;
using System.Collections;



public abstract class EnemyBase : MonoBehaviour {

    private SpriteRenderer  _spriteRenderer;
    private Animator        _anim;
    private Rigidbody2D     _rigidbody2D;
    private Transform       _tran;


    [Header("FLIP")]
    [SerializeField]
    protected bool flip;                       //Разворачивать спарйт при движении по Х?
    [SerializeField]
    protected StartRotation startRotation;     //Куда изнасчально развернут спрайт по Х?

    protected CurrentRotation currentRotation; //Куда смотрит спрайт?

    public EnemyBase()
    {
    }

    public virtual void Awake()
    {
       _spriteRenderer = (SpriteRenderer)CheckComponent(GetComponent<SpriteRenderer>());
       _anim           = (Animator)CheckComponent(GetComponent<Animator>());
       _rigidbody2D    = (Rigidbody2D)CheckComponent(GetComponent<Rigidbody2D>());
       _tran           = (Transform)CheckComponent(GetComponent<Transform>());
    }



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
        else if (startRotation == StartRotation.LEFT)
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

    private Component CheckComponent(Component Component)
    {
        if (Component)
        {
            return Component;
        }
        else
        {
            Debug.Log(this.gameObject +" "+"SOME COMPONENT NOT FOUND");
            return null;
        }
    }
    #endregion
}




