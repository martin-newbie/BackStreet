using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform player;
    public Map map;
    List<Map> mapList = new List<Map>();


    Map center;
    Map nearest;

    void Start()
    {
        SpawnTile(-1, 1);
        SpawnTile(-1, 0);
        SpawnTile(-1, -1);
        SpawnTile(1, 1);
        SpawnTile(1, 0);
        SpawnTile(1, -1);
        SpawnTile(0, 1);
        center = SpawnTile(0, 0);
        SpawnTile(0, -1);
        map.gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckCenter();
    }

    void CheckCenter()
    {
        nearest = mapList.OrderBy((item) => Vector3.Distance(player.position, item.transform.position)).ElementAt(0);
        if (nearest != center)
        {
            float x_movement = (nearest.transform.position.x - center.transform.position.x);
            float y_movement = (nearest.transform.position.y - center.transform.position.y);

            foreach (var item in mapList)
            {
                item.transform.position += new Vector3(x_movement, y_movement);
            }
        }
    }

    Map SpawnTile(int x, int y)
    {
        Map obj = Instantiate(map, transform);
        obj.transform.position = GetPositionOfTile(x, y);
        obj.InitMap(x, y);
        mapList.Add(obj);
        return obj;
    }

    Vector3 GetPositionOfTile(int x, int y)
    {
        return new Vector3(x * 20, y * 20);
    }
}
