using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CircleCollider2D radius;
    private EventInstance explosionSound;


    // Start is called before the first frame update
    void Start()
    {
        animator.ResetTrigger("Explode");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            animator.SetTrigger("Explode");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.barrelExplosion, this.transform.position);
        }
    }
    
    public void destroyBarrel()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    private void KnockBack()
    {

    }
}
