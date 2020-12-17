using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    //класс заставляет камеру следить за игроком
    [SerializeField]
    Transform player = null;
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10); //меняет положение камеры на положение игрока
    }
}
