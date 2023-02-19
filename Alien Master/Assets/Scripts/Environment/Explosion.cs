using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float triggerForce = .5f;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float explosionForce = 500f;
    [SerializeField] ParticleSystem explosionEff;
    [SerializeField] GameObject explosionMesh;
    [SerializeField] List<Rigidbody> barrelObjs;
    void Start()
    {
        explosionEff.Stop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            DoExplode();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.relativeVelocity.magnitude >= triggerForce
            && !collision.gameObject.CompareTag("Player")
            && !collision.gameObject.CompareTag("Enemy")
            && !collision.gameObject.CompareTag("Explosive") 
            )
             
        {
            DoExplode();
        }
    }

    public void DoExplode()
    {
        var surroundingObjs = Physics.OverlapSphere(transform.position, explosionRadius);
        GetComponent<Collider>().enabled = false;

        foreach (var obj in surroundingObjs)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) continue;

            if (obj.CompareTag("Explosive"))
            {
                obj.GetComponent<Explosion>().DoExplode();
                continue;
            }

            if (obj.CompareTag("Enemy")) obj.GetComponent<EnemyHealth>().Exploded();
            rb.AddExplosionForce(explosionForce, transform.position, 
                explosionRadius);
        }

        explosionMesh.SetActive(false);
        explosionEff.Play();
        ThrowBarrelObjs();
    }

    void ThrowBarrelObjs()
    {
        int explosivedBarrelLayer = LayerMask.NameToLayer("Explosived Barrel");        
        foreach (Rigidbody obj in barrelObjs)
        {
            obj.isKinematic = false;
            obj.gameObject.layer = explosivedBarrelLayer;
        }
    }


}
