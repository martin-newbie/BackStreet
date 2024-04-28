using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InGameManager : MonoBehaviour
{

    public static InGameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Transform camTr;
    public Player curPlayer;
    public Transform crossHair;
    public ExpItem expItemPrefab;

    [Header("Interface")]
    public ExpGaugeBox expGauge;
    public HpGaugeBox hpGauge;
    public Text playTimeText;

    [HideInInspector] public float playTime;
    TimeSpan prevTime;

    void Start()
    {
    }

    void Update()
    {
        CrossHairFollow();
        CameraFollow();
        UpdatePlayTime();
    }

    void CrossHairFollow()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        crossHair.position = pos;
    }

    void CameraFollow()
    {
        var finalPos = curPlayer.transform.position;
        finalPos.z = -10;
        camTr.position = Vector3.Lerp(camTr.position, finalPos, Time.deltaTime * 10f);
    }

    void UpdatePlayTime()
    {
        playTime += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(playTime);
        if (prevTime.Seconds != time.Seconds)
        {
            playTimeText.text = time.ToString(@"mm\:ss");
        }
    }

    public ExpItem SpawnExpItem(Vector3 pos)
    {
        var obj = Instantiate(expItemPrefab, pos, Quaternion.identity);
        return obj;
    }

    public void IncreaseExp(int exp)
    {
        curPlayer.IncreaseExp(exp);
    }

    public int GetDrawOrder(int yPos)
    {
        return (int)curPlayer.transform.position.y - yPos;
    }
}
