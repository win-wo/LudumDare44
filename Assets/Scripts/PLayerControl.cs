using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerControl : MonoBehaviour
{

    public UnitBase State;
    public Action DamageCallback;

    public float HorizontalSpeed;
    public float VerticalSpeed;
    public GameObject Projectile;

    Rigidbody2D _rigidbody;
    float _projectileTimer;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    float cooldown = 0;
    void Update()
    {
        if (cooldown <= 0)
        {
            if (Input.GetButton("Fire1"))
            {
                if (Projectile != null)
                {
                    var projectile = Instantiate(Projectile, this.transform.position, Quaternion.identity);
                    var rb = projectile.GetComponent<Rigidbody2D>();
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pz.z = 0;
                    var bulletVel = Vector3.Normalize(pz - this.gameObject.transform.position) * 50 * State.BulletSpeed;
                    rb.AddForce(bulletVel);
                    Destroy(projectile, 2f);
                    cooldown = 1;
                }
            }
        }
        else
        {
            cooldown -= Time.deltaTime * State.RateOfAttack;
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h > 0.01 || h < -0.01)
        {
            gameObject.transform.position = transform.position + transform.right * h * HorizontalSpeed * Time.deltaTime;
            if (h > 0.01)
            {
                gameObject.transform.localScale = new Vector3(2, 2, 1);
            }
            if (h < -0.01)
            {
                gameObject.transform.localScale = new Vector3(-2, 2, 1);
            }
        }
        if (v > 0)
        {
            if (grounds.Count > 0)
            {
                if (_rigidbody.velocity.y == 0)
                {
                    _rigidbody.AddForce(Vector2.up * VerticalSpeed * 50 * State.JumpPower);
                }
            }
        }
    }


    HashSet<Collision2D> grounds = new HashSet<Collision2D>();
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounds.Add(col);
        }
        if (col.gameObject.tag == "Bullet")
        {
            Destroy(col.gameObject);
            //call scene manager
            DamageCallback();
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounds.Remove(col);
        }
    }
}
