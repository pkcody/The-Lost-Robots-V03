using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudInhaler : MonoBehaviour
{
    public bool canInhale = false;
    public bool inhaleButtonPressed = false;
    public Transform tipOfInhaler;
    public float inhaleSpeed = 1f;
    public float cloudDestroyDistance = .3f;
    public Slider cloudInhaleSlider;

    public List<Cloud> currInhalingClouds = new List<Cloud>();

    //particle
    public GameObject particleInhale;

    private void Start()
    {
        particleInhale.SetActive(false);

    }

    IEnumerator InhaleParticleTime()
    {
        particleInhale.SetActive(true);

        yield return new WaitForSeconds(.5f);
        particleInhale.SetActive(false);

    }

    public void InhaleClouds()
    {
        if (canInhale)
        {
            StartCoroutine(InhaleParticleTime());

            try
            {
                foreach (Cloud cloud in currInhalingClouds)
                {
                    print("clouds being inhaled");
                    cloud.transform.position = Vector3.MoveTowards(cloud.transform.position, tipOfInhaler.position, inhaleSpeed);
                    cloud.transform.GetChild(0).localScale = new Vector3(cloud.transform.GetChild(0).localScale.x / 1.5f, cloud.transform.GetChild(0).localScale.y / 1.5f, cloud.transform.GetChild(0).localScale.z / 1.5f);

                    if ((cloud.transform.position - tipOfInhaler.position).magnitude < cloudDestroyDistance)
                    {
                        currInhalingClouds.Remove(cloud);
                        Destroy(cloud.gameObject);

                        cloudInhaleSlider.value += 1;

                    }
                    

                }

            }
            catch
            {
                print("Unable to inhale");

            }

        }

    }



    


}
