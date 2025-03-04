using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class DinoGameWindow : EditorWindow
{
    [Header("Dino Settings")]
    private float dinoYPos = 0f;
    private float dinoVelocity = 0f;
    private bool isJumping = false;
    private readonly float gravity = -9f;
    private readonly float jumpForce = 30f;
    private readonly float groundY = 0f;

    [Header("Jump Settings")]
    private bool jumpHeld = false;
    private float jumpHoldTime = 0f;
    private readonly float maxJumpHoldTime = 3f;
    private readonly float maxJumpHeight = 500f;
    private readonly float jumpBoost = 15f;

    [Header("Timing Settings")]
    private int desiredFPS = 30;
    private double lastRepaintTime;
    private float deltaTimeMultiplier = 12f;

    [Header("Obstacle Settings")]
    public Vector2 flyingObstacleHeightRange = new Vector2(100f, 200f);
    public Vector2 obstacleSpawnIntervalRange = new Vector2(10f, 25f);

    [Header("Seed Settings")]
    public bool useFixedSeed = false;
    public int seedInput = 12345;
    private int currentSeed = 0;

    [Header("Game Start")]
    private bool gameStarted = false;

    private class Obstacle
    {
        public Vector2 position;
        public float size;
        public bool isFlying;
    }

    private List<Obstacle> obstacles = new List<Obstacle>();
    private float obstacleSpawnTimer = 0f;
    private float obstacleSpawnInterval = 15f;
    private float obstacleSpeed = 10f;

    [Header("Score Settings")]
    private float score = 0f;
    private bool gameOver = false;
    private int highscore = 0;
    private const string highscoreKey = "DinoGame_Highscore";

    [MenuItem("Window/Dino Game")]
    public static void ShowWindow() => GetWindow<DinoGameWindow>("Dino Game");

    private void OnEnable()
    {
        highscore = EditorPrefs.GetInt(highscoreKey, 0);
        EditorApplication.update += UpdateGame;
        ResetGame();
    }

    private void OnDisable() => EditorApplication.update -= UpdateGame;

    private void ResetGame()
    {
        dinoYPos = 0f;
        dinoVelocity = 0f;
        isJumping = false;
        jumpHeld = false;
        jumpHoldTime = 0f;
        score = 0f;
        obstacles.Clear();
        obstacleSpawnTimer = 0f;
        gameOver = false;
        InitializeRandomSeed();
        lastRepaintTime = EditorApplication.timeSinceStartup;
    }

    private void InitializeRandomSeed()
    {
        currentSeed = useFixedSeed ? seedInput : System.DateTime.Now.Millisecond;
        Random.InitState(currentSeed);
    }

    private void UpdateGame()
    {
        if (!gameStarted)
        {
         return;
        }
        double currentTime = EditorApplication.timeSinceStartup;
        if (currentTime - lastRepaintTime >= 1.0 / desiredFPS)
        {
            float dt = (1.0f / desiredFPS) * deltaTimeMultiplier;
            if (!gameOver)
            {
                UpdateDino(dt);
                UpdateObstacles(dt);
                UpdateScore(dt);
            }
            lastRepaintTime = currentTime;
            Repaint();
            deltaTimeMultiplier += 0.01f;
        }
    }

    private void UpdateDino(float dt)
    {
        if (isJumping)
        {
            if (jumpHeld && jumpHoldTime < maxJumpHoldTime && dinoYPos < maxJumpHeight)
            {
                dinoVelocity += jumpBoost * dt;
                jumpHoldTime += dt;
            }
            dinoVelocity += gravity * dt;
            dinoYPos += dinoVelocity * dt;
            if (dinoYPos <= groundY)
            {
                dinoYPos = groundY;
                isJumping = false;
                dinoVelocity = 0f;
                jumpHoldTime = 0f;
            }
            if (dinoYPos >= maxJumpHeight)
            {
                dinoYPos = maxJumpHeight;
                jumpHeld = false;
            }
        }
    }

    private void UpdateObstacles(float dt)
    {
        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            obstacles[i].position.x -= obstacleSpeed * dt;
            if (obstacles[i].position.x + obstacles[i].size < 0)
                obstacles.RemoveAt(i);
        }
        obstacleSpawnTimer += dt;
        if (obstacleSpawnTimer >= obstacleSpawnInterval)
        {
            SpawnObstacle();
            obstacleSpawnTimer = 0f;
            obstacleSpawnInterval = Random.Range(obstacleSpawnIntervalRange.x, obstacleSpawnIntervalRange.y);
            obstacleSpawnInterval = RoundTo(obstacleSpawnInterval, 4f);
        }
    }

    private void UpdateScore(float dt) => score += dt * deltaTimeMultiplier;

    private void SpawnObstacle()
    {
        Obstacle obs = new Obstacle();
        obs.size = 40f;
        obs.isFlying = Random.value > 0.5f;
        float groundLineY = position.height - 50;
        if (obs.isFlying)
        {
            float flyingHeight = Random.Range(flyingObstacleHeightRange.x, flyingObstacleHeightRange.y);
            flyingHeight = RoundTo(flyingHeight, 25f);
            obs.position = new Vector2(position.width, groundLineY - flyingHeight);
        }
        else
        {
            obs.position = new Vector2(position.width, groundLineY - obs.size);
        }
        obstacles.Add(obs);
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        useFixedSeed = EditorGUILayout.Toggle("Use Fixed Seed", useFixedSeed);
        seedInput = EditorGUILayout.IntField("Seed Input", seedInput);
        EditorGUILayout.LabelField("Current Seed", currentSeed.ToString());
        GUILayout.EndHorizontal();
        if (!gameStarted)
        {
            GUIStyle startStyle = new GUIStyle(EditorStyles.boldLabel);
            startStyle.fontSize = 32;
            startStyle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(0, (position.height - 40) / 2, position.width, 40), "Press Space to Start", startStyle);
            float groundLineY = position.height - 50;
            float dinoSize = 50;
            float dinoX = 50;
            float dinoY = groundLineY - dinoSize - dinoYPos;
            GUI.Box(new Rect(dinoX, dinoY, dinoSize, dinoSize), "Dino");
            Event startEvent = Event.current;
            if (startEvent.type == EventType.KeyDown && startEvent.keyCode == KeyCode.Space)
            {
                gameStarted = true;
                ResetGame();
            }
            return;
        }
        Event evt = Event.current;
        if (evt.type == EventType.KeyDown)
        {
            if (evt.keyCode == KeyCode.Space && !isJumping && !gameOver)
            {
                isJumping = true;
                jumpHeld = true;
                jumpHoldTime = 0f;
                dinoVelocity = jumpForce;
            }
            else if (evt.keyCode == KeyCode.R && gameOver)
            {
                ResetGame();
            }
        }
        else if (evt.type == EventType.KeyUp)
        {
            if (evt.keyCode == KeyCode.Space)
                jumpHeld = false;
        }
        float groundYPos = position.height - 50;
        GUI.Box(new Rect(0, groundYPos, position.width, 5), GUIContent.none);
        float dinoSize2 = 50;
        float dinoX2 = 50;
        float dinoY2 = groundYPos - dinoSize2 - dinoYPos;
        GUI.Box(new Rect(dinoX2, dinoY2, dinoSize2, dinoSize2), "Dino");
        foreach (var obs in obstacles)
            GUI.Box(new Rect(obs.position.x, obs.position.y, obs.size, obs.size), obs.isFlying ? "Fly" : "Obs");
        GUI.Label(new Rect(10, 10, 200, 30), "Score: " + Mathf.FloorToInt(score));
        GUI.Label(new Rect(10, 40, 200, 30), "Highscore: " + highscore);
        Rect dinoRect = new Rect(dinoX2, dinoY2, dinoSize2, dinoSize2);
        foreach (var obs in obstacles)
        {
            Rect obsRect = new Rect(obs.position.x, obs.position.y, obs.size, obs.size);
            if (dinoRect.Overlaps(obsRect))
            {
                gameOver = true;
            }
        }
        if (gameOver)
        {
            deltaTimeMultiplier = 12f;
            int currentScore = Mathf.FloorToInt(score);
            if (currentScore > highscore)
            {
                highscore = currentScore;
                EditorPrefs.SetInt(highscoreKey, highscore);
            }
            GUI.Label(new Rect(position.width / 2 - 70, position.height / 2 - 25, 140, 50), "Game Over! Press R to restart");
        }
    }

    private float RoundTo(float value, float to = 1) => Mathf.Round(value / to) * to;
}
