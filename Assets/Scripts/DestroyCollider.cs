using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollider : MonoBehaviour
{
    public bool finalLvl = false;

    private BoxCollider col;
    void Start()
    {
        col = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        int length = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        length = enemies.Length;

        if(finalLvl) {
            length--;
        }

        if (length <= 1) {
            Destroy(col);
            Destroy(this);
        }
    }
}
