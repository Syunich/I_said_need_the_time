using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    private PlayerInfo PlayerInfo;

    private static  GameManager Instance;
    private  float  deltatime;
    private   bool  SwichedTitle;
    private   bool  SwichedMain;
    private   bool  MonoUpdate;
    private Vector3 SpownPoint = Vector3.zero;

    public PlayerInfo playerinfo { get { return PlayerInfo; } }
    public GameObject PlayerObj { get; private set; }
    public bool IsLoaded { get; private set; }
    public static GameManager instance { get { return Instance; } }
    [SerializeField] ProgressIndicator progressindicator;
    [SerializeField] public bool IsTutorial;
    [SerializeField] Vector3 startpos;

    private void Awake()
    {
            MonoUpdate = false;
            Instance = this;
            IsLoaded = false;
            PlayerInfo                 = new PlayerInfo();
            PlayerInfo.Passedtime      = PlayerPrefs.GetFloat("passedtime",0);

        if (!IsTutorial)
        {
            PhotonNetwork.JoinOrCreateRoom("Main", new RoomOptions(), TypedLobby.Default);
        }
        else
        {
            StartUp();
            FeedManager.instance.Feedin(() => { });
            deltatime = 0;
        }
    }

    public override void OnJoinedRoom()
    {
        FeedManager.instance.Feedin(() => { });
        GamingUI.instance.Show_or_hideGameUI(true);
        if (PlayerPrefs.HasKey("playerposition"))
        { 
            PlayerObj = PhotonNetwork.Instantiate("Player", PlayerPrefsUtils.GetObject<Vector3>("playerposition"), Quaternion.identity);
            SpownPoint = PlayerObj.transform.position;
        }

        else
        {
            Debug.Log(startpos);
            PlayerObj = PhotonNetwork.Instantiate("Player", startpos, Quaternion.identity);
            SpownPoint = PlayerObj.transform.position;
        }

        StartUp();
        IsLoaded = true;
    }

    private void Update()
    {
        if (IsTutorial || !playerinfo.CanControll || !IsLoaded)
            return;

        if(!MonoUpdate)
        {
            MonoUpdate = true;
            PlayerObj.transform.position = SpownPoint;
        }

        deltatime  += Time.deltaTime;
        playerinfo.Passedtime  += Time.deltaTime;
        playerinfo.StartUpTime += Time.deltaTime;

        //進捗度合いと経過時間の処理
        if (deltatime > 1.0f)
        {
            deltatime = 0;
            PlayerInfo.Progress = progressindicator.GetProgress();
            PhotonNetwork.LocalPlayer.SetScore(PlayerInfo.Progress);
            GamingUI.instance.UpDateScoreBoard();
        }
        GamingUI.instance.ChangeTimeText(playerinfo.Passedtime);

        //音変更の処理
        if(AudioManager.BGMnumber != AudioManager.GetBGMNumberFromProgress(playerinfo.Progress))
        {
           AudioManager.BGMnumber = AudioManager.GetBGMNumberFromProgress(playerinfo.Progress);
           AudioManager.instance.PlayContinueBGM(AudioManager.BGMnumber);
        }
    }

    public void GameClear()
    {
        int milesec = (int)playerinfo.Passedtime * 1000;
        var timeScore = new System.TimeSpan(0, 0, 0, 0, milesec);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(timeScore , 1);
        Cursor.visible = true;
        playerinfo.CanControll = false;
        Resetinfo();
    }

    public void Resetinfo()
    {
        PlayerPrefs.SetFloat("passedtime", 0);
        PlayerPrefsUtils.SetObject("playerposition", startpos);
    }

    private void StartUp()
    {
        AudioManager.BGMnumber = 0;
        AudioManager.instance.PlayBGM(AudioManager.BGMnumber);
        deltatime = 0;
    }

    public void SetGotitle()
    {
        playerinfo.CanControll = false;
        SwichedTitle = true;
        FeedManager.instance.FeedOut(() =>
        {
            GamingUI.instance.Show_or_hideGameUI(false);
            PhotonNetwork.LeaveRoom();
        }
        );
    }

    public void SetRestart()
    {
        playerinfo.CanControll = false;
        SwichedMain = true;
        Resetinfo();
        FeedManager.instance.FeedOut(() =>
        {
            GamingUI.instance.Show_or_hideGameUI(false);
            PhotonNetwork.LeaveRoom();
        });
    }

    public override void OnConnectedToMaster()
    {
        if (SwichedMain)
        {
            SceneManager.LoadScene("MainScene");
        }

        else if (SwichedTitle)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
  
}
