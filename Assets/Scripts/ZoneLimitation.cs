using System.Collections;
using UnityEngine;

public class ZoneLimitation : MonoBehaviour
{
    private ParticleSystem[] lines;
    private Collider collider;

    private void Start()
    {
        lines = GetComponentsInChildren<ParticleSystem>();
        collider = GetComponent<Collider>();
        BarrierActive(false);

        GetComponent<MeshRenderer>().enabled = false;
    }

    private void BarrierActive(bool active)
    {
        foreach(var line in lines)
        {
            if (active)
            {
                line.Play();
            }else
            {
                line.Stop();
            }
        }

        collider.isTrigger = !active;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(BarrierCoroutine());
        Debug.Log("I shouldn't go further");
    }

    private IEnumerator BarrierCoroutine()
    {
        BarrierActive(true);

        yield return new WaitForSeconds(1);

        BarrierActive(false);
    }
}
