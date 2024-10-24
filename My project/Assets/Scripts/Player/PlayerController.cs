using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* Cromosoma */
    private (int, int, int, int)[] chromosome;
    private int currentGene = 0;
    private float fitness = 0f;
    private bool isDead = false;
    private bool isFinish = false;

    /* Variables */
    private int velocity = 0;
    private float moveCooldown = 0f;
    private float penality = 0f;

    /* Algoritmo */
    private GeneticController geneticController;

    /* Otros */
    private Rigidbody2D rb;
    private Checkpoint checkpoint;
    private CheckpointsManager checkpointsManager;
    
    /* Referencias a objetos */
    void Awake()
    {
        geneticController = FindAnyObjectByType<GeneticController>();
        checkpointsManager = FindAnyObjectByType<CheckpointsManager>();
    }

    /* Inicializa variables */
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = geneticController.GetVelocity();
        penality = geneticController.GetPenality();
        moveCooldown = geneticController.GetMoveCooldown();
    }

    /* Manejador del movimiento del individuo */
    void Update()
    {
        moveCooldown -= Time.deltaTime;
        if (moveCooldown <= 0 && currentGene == chromosome.Length - 1)
        {
            isDead = true;
        }
        if (moveCooldown <= 0 && !isDead && !isFinish)
        {
            Move(chromosome[currentGene]);
            fitness = Vector3.Distance(transform.position, checkpoint.transform.position);
        }
    }
    
    /* Inicializa los genes de la primera generacion */
    public void InitializeGenes()
    {
        chromosome = new (int, int, int, int)[geneticController.GetChromosomeLength()];
        for (int i = 0; i < geneticController.GetChromosomeLength(); i++){
            int randomNumber = Random.Range(0, 4);
            if (randomNumber == 0) {
                chromosome[i] = (1, 0, 0, 0);
            } else if (randomNumber == 1) {
                chromosome[i] = (0, 1, 0, 0);
            } else if (randomNumber == 2) {
                chromosome[i] = (0, 0, 1, 0);
            } else {
                chromosome[i] = (0, 0, 0, 1);
            }
        }
    }

    /* Movimiento del individuo */
    void Move((int, int, int, int) movement)
    {
        if (movement.Item1 == 1){
            rb.velocity = new Vector2(rb.velocity.x, velocity);
        } else if (movement.Item2 == 1){
            rb.velocity = new Vector2(velocity, rb.velocity.y);
        } else if (movement.Item3 == 1){
            rb.velocity = new Vector2(rb.velocity.x, velocity*-1);
        } else if (movement.Item4 == 1){
            rb.velocity = new Vector2(velocity*-1, rb.velocity.y);
        }
        currentGene++;
        moveCooldown = geneticController.GetMoveCooldown();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        /* Interaccion del individuo con los obstaculos */
        if (other.gameObject.CompareTag("Obstacle"))
        {
            fitness *= 1 + penality;

            isDead = true;

            Destroy(rb);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /* Interaccion del individuo con el checkpoint */
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            if (checkpointsManager.IsTheEnd())
            {
                isFinish = true;
            }
            else 
            {
                fitness *= 1 - penality;
                checkpointsManager.NextCheckpoint();
                checkpointsManager.LoadCheckpoints();
            }
            
            Destroy(other.gameObject);
        }
    }

    public (int, int, int, int)[] GetChromosome() => chromosome;
    public void SetChromosome((int, int, int, int)[] newGenes) => chromosome = newGenes;
    public bool GetIsDead() => isDead;
    public bool GetIsFinish() => isFinish;
    public float GetFitness() => 1 / fitness;
    public void SetCheckpoint(Checkpoint value) => checkpoint = value;
}
