using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalState : MonoBehaviour
{
    protected Animal animal;

    public AnimalState(Animal animal)
    {
        this.animal = animal;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
