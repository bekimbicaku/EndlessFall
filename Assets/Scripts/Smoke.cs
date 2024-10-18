using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public GameObject smokeEffect;
    private float smokeActive = 3f;


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            if(smokeActive >= 3f)
            {
                smokeActive = 0f;
                smokeEffect.SetActive(true);
                StartCoroutine(DeactiveSmoke());
            }
           
        }
    }

    IEnumerator DeactiveSmoke()
    {
        yield return new WaitForSeconds(10);
        smokeActive = Time.deltaTime;
        smokeEffect.SetActive(false);
    }
}
