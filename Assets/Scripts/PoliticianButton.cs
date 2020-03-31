using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliticianButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = ScenesData.GetPolitician();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick() {
        StartCoroutine(ShrinkButton());
        
    }

    private IEnumerator ShrinkButton() {
        transform.localScale = new Vector2(0.85f, 0.85f);
        yield return new WaitForSeconds(0.02f);
        transform.localScale = new Vector2(1f, 1f);
    }
}
