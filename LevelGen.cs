using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    public GameObject[] sections;
    public GameObject terrain;
    public int zPos = 179, section, previousSection;
    public bool creatingSection = false;
    public float ogX = 3.538466f, ogY = 8.8f;

    // Update is called once per frame
    void Update()
    {
        if (creatingSection == false)
        {
            creatingSection = true;
            StartCoroutine(spawnSection());
        }
    }

    IEnumerator spawnSection()
    {
        do
        {
            section = Random.Range(0, 3);
          //  Debug.Log($"rechoosing");
        } while (section == previousSection);

        Instantiate(sections[section], new Vector3(ogX, ogY, zPos), Quaternion.identity);
        Instantiate(terrain, new Vector3(ogX, ogY, zPos), Quaternion.identity);

        previousSection = section;
        zPos += 162;
     //   Debug.Log($"Spawning: {sections[section].name} and terrain at zPos: {zPos}");
        yield return new WaitForSeconds(10);
        creatingSection = false;
    }
}
