using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour //общий класс для персонажей (игрок, враги)
{
    [SerializeField]
    private float speed = 1; //скорость движения персонажа
    [SerializeField]
    private float jumpForce = 1; //высота прыжка
    [SerializeField]
    private GameObject attack = null; //триггер атаки

    private bool forward; //двигается ли персонаж вперед (для поворота спрайта)
    private bool inAir; //находится ли персонаж в воздухе

    protected bool dead = false; //мёртв ли персонаж
    public bool Dead { get => dead; set => dead = value; }

    //компоненты Unity
    private Animator anim = null;
    private Collider2D coll = null;
    protected Rigidbody2D rb = null;

    protected void Start() //вызывается Unity при загрузке сцены
    {
        //получение компонентов объекта из Unity
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    public void Move(float direction) //метод передвижения, вызывается из дочерних классов
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y); //перезаписывает скорость в направлении
        //код вращения спрайта (персонаж смотрит куда идёт)
        if (!forward && rb.velocity.x < 0)
        {
            transform.Rotate(0, 180, 0); forward = true;
        }
        else if (forward && rb.velocity.x > 0)
        {
            transform.Rotate(0, 180, 0); forward = false;
        }

        //вызов анимации если персонаж двигается
        if (Mathf.Abs(rb.velocity.x) > 0.01f) anim.SetBool("move", true);
        else anim.SetBool("move", false);

        //метод сброса прыжка
        if (inAir && CheckGround())
        {
            inAir = false;
            anim.SetBool("jumpend", true);
        }
        else if (inAir) anim.SetBool("jumpend", false);

    }

    public virtual void Attack()
    {
        //вызов анимации атаки
        anim.SetTrigger("attack");
    }


    //методы вызываются из анимации атаки с помощью Animation Event
    //включают и выключают триггер атаки
    public void ActivateAttack() { attack.SetActive(true); }
    public void DisableAttack() { attack.SetActive(false); }

    public virtual void Die()
    {
        //общий метод смерти персонажей
        dead = true;
        anim.SetTrigger("death");
    }

    public void Jump() //прыжок
    {
        anim.SetTrigger("jump"); //вызов анимации
        inAir = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); //применение силы прыжка
    }

    public bool CheckGround() //проверка наличия земли под ногами
    {
        //выпускается луч от ног персонажа вниз, если он что то встретил с тэгом "ground", возвращает true
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(coll.bounds.center.x, coll.bounds.min.y - 0.01f), Vector2.down, 0.05f);
        if (hit && hit.collider.CompareTag("ground")) return true;
        else return false;
    }
}
