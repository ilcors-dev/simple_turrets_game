using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private GameObject bar;
    // Start is called before the first frame update
    void Start()
    {
        bar = gameObject;
    }
    
    public void SetSize(float sizeNormalized)
    {
        bar.transform.localScale = new Vector3(sizeNormalized, 1f);
    }
}
