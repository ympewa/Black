using UnityEngine;
using System.Collections;

public abstract class BulletBase : MonoBehaviour {
    [Header("COLLISION SETTIG")]
    [SerializeField]
    protected bool destroyWhenStruck;

    public BulletBase()
    {
        destroyWhenStruck = true;
    }

    void Update()
    {
        //Debug.Log(this.GetComponent<Rigidbody2D>().velocity.x);
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (destroyWhenStruck)
        {
            Destroy(this.gameObject);
        }
    }
}
