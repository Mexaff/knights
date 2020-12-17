using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : Enemy //враг, который становится неуязвимым при атаке на несколько секунд
{
    private float invincible; //флаг неуязвимости

    public override void Attack()
    {
        invincible = 2; //атака накладывает две секунды неуязвимости
        base.Attack();
    }

    private new void Update()
    {
        if (invincible > 0) invincible -= Time.deltaTime; //обновление счётчика неуязвимости
        GetComponent<SpriteRenderer>().color = new Color(1, 1 - invincible / 2, 1-invincible/2, 1); //меняет цвет чтобы показать что враг неуязвим
        base.Update(); //вызов родительского Update
    }

    public override void Die()
    {
        if (invincible <= 0) base.Die(); //персонаж умирает только если он уязвим
    }
}
