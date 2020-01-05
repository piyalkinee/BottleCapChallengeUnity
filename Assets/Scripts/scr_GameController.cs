using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_GameController : MonoBehaviour
{
    #region Application
    string[] GoodWords = new string[5];
    string[] BadWords = new string[5];

    void CheckLanguage()
    {
        switch (Application.systemLanguage)
        {
            default:
                GoodWords[0] = "You're a Prince!";
                GoodWords[1] = "Magnificent!";
                GoodWords[2] = "Are you God?";
                GoodWords[3] = "Welcome to the Champions League!";
                GoodWords[4] = "Powerlifters!";
                BadWords[0] = "Manure";
                BadWords[1] = "Stink";
                BadWords[2] = "Loser";
                BadWords[3] = "Inept";
                BadWords[4] = "Even my grandmother is more stuffed";
                break;
            case SystemLanguage.Russian:
                GoodWords[0] = "Владим Владимиров вы как всегда лучший";
                GoodWords[1] = "У вас иностранное гражданство!";
                GoodWords[2] = "Вы бог?";
                GoodWords[3] = "Вы друг путина?!";
                GoodWords[4] = "Вы в танцах!";
                BadWords[0] = "О я вижу нового Панина";
                BadWords[1] = "Проснись, ты обосрался";
                BadWords[2] = "Опять облажался";
                BadWords[3] = "Че сплевываем?";
                BadWords[4] = "Моя бабушка сделает лучше";
                break;
            case SystemLanguage.Spanish:
                GoodWords[0] = "Eres un principe";
                GoodWords[1] = "¡Magnífica!";
                GoodWords[2] = "¿Eres Dios?";
                GoodWords[3] = "¡Bienvenido a la Liga de Campeones!";
                GoodWords[4] = "Super chico";
                BadWords[0] = "Estiércol";
                BadWords[1] = "Hedor";
                BadWords[2] = "Perdedora";
                BadWords[3] = "Inepta";
                BadWords[4] = "Incluso mi abuela está más llena";
                break;
            case SystemLanguage.Arabic:
                GoodWords[0] = "أنت أمير!";
                GoodWords[1] = "رائع!";
                GoodWords[2] = "أنت إله؟";
                GoodWords[3] = "مرحبا بكم في دوري الأبطال!";
                GoodWords[4] = "سوبر مان";
                BadWords[0] = "نتن";
                BadWords[1] = "خاسر";
                BadWords[2] = "أحمق";
                BadWords[3] = "حتى جدتي محشوة أكثر";
                BadWords[4] = "سماد";
                break;
            case SystemLanguage.Portuguese:
                GoodWords[0] = "Você é um príncipe!";
                GoodWords[1] = "Magnífica!";
                GoodWords[2] = "Você é Deus?";
                GoodWords[3] = "Bem-vindo à Liga dos Campeões!";
                GoodWords[4] = "Super homen";
                BadWords[0] = "Estrum";
                BadWords[1] = "Fedor";
                BadWords[2] = "Fracassada";
                BadWords[3] = "Inepta";
                BadWords[4] = "Até minha avó está mais recheada";
                break;
            case SystemLanguage.Indonesian:
                GoodWords[0] = "Lebih rapi anaconda berikutnya";
                GoodWords[1] = "Gunung berapi menjadi tenang";
                GoodWords[2] = "Selamat ayah";
                GoodWords[3] = "Aku menghargaimu!";
                GoodWords[4] = "Alam semesta akan memberi Anda keuntungan!";
                BadWords[0] = "Apakah itu yang kamu bisa?";
                BadWords[1] = "Ini yang tidak ayahmu harapkan";
                BadWords[2] = "Jangan bicara lagi?";
                BadWords[3] = "Jika Anda menghapus aplikasi itu akan lebih baik untuk semua orang!";
                BadWords[4] = "Pecundang";
                break;
            case SystemLanguage.French:
                GoodWords[0] = "Je te vois dans un nouveau costume de gucci";
                GoodWords[1] = "Prendre un croissant de l'étagère";
                GoodWords[2] = "Succès de l'aimant";
                GoodWords[3] = "Je veux des enfants de toi";
                GoodWords[4] = "Tu as enjambé la merde";
                BadWords[0] = "Vous avez un mauvais costume";
                BadWords[1] = "Il semblerait que la tour Eiffel se brise aujourd'hui";
                BadWords[2] = "Oh mon dieu comment désolé pour la france";
                BadWords[3] = "La situation politique se détériore";
                BadWords[4] = "Ne pas pleurer plus miroir";
                break;
            case SystemLanguage.German:
                GoodWords[0] = "Oooh !!! Bir !!";
                GoodWords[1] = "Du bist innerlich feurig !!!";
                GoodWords[2] = "Ich habe das noch nie gesehen!";
                GoodWords[3] = "A WELL KA REPEAT";
                GoodWords[4] = "SUPER MAN";
                BadWords[0] = "Stinkender Arsch";
                BadWords[1] = "Schwein stinkt besser";
                BadWords[2] = "So schlimm habe ich noch nicht gesehen";
                BadWords[3] = "Hike you foster";
                BadWords[4] = "Ich möchte nicht, dass du es spielst.";
                break;
        }
    }
    public void Save()
    {
        PlayerPrefs.SetInt("BGS", BestGameScore);
        PlayerPrefs.SetInt("NA", GetComponent<scr_ADSIAP>().NoADS);
    }
    void Load()
    {
        BestGameScore = PlayerPrefs.GetInt("BGS");
        GetComponent<scr_ADSIAP>().NoADS = PlayerPrefs.GetInt("NA");
        UpdateScore();
    }
    #endregion

    #region GamePlay
    public Transform[] BottelCreators = new Transform[4]; // 0 - 1 Left side, 2 - 3 Right side 

    public GameObject[] Bottels = new GameObject[3];

    public bool GameIsStarted = false;
    int GameScore = 0;
    int BestGameScore = 0;
    int SpawnRank = 0;

    void Start()
    {
        CheckLanguage();
        Load();
    }
    void StartGame()
    {
        GameIsStarted = true;

        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);

        StartCoroutine(SpawnBottles());
    }
    IEnumerator SpawnBottles()
    {
        SpawnRank++;

        float delay = Random.Range(0.8f, 1.5f);
        yield return new WaitForSeconds(delay);

        int spawnIndex = Random.Range(0, BottelCreators.Length);

        GameObject bottle = Instantiate(Bottels[Random.Range(0, 3)], BottelCreators[spawnIndex].position, BottelCreators[spawnIndex].rotation);
        bottle.GetComponent<scr_Bottle>().GameController = gameObject;

        if (GameIsStarted && SpawnRank < 20)
            StartCoroutine(SpawnBottles());
        else
            StartCoroutine(SuperSpawnBottles());
    }
    IEnumerator SuperSpawnBottles()
    {
        SpawnRank++;

        float delay = Random.Range(0.3f, 1);
        yield return new WaitForSeconds(delay);

        int spawnIndex = Random.Range(0, BottelCreators.Length);

        if (Random.Range(0, 10) == 0)
        {
            GameObject bottle1 = Instantiate(Bottels[Random.Range(0, 1)], BottelCreators[spawnIndex].position, BottelCreators[spawnIndex].rotation);
            bottle1.GetComponent<scr_Bottle>().GameController = gameObject;
            GameObject bottle2 = Instantiate(Bottels[Random.Range(2, 3)], BottelCreators[spawnIndex].position, BottelCreators[spawnIndex].rotation);
            bottle2.GetComponent<scr_Bottle>().GameController = gameObject;
        }
        else
        {
            GameObject bottle = Instantiate(Bottels[Random.Range(0, 3)], BottelCreators[spawnIndex].position, BottelCreators[spawnIndex].rotation);
            bottle.GetComponent<scr_Bottle>().GameController = gameObject;
        }

        if (GameIsStarted && SpawnRank > 20 && SpawnRank < 100)
            StartCoroutine(SuperSpawnBottles());
        else
            StartCoroutine(MegaSuperSpawnBottles());
    }
    IEnumerator MegaSuperSpawnBottles()
    {
        SpawnRank++;

        float delay = Random.Range(0.3f, 0.7f);
        yield return new WaitForSeconds(delay);

        int spawnIndex = Random.Range(0, BottelCreators.Length);

        if (Random.Range(0, 8) == 0)
        {
            GameObject bottle1 = Instantiate(Bottels[Random.Range(0, 1)], BottelCreators[spawnIndex].position, BottelCreators[spawnIndex].rotation);
            bottle1.GetComponent<scr_Bottle>().GameController = gameObject;
            GameObject bottle2 = Instantiate(Bottels[Random.Range(2, 3)], BottelCreators[spawnIndex].position, BottelCreators[spawnIndex].rotation);
            bottle2.GetComponent<scr_Bottle>().GameController = gameObject;
        }
        else
        {
            GameObject bottle = Instantiate(Bottels[Random.Range(0, 3)], BottelCreators[spawnIndex].position, BottelCreators[spawnIndex].rotation);
            bottle.GetComponent<scr_Bottle>().GameController = gameObject;
        }

        if (GameIsStarted)
            StartCoroutine(MegaSuperSpawnBottles());
    }
    public void AddScore(int count)
    {
        GameScore += count;
        UpdateScore();
    }
    public void Defeat()
    {
        GameIsStarted = false;

        if (GameScore > BestGameScore)
        {
            BestGameScore = GameScore;
            OpenWinScreen();
            Save();
        }
        else
        {
            OpenLoseScreen();
        }

        GameScore = 0;
        SpawnRank = 0;
        UpdateScore();

        Invoke("ShowADS", 5.1f);
        Invoke("StartGame", 5.5f);
    }
    #endregion

    #region UI
    public GameObject Background;
    public GameObject Button;
    public GameObject ImageText;
    public GameObject Score;
    public GameObject BestScore;
    public GameObject WinScreen;
    public Text WinText;
    public GameObject LoseScreen;
    public Text LoseText;
    public GameObject NoADS;
    public Sprite[] SBackgounds = new Sprite[2];

    public void ButtonStartGame()
    {
        Button.SetActive(false);
        ImageText.SetActive(false);
        Score.SetActive(true);
        BestScore.SetActive(true);
        NoADS.SetActive(true);

        UpdateBackground();
        StartGame();
    }
    public void ButtonNoADS()
    {
        GetComponent<scr_ADSIAP>().BuyNoADS();
    }
    public void UpdateBackground()
    {
        Background.GetComponent<Image>().sprite = SBackgounds[1];
    }
    public void UpdateScore()
    {
        Score.GetComponent<Text>().text = "" + GameScore;
        BestScore.GetComponent<Text>().text = "BEST SCORE : " + BestGameScore;
    }
    void OpenWinScreen()
    {
        WinScreen.SetActive(true);

        WinText.text = GoodWords[Random.Range(0, 5)];
    }
    void OpenLoseScreen()
    {
        LoseScreen.SetActive(true);

        LoseText.text = BadWords[Random.Range(0, 5)];
    }
    void ShowADS()
    {
        GetComponent<scr_ADSIAP>().ShowADS();
    }
    #endregion
}
