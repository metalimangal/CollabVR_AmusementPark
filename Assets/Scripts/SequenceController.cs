using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    public List<GameObject> templates;

    private int templateIndex;

    // Start is called before the first frame update
    void Start()
    {
        for (int i=1; i < templates.Count; i++)
        {
            templates[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(templateIndex < templates.Count)
        {
            if (!templates[templateIndex].activeSelf)
            {
                templateIndex++;
                templates[templateIndex].SetActive(true);
            }
        }

    }
}
