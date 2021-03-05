using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] CloneObjects, DroidObjects;
    public GameObject Droid, CloneAlly, GunShip;

    public int currentNumberofClones, currentNumberofDroids, waveNumber, cursorTextureNumber, delayEnemyWave;
    public bool startGameBool, spawnReady, nextWaveReady, gameOver, regenHealth, gameStarted; //starts the game when true

    [SerializeField]
    private Texture2D[] cursorAim;

    [SerializeField]
    private Animator textWaveAnimator, textGameOverAnimator, playButtonAnimator;

    [SerializeField]
    private Text textWave;

    [SerializeField]
    private int spawnTime, positionNumber, enemyCount, gunShipTransitionTime, addTroopWave;

    [SerializeField]
    private Transform[] spawnPosition;

    [SerializeField]
    private Transform GunShipTransform;

    [SerializeField]
    private int[] waitTime;

    [SerializeField]
    public int enemyKills;

    // Start is called before the first frame update
    void Start()
    {
        CursorSettings(); //function of cursorsetting
        textWaveAnimator.GetComponent<Animator>(); //get the animator of textwaveanimator
        textGameOverAnimator.GetComponent<Animator>(); //get the animator of textgameoveranimator
        playButtonAnimator.GetComponent<Animator>(); //get the animator of startButton
        addTroopWave = 3; //addtroopwave equal to 3 (this is for when the wave equals 3 or divisible by 3, troops come in
        cursorTextureNumber = 0; //reset cursor texturenumber
    }

    // Update is called once per frame
    void Update()
    {
        NextWave(); //function of nextfunction
        GameOver(); // function of gameover
    }

    public void StartGame()
    {
        waveNumber = 1; // wavenumber equals 1 (this number identifies what wave the player is on, is the start of the game
        enemyCount = 2; // enemycount starts at 2
        gameOver = false; //gameover equals false
        waitTime[0] = 2; //reset the waittime to 2
        currentNumberofDroids = 0; //reset the droid count
        StartCoroutine(InstantaitePlayer()); // start coroutine instantiateplayer to create the player
    }

    IEnumerator InstantaitePlayer()
    {
        Instantiate(GunShip, GunShipTransform.position, Quaternion.Euler(0, -90, 0)); //instance the gunship
        yield return new WaitForSeconds(gunShipTransitionTime); //wait for the guntransitionntime then start the first wave
        gameStarted = true;
        StartCoroutine(Wave()); //start the coroutine wave
    }

    IEnumerator InstantaiteTroops()
    {
        Instantiate(GunShip, GunShipTransform.position, Quaternion.Euler(0, -90, 0)); //instance the gunship
        yield return new WaitForSeconds(gunShipTransitionTime); //wait for the guntransitionntime then start the first wave
        StartCoroutine(Wave()); //start the coroutine wave
    }

    IEnumerator Wave()
    {
        if (!gameOver) //if the game is not over (uneccessay but to scaried to delete)
        {
            yield return new WaitForSeconds(waitTime[0]); //wait for specific amount of time

            textWave.text = "Wave " + waveNumber; //textwave text equals wave plus the wave number
            textWaveAnimator.SetTrigger("Transition"); //trigger the textwaveanimator transition

            yield return new WaitForSeconds(waitTime[1]); //wait for specific amount of time

            for (int i = 0; i < waveNumber * enemyCount; i++) //run this loop until i equalls wavenumber multipled by the enemycount
            {
                if (!gameOver) //if the game is not over
                {
                    if (delayEnemyWave == i) //if enemy delay equals to i (3) with wait five seconds to spawn the next droid, this makes it easier for the player and gives them a little break to regroup instead of being showered upon droid
                    {
                        yield return new WaitForSeconds(5); //wait five seconds
                        delayEnemyWave += 3; //add 3 to delayenemywave, so when another three droids (i) spawn, this property or if statment will run again and agian each time
                    }

                    SpawnDroid(); // call the spawn droid
                    yield return new WaitForSeconds(spawnTime); //wait for specific amount of time
                }
                //else
                //{
                //    i = waveNumber * enemyCount; //might be unneccessary but to scared to delete)
                //    StopCoroutine(Wave()); //stop the coroutine!!! (many issues with the droid current count getting messed up so hopeful this does it (why there might be neccessary lines of code)
                //    Debug.Log("CoroutineStopped");
                //}
            }

            if (!gameOver) // is game is not over next wave equals true, this helps the game know if the next wave should happen or not
            {
                nextWaveReady = true; //next weve equals true
            }

            delayEnemyWave = 3; // reset the delayenemywave back to 3
        }
    }

    void NextWave()
    {
        if(currentNumberofDroids == 0 && nextWaveReady) //if the currentnumberof droids equal 0 and nextwaveready equals true
        {
            nextWaveReady = false; // nextwaveready equals false, so waves dont start happening when there not supposed to
            waitTime[0] = 5; // waittime equals 5 because it was 2 but since were not instancing the player we can change it
            waveNumber++; //wave number increase by one
            regenHealth = true; //regen health equal true, every time a wave ends the player regenerates all there health
            if (waveNumber == addTroopWave && !gameOver) //if the wavenumber equals addtroopwave, addtroopwave is every 3 waves
            {
                addTroopWave += 3; // add three to the addtroopwave so this if statement will run again and again each 3 waves
                StartCoroutine(InstantaiteTroops()); //start the instancetroop coroutine
            }
            else //else if it's not
            {
                StartCoroutine(Wave()); //start the wave coroutine
            }
        }
    }

    void SpawnDroid()
    {
        positionNumber = Random.Range(0, 5); //get a random number 
        GameObject EnemyDroid; //instance a gameobject
        EnemyDroid = Instantiate(Droid, spawnPosition[positionNumber].position, Quaternion.identity); //place the gameobject/prefab in the scene at the spawnposition
    }

    public void CursorSettings()
    {
        Cursor.SetCursor(cursorAim[cursorTextureNumber], new Vector2(16, 16), CursorMode.Auto); // this is the cursor setting, the middle of the cursor where the player clicks
    }

    void GameOver()
    {
        if(gameStarted && currentNumberofClones == 0) //when the game has started and there are no more clones
        {
            gameStarted = false; //game started equal false
            gameOver = true; //game over equals true
            StartCoroutine(PromptGameOver()); //start the coroutine promptgameover
        }
    }

    IEnumerator PromptGameOver()
    {
        yield return new WaitForSeconds(0.5f); // wait 0.5f seconds
        textGameOverAnimator.SetTrigger("Transition"); //set the trigger of the textgameoveranimator transition
        StartCoroutine(PromptStartButton()); //start the coroutine prompt start button
    }

    IEnumerator PromptStartButton()
    {
        yield return new WaitForSeconds(1.5f); //wait 1.5 seconds
        playButtonAnimator.SetTrigger("Transition"); //set the trigger of the playbutton animator transition
    }
}
