using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ClickObjectFly : MonoBehaviour
{
    private bool move;
    private MineManager mineManager;
    private Vector2 randomVector;

    private void Start()
    {
        mineManager = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>().transform.parent.transform.GetChild(2).gameObject.GetComponent<MineManager>();
    }

    private void Update()
    {
        if(move)
            transform.Translate(randomVector * Time.deltaTime);
    }

    public void StartMotion()
    {
        transform.localPosition = Vector2.zero;
        GetComponent<Text>().text = "+" + mineManager.OrePerClick;
        randomVector = new Vector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5));
        move = true;
        GetComponent<Animation>().Play();
    }

    public void StartMotion(int x, int y, string text)
    {
        transform.localPosition = Vector2.zero;
        GetComponent<Text>().text = text;
        randomVector = new Vector2(x, y);
        move = true;
        GetComponent<Animation>().Play();
    }

    public void StopMotion()
    {
        move = false;
    }
}
