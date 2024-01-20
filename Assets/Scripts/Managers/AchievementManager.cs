using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Steamworks;

public enum Achievement
{
    Aloha,
    BallsToTheWall,
    SafeToEat,
    VivaVidsel,
    Ballpark
}

public enum Stat
{
    Bounces
}

public class AchievementManager : MonoBehaviour
{
    //protected Callback<UserStatsReceived_t> m_UserStatsReceived;

    private bool hasReceivedUserStats = false;
    private List<string> achievementQueue = new List<string>();

    private Dictionary<Stat, int> statsQueue = new Dictionary<Stat, int>();

    private const int BALLS_TO_THE_WALL_THRESHOLD = 83; // Balls to the Wall was released in '83.
    // Should probably be higher.

    private void OnEnable()
    {

        //if (!SteamManager.Initialized) return;

        //m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatReceived);

        //SteamUserStats.RequestCurrentStats();
        OnUserStatReceived();

    }

    private void OnUserStatReceived() // Should contain UserStatsReceived_t pCallback
    {

        //if (pCallback.m_eResult != EResult.k_EResultOK) return;

        hasReceivedUserStats = true;

        ClearAchievementQueue();
        ClearStatQueue();

        /*

        Use this code to delete all your achievements

        SteamUserStats.ClearAchievement("ACH_COMEBACK");
        SteamUserStats.ClearAchievement("ACH_FIRST_WIN");
        SteamUserStats.ClearAchievement("ACH_SCORE_FIVE");
        SteamUserStats.ClearAchievement("ACH_NEAR_MISS");
        SteamUserStats.ClearAchievement("ACH_PLAY_MULTIPLAYER");
        SteamUserStats.ClearAchievement("ACH_THREE_CONSECUTIVE");
        SteamUserStats.ClearAchievement("ACH_SELF_GOAL");
        SteamUserStats.StoreStats();

        */

    }

    private void GiveAchievement(string achievement)
    {

        Debug.Log(achievement);

        if (hasReceivedUserStats)
        {

            GameManager.instance.audioManager.PlayVoice("Score_-Noel_-Deep2");

            //SteamUserStats.SetAchievement(achievement);

            //SteamUserStats.StoreStats();

        }
        else
        {

            if (!achievementQueue.Contains(achievement))
                achievementQueue.Add(achievement);

        }

    }

    public void GiveAchievement(Achievement achievement)
    {

        switch (achievement)
        {

            case Achievement.Aloha:
                GiveAchievement("ACH_ALOHA");
                break;
            case Achievement.BallsToTheWall:
                GiveAchievement("ACH_BALLTTWALL");
                break;
            case Achievement.SafeToEat:
                GiveAchievement("ACH_SAFETOEAT");
                break;
            case Achievement.VivaVidsel:
                GiveAchievement("ACH_VIVAVIDSEL");
                break;
            case Achievement.Ballpark:
                GiveAchievement("ACH_BALLPARK");
                break;
            default:
                break;

        }

    }

    public void AddStat(Stat stat, int value)
    {

        string statReference = "Noel";

        switch (stat)
        {
            case Stat.Bounces:
                statReference = "stat_bounces";
                break;
            default:
                break;
        }

        if (statReference.Equals("Noel")) return;

        if (hasReceivedUserStats)
        {

            int currentValue = 0;
            /*
            SteamUserStats.GetStat(statReference, out currentValue);
            SteamUserStats.SetStat(statReference, currentValue + value);
            SteamUserStats.StoreStats();
            */

            switch (stat)
            {
                case Stat.Bounces:
                    if (currentValue + value >= BALLS_TO_THE_WALL_THRESHOLD)
                    {
                        GiveAchievement(Achievement.BallsToTheWall);
                    }
                    break;
                default:
                    break;
            }

        }
        else
        {

            if (statsQueue.ContainsKey(stat))
            {
                statsQueue[stat] += value;
                // Placeholder code to test w/o Steamworks
                switch (stat)
                {
                    case Stat.Bounces:
                        if (statsQueue[stat] >= BALLS_TO_THE_WALL_THRESHOLD)
                        {
                            GiveAchievement(Achievement.BallsToTheWall);
                        }
                        break;
                    default:
                        break;
                }
            }

        }

    }

    public int GetStat(Stat stat)
    {

        string statReference = "Noel";

        switch (stat)
        {
            case Stat.Bounces:
                statReference = "stat_bounces";
                break;
            default:
                break;
        }

        if (statReference.Equals("Noel")) return -1;

        if (hasReceivedUserStats)
        {

            int currentValue = 0;
            //SteamUserStats.GetStat(statReference, out currentValue);

            return currentValue;

        }
        else
        {

            return -1;

        }

    }

    private void ClearAchievementQueue()
    {

        foreach (string s in achievementQueue)
        {
            GiveAchievement(s);
        }

        achievementQueue.Clear();

    }

    private void ClearStatQueue()
    {

        if (statsQueue.ContainsKey(Stat.Bounces)) AddStat(Stat.Bounces, statsQueue[Stat.Bounces]);

        statsQueue.Clear();

    }

}
