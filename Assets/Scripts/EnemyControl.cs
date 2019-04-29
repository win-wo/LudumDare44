using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public UnitBase State;
    public int Index;
    public GameObject Target;
    public float Range;
    public GameObject Projectile;
    public Action<int> DamageCallback;
    float _projectileTimer;
    Rigidbody2D _rigidbody;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

      

     float cooldown = 0;
    void Update()
    {
        if (cooldown <= 0)
        {
            if (Vector2.Distance(Target.transform.position, this.gameObject.transform.position) < State.Range)
            {
                var projectile = Instantiate(Projectile, this.transform.position, Quaternion.identity);
                var rb = projectile.GetComponent<Rigidbody2D>();

                var bulletVel = Vector3.Normalize(Target.transform.position - this.gameObject.transform.position) * 50 * State.BulletSpeed;
                rb.AddForce(bulletVel);
                Destroy(projectile, 2f);
                cooldown = 5;
            }
        }
        else
        {
            cooldown -= Time.deltaTime * State.RateOfAttack;
        }
    }

    private void FixedUpdate()
    {
        if (Target != null)
        {
            var targetDirection = Vector3.Normalize(Target.transform.position - this.gameObject.transform.position);
            _rigidbody.AddForce(targetDirection);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Destroy(col.gameObject);
            DamageCallback(Index);
        }
    }
}
