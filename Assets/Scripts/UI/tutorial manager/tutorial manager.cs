using UnityEngine;
using System.Collections;

public class tutorialmanager : MonoBehaviour
{
    public GameObject[] images;
    public GameObject base_image;
    public float activeTime = 6f; 

    private void Start()
    {
        StartCoroutine(ShowImagesOneByOne());
    }

    private IEnumerator ShowImagesOneByOne()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(true);

            yield return new WaitForSeconds(activeTime);

            images[i].SetActive(false);
        }
        base_image.SetActive(false);
    }
}
