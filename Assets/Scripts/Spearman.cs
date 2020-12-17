using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Enemy //враг, который при атаке бросается на игрока
{
    [SerializeField]
    private float force = 1; //сила броска

    public override void Attack()
    {
        moving = false; //отключает передвижение (иначе скорость сразу заменится на обычную скорость передвижения)
        rb.AddForce(Vector2.right * force * direction); //применяет силу броска
        base.Attack(); //вызывает анимацию атаки
    }

    public void SetMoving()
    {
        moving = true; //включает передвижение обратно
    }

}
