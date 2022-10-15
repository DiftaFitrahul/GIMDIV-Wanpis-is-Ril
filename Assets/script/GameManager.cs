using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool startPlaying;

    public BeatScroller theBS;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int currentMultiplier;
    public int multiplierTracker = 0;
    public int[] multiplierThresholds;


    public Text scoreText;
    public Text multiText;

    public float totalNote;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public GameObject resultScreen;
    public Text percenHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;

    public static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        scoreText.text = "Score: 0";
        currentMultiplier = 1;

        totalNote = FindObjectsOfType<NoteObject>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;

                theMusic.Play();
            }
        } else {
            if(!theMusic.isPlaying && !resultScreen.activeInHierarchy){
                resultScreen.SetActive(true);

                normalsText.text = normalHits.ToString();
                goodsText.text = goodHits.ToString();
                perfectsText.text = perfectHits.ToString();
                missesText.text = missedHits.ToString();

                float totalHits = normalHits + goodHits + perfectHits;
                float percenHit = (totalHits / totalNote) * 100f;

                percenHitText.text = percenHit.ToString("F1") + "%";


                if(percenHit < 50){
                    rankText.text = "D";
                } else if (percenHit < 70){
                    rankText.text = "C";
                } else if (percenHit < 80){
                    rankText.text = "C+";
                } else if (percenHit < 90){
                    rankText.text = "B";
                } else if (percenHit < 95){
                    rankText.text = "A";
                } else if (percenHit <= 100){
                    rankText.text = "S";
                }
                
                Debug.Log(rankText.text);

                finalScoreText.text = currentScore.ToString();

            }
        }

    }

    public void NoteHit()
    {
        Debug.Log("Hit on Time");
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;
            if (multiplierThresholds[currentMultiplier - 1] == multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        multiText.text = "Multiplier: x" + currentMultiplier;

        //currentScore += scorePerNote * currentMultiplier;
        scoreText.text = "Score: " + currentScore;
    }

    public void NormalHit(){
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        normalHits++;
    }

    public void GoodHit(){
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit(){
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        perfectHits++;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
        currentMultiplier = 1;
        multiplierTracker = 0;
        missedHits++;

        multiText.text = "Multiplier: x" + currentMultiplier;
    }
}
