using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] tetrominoes;
    public float spawnInterval = 1f;
    public int minX = -11;
    public int maxX = 11;

    private GameObject currentTetromino;
    private float spawnTimer;
    public AudioSource soundEffect;


    private void Start()
    {
        spawnTimer = spawnInterval;
        SpawnTetromino(null);

        Rigidbody2D tetrominoRigidbody = currentTetromino.GetComponent<Rigidbody2D>();
        tetrominoRigidbody.sleepMode = RigidbodySleepMode2D.StartAwake;
        tetrominoRigidbody.velocity = new Vector2(0f, 1f); // Dostosuj wartoœci wektora prêdkoœci do swoich potrzeb
    }


    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            spawnTimer = spawnInterval;
            SpawnTetromino(null);
        }
        CheckSleepState();
        CheckSleepStateUnderScreen();

    }

    private void CheckSleepState()
    {
        GameObject[] tetrominos = GameObject.FindGameObjectsWithTag("Tetromino");

        foreach (GameObject tetromino in tetrominos)
        {
            Rigidbody2D tetrominoRigidbody = tetromino.GetComponent<Rigidbody2D>();

            if (tetrominoRigidbody != null && tetrominoRigidbody.IsSleeping())
            {
                tetromino.tag = "StoppedTetromino";
                tetrominoRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                tetrominoRigidbody.sleepMode = RigidbodySleepMode2D.StartAsleep;
                tetrominoRigidbody.bodyType = RigidbodyType2D.Static;
                //soundEffect.Play();
            }
        }
    }

    private void CheckSleepStateUnderScreen()
    {
        GameObject[] tetrominos = GameObject.FindGameObjectsWithTag("Tetromino");

        Camera mainCamera = Camera.main;
        float screenBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).y - 2.4f;

        foreach (GameObject tetromino in tetrominos)
        {
            if (tetromino.transform.position.y < screenBottom)
            {
                Rigidbody2D tetrominoRigidbody = tetromino.GetComponent<Rigidbody2D>();

                if (tetrominoRigidbody != null && !tetrominoRigidbody.IsSleeping())
                {
                    //soundEffect.Play();
                    tetromino.tag = "StoppedTetromino";
                    tetrominoRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    tetrominoRigidbody.sleepMode = RigidbodySleepMode2D.StartAsleep;
                    tetrominoRigidbody.bodyType = RigidbodyType2D.Static;
                }
            }
        }
    }



    private void SpawnTetromino(float? x)
    {
        if (x == null)
        {
            int index = UnityEngine.Random.Range(0, tetrominoes.Length);
            int randomX = UnityEngine.Random.Range(minX, maxX);
            var xSpawnPoint = randomX * (float)0.8;

            Vector3 spawnPosition = new Vector3(xSpawnPoint, spawnPoint.position.y, spawnPoint.position.z);
            currentTetromino = Instantiate(tetrominoes[index], spawnPosition, Quaternion.identity);

            currentTetromino.tag = "Tetromino";

            // Losowy wybór rotacji
            var rotation = UnityEngine.Random.Range(0, 4) * 90;
            int rotationIndex = UnityEngine.Random.Range(0, rotation) * 90;
            currentTetromino.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationIndex));

            DoubleBlockTrigger(xSpawnPoint);

        }
        else if (x != null)
        {
            int index = UnityEngine.Random.Range(0, tetrominoes.Length);
            int randomX = UnityEngine.Random.Range(minX, maxX);
            var xSpawnPoint = randomX * (float)0.8;

            if (xSpawnPoint - 1.6f < x && x < xSpawnPoint + 1.6f)
            {
                float randomNumber = UnityEngine.Random.Range(4f, 8f);
                if (x - 1.6f < xSpawnPoint)
                {
                    xSpawnPoint = 1.6f * randomNumber;
                }
                else if (xSpawnPoint <  + 1.6f)
                {
                    xSpawnPoint = 1.6f * randomNumber;
                }
            }

            Vector3 spawnPosition = new Vector3(xSpawnPoint, spawnPoint.position.y, spawnPoint.position.z);
            currentTetromino = Instantiate(tetrominoes[index], spawnPosition, Quaternion.identity);

            // Losowy wybór rotacji
            var rotation = UnityEngine.Random.Range(0, 4) * 90;
            int rotationIndex = UnityEngine.Random.Range(0, rotation) * 90;
            currentTetromino.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationIndex));

            currentTetromino.tag = "Tetromino";
        }
    }



    private void RotateTetromino()
    {
        currentTetromino.transform.Rotate(Vector3.forward * 90f);
        currentTetromino.transform.Rotate(Vector3.back * 90f);
    }

    private void DoubleBlockTrigger(float x)
    {
        int randomNumber = UnityEngine.Random.Range(0, 100);
        if (randomNumber > 90)
        {
            SpawnTetromino(x);
        }
    }
}
