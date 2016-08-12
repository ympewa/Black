using UnityEngine;
using System.Collections;

public abstract class WeaponsBase : MonoBehaviour {

    public Rigidbody2D projectile;  //Префаб снаряда

    [SerializeField]
    public float speed;


    [SerializeField]
    protected string weaponName;   //Название
    [SerializeField]
    protected float damage;        //Урон
    [SerializeField]
    public float AttackRate;     //Скорость стрельбы.
    private float delay;

    protected WeaponsBase()
    {
        weaponName  = "NULL";
        damage      = 1f;
        AttackRate  = 0.5f;
        speed       = 30f;
    }
}
