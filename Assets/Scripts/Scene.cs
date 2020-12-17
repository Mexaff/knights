using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    //оба метода вызываются по клику на кнопки
    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index); //смена сцены
    }

    public void Quit()
    {
        Application.Quit(); //выход
    }
}
