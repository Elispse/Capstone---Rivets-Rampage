using Edgar.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public enum RoomType
    {
        SmallRoom,
        MediumRoom,
        LargeRoom,
        BossRoom
    }
    [SerializeField] public RoomType roomType;
    [SerializeField] private Collider2D[] collider2Ds;
    [SerializeField] private BoolEvent roomCompleteEvent;
    [SerializeField] private BoolEvent roomStartEvent;
    [SerializeField] private VoidEvent endRoomEvent;
    [SerializeField] private GameObject[] enemyList;
    [SerializeField] private List<GameObject> enemies;

    private Utility utility = new Utility();
    private bool roomComplete = true;
    private bool triggerActivated = false;
    private int numberOfEnemies;
    private Vector3 spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && roomComplete == true && !triggerActivated)
        {
            if (roomType == RoomType.SmallRoom)
            {
                numberOfEnemies = Random.Range(1, 3);
            }
            else if (roomType == RoomType.MediumRoom)
            {
                numberOfEnemies = Random.Range(2, 5);
            }
            else if (roomType == RoomType.LargeRoom)
            {
                numberOfEnemies = Random.Range(4, 8);
            }
            else if (roomType == RoomType.BossRoom)
            {
                numberOfEnemies = 1;
            }
            for (int i = 0; i < numberOfEnemies; i++)
            {
                int rand = Random.Range(0, (enemyList.Length));
                EnemySpawnLocation();
                GameObject enemy = Instantiate(enemyList[rand], spawnPoint, gameObject.transform.rotation);
                enemies.Add(enemy);
            }
            roomComplete = false;
            roomCompleteEvent.RaiseEvent(false);
            roomStartEvent.RaiseEvent(true);
            triggerActivated = true;
        }
    }
    private void FixedUpdate()
    {
        if (enemies.Count == 0 && roomComplete == false)
        {
            roomComplete = true;
            roomCompleteEvent.RaiseEvent(true);
            if (transform.GetChild(3).name == "End Portal")
            {
                endRoomEvent.RaiseEvent();
            }
            AudioManager.instance.PlayOneShot(FMODEvents.instance.roomComplete, this.transform.position);
        }
        EnemyDead();
    }

    private void EnemySpawnLocation()
    {
        RaycastHit2D hit;
        GameObject parent = GetComponentInParent<DoorsGrid2D>().gameObject;
        TilemapCollider2D tileMap = parent.GetComponentInChildren<TilemapCollider2D>();
        Vector3 point = utility.GetRandomDestination(tileMap.bounds);
        hit = Physics2D.Raycast(point, Vector2.up);
        if (hit.collider != null && hit.collider.gameObject.name == "Floor" && hit.collider == tileMap && hit.collider.gameObject != GameObject.Find("RoomManager").GetComponentInChildren<BoxCollider2D>())
        {
            spawnPoint = hit.point;
        }
        else
        {
            EnemySpawnLocation();
        }
    }

    private void EnemyDead()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].gameObject == null)
            {
                enemies.Remove(enemies[i]);
            }
        }
    }

    public void ClearAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i]);
            enemies.Remove(enemies[i]);
        }
    }
}