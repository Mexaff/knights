using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character //общий класс для врагов
{
    [SerializeField]
    protected bool moving = false; //двигается ли персонаж
    public bool Moving { get => moving; set => moving = true; }
    [SerializeField]
    private float patrolDistance = 1; //дистанция патрулирования
    [SerializeField]
    private float visionDistance = 1; //дистанция "зрения"

    private Vector3 visionOrigin; //источник луча, откуда враг "смотрит"
    private float visionUpdate = 0.5f; //время обновления луча
    private float visionUpdateCurrent = 1; //текущее время обновления

    protected float direction = 1; //направление патрулирования
    private float xTransform; //исходная позиция патрулирования

    private new void Start()
    {
        xTransform = transform.position.x; //установка исходной позиции
        var c = GetComponent<Collider2D>(); //получение коллайдера
        visionOrigin = new Vector3(c.bounds.max.x + 0.1f - transform.position.x, c.bounds.center.y - transform.position.y); //находится немного правее края персонажа
        base.Start(); //вызов родительского метода Start
    }

    protected void Update() //вызывается каждый кадр
    {
        if (moving)
        {
            Move(direction); //передвижение
        }

        //механика патрулирования
        if (Mathf.Abs(transform.position.x - xTransform) > patrolDistance) //если враг вышел за зону патрулирования
        {
            if (transform.position.x - xTransform > 0) direction = -1;     //он разворачивается
            else direction = 1;
        }

        if (visionUpdateCurrent <= 0) //проверка, есть ли впереди игрок
        {
            var hit = Physics2D.Raycast(transform.position + visionOrigin*direction, Vector2.right*direction, visionDistance);
            if (hit && !dead && hit.collider.CompareTag("Player")) Attack(); //если игрок присутствует, враг атакует
            visionUpdateCurrent = visionUpdate; //сброс счётчика "зрения"
        }
        visionUpdateCurrent -= Time.deltaTime; //обновление счётчика "зрения"
    }

    public override void Die()
    {
        base.Die(); //вызов анимации
        moving = false; //отмена передвижения
        rb.constraints = RigidbodyConstraints2D.FreezeAll; //заморозка позиции
    }



    public void Despawn() //вызывается после анимации смерти (Unity Animation Event)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; //разморозка позиции (вращение заморожено по умолчанию)
        gameObject.SetActive(false); //отключение объекта
    }

    private void OnTriggerEnter2D(Collider2D collider) //вызывается Unity при пересечении коллайдера триггером
    {
        if (!dead)
            if (collider.CompareTag("trap") || collider.CompareTag("playerAttack")) { Die(); } //враг умирает при попадании на атаку игрока или ловушку
    }
}
