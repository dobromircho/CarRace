using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider slider;
    public Image fuel;
    public Text notEnough;
    public Text[] checkPointsText;
    public GameObject[] checkPoints;
    public GameObject[] zombies;
    public GameObject gameOver;
    float seconds = 60;
    float secs;
    int z;
    bool isTrigger;
    int currCHP;
    string tags;
    Animator anim;
    public  AudioClip[] checkPointSounds;
    public Text score;
    float currScore;
    float rewardScore;
    public Animator scoreAnim;
    float finishScore = 49999;
    Transform car;
    float timer;
    float resTime = 2;

    void Start()
    {
        for (int i = 0; i < checkPointsText.Length; i++)
        {
            checkPointsText[i].color = new Color(0, 0, 0, 0);
        }
        fuel.color = Color.red;
        slider.value = seconds;
        tags = (z + 1).ToString();
        anim = GameObject.FindGameObjectWithTag("Anim").GetComponent<Animator>();
        notEnough.color = new Color(1, 0, 0, 0);
    }

   
    void Update()
    {
        timer += Time.deltaTime;
        score.text = string.Format("Score : {0:00000}/{1}", currScore,finishScore);
        if (z == currCHP && z < checkPoints.Length)
        {
            isTrigger = GameObject.FindGameObjectWithTag((currCHP + 1).ToString()).GetComponent<CheckPointManager>().triggerIsEnter;
        }

        if (GameObject.FindGameObjectWithTag("Car"))
        {
            car = GameObject.FindGameObjectWithTag("Car").GetComponent<Transform>();
            seconds -= Time.deltaTime;
            slider.value = seconds;
            if (isTrigger)
            {
                if (currCHP == 6 && currScore < finishScore)
                {
                    notEnough.color = new Color(1, 0, 0, 1);

                }
                else if (currCHP == 6 && currScore > finishScore)
                {
                    gameOver.GetComponent<Image>().color = new Color(1, 1, 1,1);
                    gameOver.audio.Play();
                    StartCoroutine(RestartGame());
                }
                else
                {
                    checkPointsText[z].color = Color.green;
                    seconds = 60;
                    z++;
                    currCHP++;
                    anim.SetTrigger("Checked");
                    isTrigger = false;
                    PlaySounds();
                    audio.Play();
                }
            }
            if (seconds < 0 )
            {
                seconds = 0;
            }
            if (MoveZombies.isHit)
            {
                GivePoints();
               
                
            }
            if (timer >= resTime)
            {
                MakeZombies();
                timer = 0;
            }
          
        }
       
    }

    void PlaySounds()
    {
        audio.clip = checkPointSounds[Random.Range(0, 3)];
    }

    void GivePoints()
    {
        float rewardPoints = Random.Range(800, 1200);
        currScore = Mathf.Lerp(currScore, currScore + rewardPoints, 1f * Time.deltaTime);
        scoreAnim.SetTrigger("Scored");
        StartCoroutine(OneSecond());
    }

    IEnumerator OneSecond()
    {
        yield return new WaitForSeconds(1f);
        MoveZombies.isHit = false;
    }

    void MakeZombies()
    {
        Vector3 carPos = new Vector3(car.transform.position.x, 0f, car.transform.position.z);
        Instantiate(zombies[Random.Range(0,zombies.Length)], carPos + (car.transform.forward * 50), Quaternion.identity);
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5);
        Application.LoadLevel(0);
    }
}
