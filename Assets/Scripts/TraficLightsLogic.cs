using System.Collections;
using UnityEngine;

public class TraficLightsLogic : MonoBehaviour
{
    [SerializeField] private GameObject lights;
    [SerializeField] private float delayToBlink;

    private void Awake()
    {
        lights.SetActive(false);

        StartCoroutine(LightsBlinkCoroutine(lights));
    }

    private IEnumerator LightsBlinkCoroutine(GameObject lights) 
    {
        lights.SetActive(false);

        while (true)
        {
            yield return new WaitForSeconds(delayToBlink);
            lights.SetActive(true);
            yield return new WaitForSeconds(delayToBlink);
            lights.SetActive(false);
        }
    }
}
