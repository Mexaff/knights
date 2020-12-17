using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character //класс игрока
{
    [SerializeField]
    private GameObject pause = null; //интерфейс паузы

    //переменные управления
    private float move;
    private bool jump;
    private bool attacking;

    //чекпоинт
    private Vector3 respawn = new Vector3();

    private void Update()
    {
        //каждый кадр вводится управление
        move = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && CheckGround()) //прыгать можно только на земле
        {
            jump = true;
        }
        if (Input.GetButtonDown("Fire1")) attacking = true;
        if (Input.GetButtonDown("Cancel")) pause.SetActive(!pause.activeSelf);
    }

    private void FixedUpdate()
    {
        //если персонаж еще жив
        if (!dead)
        {
            Move(move); //передвижение
            if (jump)
            {
                Jump();
                jump = false; //вызов метода и сброс переменной
            }
            if (attacking)
            {
                Attack();
                attacking = false; //вызов метода и сброс переменной
            }
        }
    }

    public override void Die()
    {
        base.Die(); //вызывает анимацию смерти
        rb.constraints = RigidbodyConstraints2D.FreezeAll; //заморозка позиции
    }

    public void Respawn() //вызывается после анимации смерти, откат к предыдущему чекпоинту
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; //разморозка
        transform.position = respawn; //телепортация на чекпоинт
        dead = false;
        DisableAttack(); //если игрок умрёт во время атаки, атака не будет выключена анимацией, значит нужно её выключить здесь
        
    }



    private void OnTriggerEnter2D(Collider2D collider) //коллайдер пересечен триггером
    {
        if (!dead) //игрок не может умереть дважды
        {
            if (collider.CompareTag("trap") || collider.CompareTag("enemyAttack")) { Die(); } //ловушка или вражеская атака приводят к смерти
            else if (collider.CompareTag("checkpoint")) { respawn = collider.transform.position; collider.gameObject.SetActive(false); } //чекпоинт изменяет позицию респавна
        }
    }



}
