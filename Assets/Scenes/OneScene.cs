using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OneScene : MonoBehaviour
{
    public Text LevelDisplay;
    public Text LifeDisplay;

    public Text MaxHealthDisplay;
    public Text AttackDisplay;
    public Text DefenseDisplay;
    public Text RateOfAttackDisplay;
    public Text BulletSpeedDisplay;
    public Text JumpPowerDisplay;





    public int Level = 1;
    public GameObject Menu;
    public GameObject Player;
    public GameObject EnemyPreFab;

    public UnitBase PlayerStat;
    public UnitBase EnemiesStat;
    int _numberOfEnemies;
    bool _hasActiveEnemies = false;
    void Start()
    {
        PlayerStat = new UnitBase() { };
        EnemiesStat = new UnitBase() { };
        ResetGame();
        UpdateUpgradeUI();
        PlayerStat.Health = PlayerStat.MaxHealth;
        LifeDisplay.text = " Life: " + PlayerStat.Health + "/" + PlayerStat.MaxHealth;
        LevelDisplay.text = " Level: " + Level;
        var playerState = Player.GetComponent<PLayerControl>();
        playerState.State = PlayerStat;
        playerState.DamageCallback = PlayerDamageCallback;
    }

    void PlayerDamageCallback()
    {
        PlayerStat.Health -= EnemiesStat.Attack - PlayerStat.Defense > 0 ? EnemiesStat.Attack - PlayerStat.Defense : 0;
        LifeDisplay.text = " Life: " + PlayerStat.Health + "/" + PlayerStat.MaxHealth;
        if (PlayerStat.Health <= 0)
        {
            //game over
        }
    }
    void DamageCallback(int index)
    {
        var enemy = _enemies[index];
        var unit = enemy.GetComponent<EnemyControl>();
        unit.State.Health -= (PlayerStat.Attack);
        if (unit.State.Health <= 0)
        {
            enemy.SetActive(false);
            _hasActiveEnemies = _enemies.Where(x => x.activeSelf).Count() > 0;
        }
    }

    List<GameObject> _enemies = new List<GameObject>();
    void Update()
    {
        if (!_hasActiveEnemies || PlayerStat.Health <= 0)
        {
            Menu.SetActive(true);
            Time.timeScale = 0;
            if (PlayerStat.Health <= 0)
            {
                ResetGame();
            }
            else
            {

            }
        }
        else
        {
            Menu.SetActive(false);
            Time.timeScale = 1;
        }
        //fall and die
        if (Player.transform.position.y < -10)
        {
            _enemies.ForEach(x => x.SetActive(false));
            PlayerStat.Health = 0;
            _hasActiveEnemies = false;
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(PlayerStat.Health + " " + _hasActiveEnemies);
    }


    public void ResetGame()
    {
        Level = 0;



        PlayerStat.Health = 10;
        PlayerStat.MaxHealth = 10;
        PlayerStat.Attack = 2;
        PlayerStat.Defense = 0;
        PlayerStat.RateOfAttack = 2;
        PlayerStat.BulletSpeed = 4;
        PlayerStat.JumpPower = 1;

        Player.transform.position = Vector3.zero;

        EnemiesStat.Health = 5;
        EnemiesStat.Attack = 1;
        EnemiesStat.Defense = 0;
        EnemiesStat.RateOfAttack = 3;
        EnemiesStat.Range = 2;
        EnemiesStat.BulletSpeed = 1;
        _numberOfEnemies = 1;

        LifeDisplay.text = " Life: " + PlayerStat.Health + "/" + PlayerStat.MaxHealth;
        LevelDisplay.text = " Level: 1";
    }

    public void Upgrade(string key)
    {
        switch (key)
        {
            case "MaxHealth":
                if (PlayerStat.Health - (PlayerStat.MaxHealth / 2) > 0)
                {
                    PlayerStat.Health -= (PlayerStat.MaxHealth / 2);
                    PlayerStat.MaxHealth++;
                }
                break;
            case "Attack":
                if (PlayerStat.Health - (PlayerStat.Attack / 2) > 0)
                {
                    PlayerStat.Health -= (PlayerStat.Attack / 2);
                    PlayerStat.Attack++;
                }
                break;
            case "Defense":
                if (PlayerStat.Health - (PlayerStat.Defense / 2) > 0)
                {
                    PlayerStat.Health -= (PlayerStat.Defense / 2);
                    PlayerStat.Defense++;
                }
                break;
            case "RateOfAttack":
                if (PlayerStat.Health - (PlayerStat.RateOfAttack / 2) > 0)
                {
                    PlayerStat.Health -= (PlayerStat.RateOfAttack / 2);
                    PlayerStat.RateOfAttack++;
                }
                break;
            case "BulletSpeed":
                if (PlayerStat.Health - (PlayerStat.BulletSpeed / 2) > 0)
                {
                    PlayerStat.Health -= (PlayerStat.BulletSpeed / 2);
                    PlayerStat.BulletSpeed++;
                }
                break;
            case "JumpPower":
                if (PlayerStat.Health - (PlayerStat.JumpPower / 2) > 0)
                {
                    PlayerStat.Health -= (PlayerStat.JumpPower / 2);
                    PlayerStat.JumpPower++;
                }
                break;
            default:
                break;
        }



        LifeDisplay.text = " Life: " + PlayerStat.Health + "/" + PlayerStat.MaxHealth;
        UpdateUpgradeUI();
    }

    private void UpdateUpgradeUI()
    {
        //update UI
        MaxHealthDisplay.text = "Max life: " + PlayerStat.MaxHealth + " cost: " + PlayerStat.MaxHealth / 2 + " life";
        //AttackDisplay.text = "";
        AttackDisplay.text = "Attack: " + PlayerStat.Attack + " cost: " + PlayerStat.Attack / 2 + " life";
        //DefenseDisplay.text = "";
        DefenseDisplay.text = "Defense: " + PlayerStat.Defense + " cost: " + PlayerStat.Defense / 2 + " life";
        //RateOfAttackDisplay.text = "";
        RateOfAttackDisplay.text = "Rate of attack: " + PlayerStat.RateOfAttack + " cost: " + PlayerStat.RateOfAttack / 2 + " life";
        //BulletSpeedDisplay.text = "";
        BulletSpeedDisplay.text = "Bullet speed: " + PlayerStat.BulletSpeed + " cost: " + PlayerStat.BulletSpeed / 2 + " life";
        //JumpPowerDisplay.text = "";
        JumpPowerDisplay.text = "Jump power: " + PlayerStat.JumpPower + " cost: " + PlayerStat.JumpPower / 2 + " life";
    }

    public void NextLevel()
    {
        if (PlayerStat.Health <= 0)
        {
            ResetGame();
        }
        Level++;
        //heal player
        PlayerStat.Health = PlayerStat.MaxHealth;
        LifeDisplay.text = " Life: " + PlayerStat.Health + "/" + PlayerStat.MaxHealth;
        LevelDisplay.text = " Level: " + Level;
        //boost 2 enemy stats
        for (int i = 0; i < 2; i++)
        {
            var upgrade = Random.Range(0, 7);
            switch (upgrade)
            {
                case 0:
                    EnemiesStat.Health++;
                    break;
                case 1:
                    EnemiesStat.Attack++;
                    break;
                case 2:
                //EnemiesStat.Defense++;
                //break;
                case 3:
                    EnemiesStat.RateOfAttack++;
                    break;
                case 4:
                    EnemiesStat.Range++;
                    break;
                case 5:
                    EnemiesStat.BulletSpeed++;
                    break;
                case 6:
                    _numberOfEnemies++;
                    break;
            }
        }


        //spawn enemies
        while (_enemies.Count < _numberOfEnemies)
        {
            var enemy = Instantiate(EnemyPreFab, this.transform.position, Quaternion.identity);
            _enemies.Add(enemy);
            var enemyControl = enemy.GetComponent<EnemyControl>();
            enemyControl.Target = Player;
            enemyControl.Index = _enemies.Count - 1;
            enemyControl.State = new UnitBase() { };
            enemyControl.DamageCallback = DamageCallback;

        }
        for (int i = 0; i < _numberOfEnemies; i++)
        {
            _enemies[i].SetActive(true);
            var enemyControl = _enemies[i].GetComponent<EnemyControl>();
            enemyControl.State.Health = EnemiesStat.Health;
            enemyControl.State.Defense = EnemiesStat.Defense;
            enemyControl.State.Attack = EnemiesStat.Attack;
            enemyControl.State.RateOfAttack = EnemiesStat.RateOfAttack;
            enemyControl.State.Range = EnemiesStat.Range;
            enemyControl.State.BulletSpeed = EnemiesStat.BulletSpeed;
            do
            {
                _enemies[i].transform.position = (new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)));
            } while (Vector2.Distance(Player.transform.position, _enemies[i].transform.position) < 2f);
        }
        _hasActiveEnemies = true;
    }

}

public class UnitBase
{
    public int Health;
    public int Defense;
    public int Attack;
    public int RateOfAttack;
    public int BulletSpeed;
    //Robot
    public int Range;
    //Player
    public int MaxHealth;
    public int JumpPower;
}