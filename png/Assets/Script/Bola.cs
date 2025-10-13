using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bola : MonoBehaviour
{

    public float velocidadeDaBola;

    public float direcaoAleatoriaX;

    public float direcaoAleatoriaY;


    public Rigidbody2D oRigidyBody2D;
    
    public AudioSource somDaBola;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoverBola();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MoverBola(){
        oRigidyBody2D.linearVelocity = new Vector2(velocidadeDaBola, velocidadeDaBola);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo){
        somDaBola.Play();
        oRigidyBody2D.linearVelocity += new Vector2(direcaoAleatoriaX, direcaoAleatoriaY);
    }
}
