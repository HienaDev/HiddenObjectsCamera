using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

    public enum Difficulty
    {
        VeryEasy, // No animation
        Easy, // Animation Duration = 0.1, Initial Delay = 0
        Normal, // Animation Duration = 2, Initial Delay = 1
        Hard // Animation Duration = 10, Initial Delay = 2
    }

    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private Color veryEasyColor;
    [SerializeField] private string veryEasyName;
    [SerializeField] private Color easyColor;
    [SerializeField] private string easyName;
    [SerializeField] private Color normalColor;
    [SerializeField] private string normalName;
    [SerializeField] private Color hardColor;
    [SerializeField] private string hardName;

    [HideInInspector] public bool isAnimated;
    [HideInInspector] public float animationDuration;
    [HideInInspector] public float initalDelay;
    [SerializeField] private GameObject[] particles;
    [HideInInspector] public GameObject currParticle;

    private Difficulty difficulty;
    private int difficultyIndex = 2;

    [HideInInspector] public static DifficultyManager Instance;

    [HideInInspector] public bool GameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        CheckDifficulty();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        GameStarted = true;
    }

    public void NextDifficulty()
    {
        difficultyIndex++;

        if (difficultyIndex > 3)
        {
            difficultyIndex = 0;
        }

        CheckDifficulty();
    }


    private void CheckDifficulty()
    {
        switch (difficultyIndex)
        {
            case 0: // Very Easy
                difficultyText.text = veryEasyName;
                difficultyText.color = veryEasyColor;
                difficulty = Difficulty.VeryEasy;
                isAnimated = false;
                animationDuration = 0.1f;
                initalDelay = 0f;
                
                currParticle = particles[0];
                break;
            case 1: // Easy
                difficultyText.text = easyName;
                difficultyText.color = easyColor;
                difficulty = Difficulty.Easy;
                isAnimated = true;
                animationDuration = 0.1f;
                initalDelay = 0f;
                
                currParticle = particles[1];
                break;
            case 2: // Normal
                difficultyText.text = normalName;
                difficultyText.color = normalColor;
                difficulty = Difficulty.Normal;
                isAnimated = true;
                animationDuration = 2f;
                initalDelay = 1f;

                currParticle = particles[2];
                break;
            case 3: // Hard
                difficultyText.text = hardName;
                difficultyText.color = hardColor;
                difficulty = Difficulty.Hard;
                isAnimated = true;
                animationDuration = 5f;
                initalDelay = 1f;

                currParticle = particles[3];
                break;
            default: // Default to Normal
                difficultyText.text = normalName;
                difficultyText.color = normalColor;
                difficulty = Difficulty.Normal;
                isAnimated = true;
                animationDuration = 2f;
                initalDelay = 1f;

                currParticle = particles[2];
                break;
        }
    }


}
