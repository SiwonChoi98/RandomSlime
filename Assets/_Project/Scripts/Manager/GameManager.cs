using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    [Header("플레이어")]
    public Player player;
    public EquipWeapon EquipWeapon;
    public GameObject[] Characters;
    
    [Header("맵 관리")] 
    [SerializeField] private Sprite[] _mapSprite;
    [SerializeField] private SpriteRenderer[] _mapSpriteRenderer;
    [Header("플레이어 수치 관련")] 
    [SerializeField] private Image _playerHealthImage;
    
    [Header("몬스터 관리")] 
    public int EnemyActiveCount; //현재 활성화된 몬스터 수
    public int EnemyKillCount; //몬스터 처치 수

    [Header("쉴드 스킬 관리")] public bool isActiveShield = false;
    [SerializeField] private GameObject _skillSelectPanel; //스킬 선택 판넬
    public ParticleSystem SkillSelectPs;
    [SerializeField] private GameObject _pauseUIPanel; //일시정지 버튼 판넬
    [SerializeField] private GameObject _psuseUISoundUIOff; //사운드 off 
    [SerializeField] private GameObject _stageFailUIPanel; //스테이지 Fail 판넬
    [SerializeField] private GameObject _stageClearUIPanel; //스테이지 Clear 판넬
    [SerializeField] private GameObject _randomSlimeUIPanel; //랜덤 슬라임 판넬
    [Header("Rule")]
    public bool IsBoss = false;
    
    private const string _BEST_SCORE_KEY = "BEST_SCORE";
    public float PlayTime;
    public float BestTime;    
    
    [Header("UI")] 
    [SerializeField] private Text _enemyCountText; //몬스터 처치 카운트
    [SerializeField] private Image _playerExpImage; //플레이어 경험치 이미지
    [SerializeField] private Text _playerLevelText; //플레이어 레벨
    [SerializeField] private Text _SkillSelectPlayerLevelText; //스킬 선택 플레이어 레벨
    [SerializeField] private Text _playTimeText; //플레이 타임
    [SerializeField] private Text _bestTimeText; //최고 플레이 타임
    [SerializeField] private Image _playGaugeImage; //플레이 게이지
    [SerializeField] private Text _failPlayTimeText; //실패 텍스트 플레이 타임
    [SerializeField] private Text _clearPlayTimeText; //성공 텍스트 플레이 타임

    [SerializeField] private Image randomSlimeImage; //처음 랜덤 슬라임 이미지
    public List<InGameExp> InGameExps = new (); //인게임 Exp 데이터
    public List<Monster> EnemyDatas; //몬스터 데이터
    
    [Header("몬스터 스폰 관련")]
    [SerializeField] private float _intervalTime;
    [SerializeField] private float _initIntervalTime;
    [SerializeField] private int _monsterCount;
    [SerializeField] private float _coolTime;
    [Header("몬스터 스폰 위치")]
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private Transform _bossSpawnPoint;
    [Header("현재 스테이지")]
    public int StageIndex = 0; //스테이지 인덱스
    public int SpwanPosCount;
    //몬스터 스프라이트 이미지
    public Dictionary<int, Sprite> DicSprites = new();
    public List<Sprite> enemySprite;
    
    public void StopButton()
    {
        Time.timeScale = 0;
    }

    public void PlayButton()
    {
        Time.timeScale = 1;
    }

    public void ExitButton(GameObject ga)
    {
        SoundManager.Instance.SfxPlaySound("Button");
        Time.timeScale = 1;
        ga.SetActive(false);
    }

    public void PauseUIButton()
    {
        SoundManager.Instance.SfxPlaySound("Button");
        Time.timeScale = 0;
        _pauseUIPanel.SetActive(true);
    }
    public void ResumeUIButton()
    {
        SoundManager.Instance.SfxPlaySound("Button");
        Time.timeScale = 1;
        _pauseUIPanel.SetActive(false);
    }
    public void MainSceneButton()
    {
        SoundManager.Instance.SfxPlaySound("Button");
        SceneManager.LoadScene("Main");
    }
    public void IngameSceneButton()
    {
        SceneManager.LoadScene("InGame");
    }

    public void SoundVolumeButton()
    {
        SoundManager.Instance.SfxPlaySound("Button");
        if (SoundManager.Instance.bgmAudioSource.volume > 0)
        {
            SoundManager.Instance.bgmAudioSource.volume = 0;
            _psuseUISoundUIOff.SetActive(true);
        }
        else
        {
            SoundManager.Instance.bgmAudioSource.volume = 0.05f;
            _psuseUISoundUIOff.SetActive(false);
        }
        
        if (SoundManager.Instance.sfxAudioSource.volume > 0)
        {
            SoundManager.Instance.sfxAudioSource.volume = 0;
        }
        else
        {
            SoundManager.Instance.sfxAudioSource.volume = 1;
        }
    }
    
    private void Awake()
    {
        SpecDataManager.Instance.Load(SpecDataResourceLoader.LoadSpecData());
        Singleton();
        SetPlayer();
        InGameExps = SpecDataManager.Instance.InGameExp.All.ToList();
        EnemyDatas = SpecDataManager.Instance.Monster.All.ToList();
        SetEnemySprite();
        
        
        GetBestTime();
        //SpecDataManager.Instance.Attendance.Get() // id
        //SpecDataManager.Instance.Attendance.All.ToList().Find(t=>t.day == 7) //모든 리스트 
    }

    //최고 시간 불러오기
    private void GetBestTime()
    {
        BestTime = PlayerPrefs.GetFloat(_BEST_SCORE_KEY);
        int minute = (int)BestTime / 60;
        int second = (int)BestTime % 60;
        _bestTimeText.text = "High Score " + minute.ToString("00") + ":" + second.ToString("00");
    }
    private void SetEnemySprite()
    {
        string path = "Prefabs/Sprite/Monster/";
        //일반몬스터 이미지 저장
        Sprite[] normalMonsterSprite = Resources.LoadAll<Sprite>(path+"monster");
        for (int i = 0; i < normalMonsterSprite.Length; i++)
        {
            enemySprite.Add(normalMonsterSprite[i]);
        }
        //엘리트몬스터 이미지 저장
        Sprite[] middleMonsterSprite = Resources.LoadAll<Sprite>(path+"middleboss");
        for (int i = 0; i < middleMonsterSprite.Length; i++)
        {
            enemySprite.Add(middleMonsterSprite[i]);
        }
        //보스몬스터 이미지 저장
        Sprite[] bossMonsterSprite = Resources.LoadAll<Sprite>(path+"Boss");
        for (int i = 0; i < bossMonsterSprite.Length; i++)
        {
            enemySprite.Add(bossMonsterSprite[i]);
        }
        
        //해당 몬스터 id 와 맞는 이미지 저장
        for (int i = 0; i < EnemyDatas.Count; i++)
        {
            Sprite monsterSprite = enemySprite.Find(t => t.name == EnemyDatas[i].sprite_name);
            DicSprites.Add(EnemyDatas[i].id, monsterSprite);   
        }
    }
    public void SetSpawnData()
    {
        //몬스터 생성 주기
        _intervalTime = 0.1f;
        _initIntervalTime = EnemyDatas[StageIndex].frequency;
        //몬스터 소환 갯수
        _monsterCount = EnemyDatas[StageIndex].monster_count;
        //다음 인덱스로 넘어가는 쿨타임
        _coolTime = EnemyDatas[StageIndex].cool_time;
    }
    public int GetMaxExp()
    {
        return InGameExps[player.Level - 1].need;
    }
    private void Start()
    {
        //게임을 최대한 빠르게 실행하도록 합니다 
        Application.targetFrameRate = -1;
        //60프레임 고정
        Application.targetFrameRate = 60;
        
        PlayButton();
        SetPlay();
        SpwanPosCount = _spawnPoint.Length;
        SetUserData();
        SoundManager.Instance.BgmPlaySound("InGame", 0.05f);
        SKillSelect(SpecDataManager.Instance.SkillLevel.Get(1051)); //1041
        SetMap();
        StartCoroutine(SetRandomSlimePanel());
    }

    //플레이어 셋팅
    private void SetUserData()
    {
        player.UserSetting 
        (
            "player1",
            SpecDataManager.Instance.GameConfig.Get(3015).value,
            SpecDataManager.Instance.GameConfig.Get(3006).value,
            SpecDataManager.Instance.GameConfig.Get(3006).value,
            SpecDataManager.Instance.GameConfig.Get(3007).value,
            1,
            1,
            0,
            GameManager.Instance.InGameExps[0].need,
            SpecDataManager.Instance.GameConfig.Get(3017).value
        );

    }
    //랜덤 슬라임 판넬 연출
    private IEnumerator SetRandomSlimePanel()
    {
        _randomSlimeUIPanel.SetActive(true);
        randomSlimeImage.sprite = player.GetComponent<SpriteRenderer>().sprite;
        yield return new WaitForSeconds(4f);
        _randomSlimeUIPanel.SetActive(false);
        SetStartExp();
        SetSpawnData();
    }
    //맵 셋팅
    private void SetMap()
    {
        int ran = Random.Range(0, _mapSprite.Length);
        for (int i = 0; i < _mapSpriteRenderer.Length; i++)
        {
            _mapSpriteRenderer[i].sprite = _mapSprite[ran];
        }
    }
    //게임 시작 시 처음 뿌려주는 exp
    private void SetStartExp()
    {
        //나중에 데이터 시트로 뺀 숫자 받아야함
        for (int i = 0; i < SpecDataManager.Instance.GameConfig.Get(3018).value; i++)
        {
            int ran = Random.Range(Config.RANSKILL_POS * -1, Config.RANSKILL_POS);
            int ran2 = Random.Range(Config.RANSKILL_POS * -1, Config.RANSKILL_POS);
            Vector3 vec = new Vector3(ran, ran2, 0);
            
            GameObject exp = ExpPool.instance.Get(0);
            exp.transform.position = GameManager.Instance.player.transform.position + vec;
        }
    }
    
    //플레이 초기화
    private void SetPlay()
    {
        PlayTime = 0; 
    }
    //캐릭터 초기화
    private void SetPlayer()
    {
        //캐릭터 소환
        int ran = Random.Range(0, Characters.Length);
        Instantiate(Characters[ran]);
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        EquipWeapon = player.GetComponentInChildren<EquipWeapon>();
        _playerHealthImage = player.gameObject.GetComponentsInChildren<Image>()[1];

        _bossSpawnPoint = player.gameObject.GetComponentsInChildren<Transform>()[1];

        int spawnPointSize = player.gameObject.GetComponentsInChildren<Transform>()[2].childCount;
        for (int i = 0; i < spawnPointSize; i++)
        {
            _spawnPoint[i] = player.gameObject.GetComponentsInChildren<Transform>()[2].GetComponentsInChildren<Transform>()[i+1];
        }
    }
   
    private void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        NextIndex();
        EnemySpawn();
    }

    //일정 시간마다 다음 행으로 이동
    private void NextIndex()
    {
        if (_coolTime > 0)
        {
            _coolTime -= Time.deltaTime;
            if (_coolTime <= 0)
            {
                //다음 인덱스로 넘어가고 다시 셋팅
                StageIndex++;
                SetSpawnData();
            }
        }
    }
    //일정 시간 마다 몬스터 생성
    private void EnemySpawn()
    {
        if (0 < _intervalTime)
        {
            _intervalTime -= Time.deltaTime;
        }
        else
        {
            int spawnPosRan = Random.Range(0, SpwanPosCount);
            _intervalTime = _initIntervalTime;
            //시간 되었을 때 몬스터 갯수만큼 소환
            for (int i = 0; i < _monsterCount; i++)
            {
                GetEnemy(SpwanPosCount, spawnPosRan);
                SpwanPosCount--;
                //스폰지역이 0보다 작아졌을 때 다시 초기화
                if (SpwanPosCount <= 0)
                {
                    SpwanPosCount = _spawnPoint.Length;
                }
            }
        }
    }
    //일반 몬스터 소환
    private void GetEnemy(int enemyPos, int CrowdPos)
    {
        int ranPos1 = Random.Range(0, 5);
        int ranPos2 = Random.Range(0, 5);
        Vector3 vec = new Vector3(ranPos1, ranPos2, 0); //조금씩 위치를 다르게 생성
        GameObject Enemy;
        //일반 몬스터
        Enemy = EnemyPool.instance.Get(0, SetEnemyData());

        if (SetEnemyData().spawn_type == SpawnType.ROUND)
        {
            Enemy.transform.position = _spawnPoint[enemyPos - 1].transform.position + vec;    
        }
        else if (SetEnemyData().spawn_type == SpawnType.CROWD)
        {
            Enemy.transform.position = _spawnPoint[CrowdPos].transform.position + vec;   
        }
    }

    //몬스터 데이터 셋팅
    private Monster SetEnemyData()
    {
        int curWaveMonsterID = EnemyDatas[StageIndex].id;
        Monster enemyData = EnemyDatas.Find((t) =>
        {
            return t.id == (curWaveMonsterID);
        });
        return enemyData;
    }
    
    private void LateUpdate()
    {
        GUI();

        GetPlayTime();

        StageFail();
        StageClear();
    }
    
    //플레이 타임 계산
    private void GetPlayTime()
    {
        PlayTime += Time.deltaTime;
    }
    private void StageFail()
    {
        if (player.CurHealth <= 0)
        {
            BestTimeSave();
            _failPlayTimeText.text = _playTimeText.text;
            StopButton();
            _stageFailUIPanel.SetActive(true);
        }   
    }

    private void StageClear()
    {
        if (StageIndex >= EnemyDatas.Count)
        {
            BestTimeSave();
            _clearPlayTimeText.text = _playTimeText.text;
            StopButton();
            _stageClearUIPanel.SetActive(true);
        }
    }

    private void BestTimeSave()
    {
        if (PlayTime >= BestTime)
        {
            BestTime = PlayTime;
            PlayerPrefs.SetFloat(_BEST_SCORE_KEY, BestTime);
        }
    }
    
    //이부분 값 변화했을 때만 동작하게 수정해야함
    private void GUI()
    {
        _playerHealthImage.fillAmount = player.CurHealth / player.MaxHealth;
        _enemyCountText.text = EnemyKillCount.ToString();
        _playerExpImage.fillAmount = player.CurExp / player.MaxExp;
        
        _playerLevelText.text = player.Level.ToString();
        _SkillSelectPlayerLevelText.text= player.Level.ToString();
        
        //타이머
        int minute = (int)PlayTime / 60;
        int second = (int)PlayTime % 60;
        _playTimeText.text = $"{minute.ToString("00")}:{second.ToString("00")}";
    }


    
    //레벨업
    public void LevelUp()
    {
        if (player.CurExp >= player.MaxExp)
        {
            player.CurExp = 0;
            player.Level++;
            player.MaxExp = GetMaxExp();
            
            _skillSelectPanel.SetActive(true);
            
            SkillSelectPs.gameObject.SetActive(true);
            SkillSelectPs.Play();
            StopButton();
        }
    }


 
    //스킬 선택 
    public void SKillSelect(SkillLevel holdSelectedSkill)
    {
        //해당 id가 존재했을 때는 삭제시켜주는게 맞고 해당 id가 없으면 삭제 로직은 없어야함
        
        //액티브 스킬
        if (holdSelectedSkill.prj_type != PrjType.NONE)
        {
            SkillPattern? skillId = EquipWeapon.ActiveAttacks.Find(x => holdSelectedSkill.skill_id == x.SkillId);
            //스킬이 존재 했을 경우
            if (skillId)
            {
                if (holdSelectedSkill.skill_id == skillId.SkillId)
                {
                    EquipWeapon.ActiveAttacks.Remove(skillId);
                    Destroy(skillId.gameObject);
                
                    //<쉴드전용> 아이디가 같을 때 쉴드 인지를 체크해서 쉴드면 false로 바꿔줘서 전 쉴드 오브젝트 삭제시킴
                
                    if (holdSelectedSkill.prj_type == PrjType.SHIELD)
                    {
                        isActiveShield = false;
                    }
                }    
            }
        }
        //패시브 스킬
        else
        {
            SkillPattern? skillId = EquipWeapon.PassiveAttacks.Find(x => holdSelectedSkill.skill_id == x.SkillId);
            
            if (skillId)
            {
                if (holdSelectedSkill.skill_id == skillId.SkillId)
                {
                    EquipWeapon.PassiveAttacks.Remove(skillId);
                    Destroy(skillId.gameObject);
                }    
            }
        }
       
        
        //현재 스플레드 시트 기반으로 정보들 생성하는 로직 -> 리소스에 있는 경로로 생성한다.
        GameObject Create = Resources.Load<GameObject>(holdSelectedSkill.folder_name);
        
        //스킬 생성
        GameObject skill = Instantiate(Create);
        skill.transform.parent = player.equipWeapon.transform;
        
        //해당 스킬 정보 초기화
        SkillPattern skillPattern = skill.GetComponent<SkillPattern>();
        skillPattern.SetSkillLevel(holdSelectedSkill);
        
        //엑티브 패시브 구분
        if (holdSelectedSkill.prj_type != PrjType.NONE)
        {
            EquipWeapon.ActiveAttacks.Add(skillPattern);
        }
        else
        {
            EquipWeapon.PassiveAttacks.Add(skillPattern);
        }
        
    }
}
