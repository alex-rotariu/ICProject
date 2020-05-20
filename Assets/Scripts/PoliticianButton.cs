using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliticianButton : MonoBehaviour
{
    private string[] politicianImages = { "Dragnea", "Basescu", "Viorica", "Robu", "Vadim", "Boc", "Firea", "Klaus", "Mazare", "Olguta", "Ponta", "Orban", "Tariceanu", "Boldea"  };
    private string politicianPath = "Images/Politicians/";
    Stats stats;

    void Start()
    {
        SetPolitician(ScenesData.GetPlayer().character);
        stats = FindObjectOfType<Stats>();
    }

    public void SetPolitician(int index)
    {
        Sprite politician = Resources.Load<Sprite>(politicianPath + politicianImages[index]);
        GetComponent<Image>().sprite = politician;
    }

    public void OnClick() {
        stats.addMoney();
        StartCoroutine(ShrinkButton());
    }

    private IEnumerator ShrinkButton() {
        transform.localScale = new Vector2(0.85f, 0.85f);
        yield return new WaitForSeconds(0.025f);
        transform.localScale = new Vector2(1f, 1f);
    }
}
