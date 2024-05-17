using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    #region
    private const string kSaveRootPath = "questSystem";
    private const string kActiveQuestSavePath = "activeQuest";
    private const string kCompletedQuestSavePath = "completedQeust";
    private const string kActiveAchievementSavePath = "activeAchievement";
    private const string kCompletedAchievementSavePath = "completedAchievement";
    #endregion


    #region Events
    public delegate void QuestRegisteredHandler(Quest newQeust);
    public delegate void QuestCompletedHangler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);
    public delegate void QuestCompletedOutPut();
    #endregion

    private static QuestSystem instance;
    private static bool isApplicationQuitting;

    public static QuestSystem Instance
    {
        get
        {
            if(!isApplicationQuitting && instance == null)
            {
                instance = FindObjectOfType<QuestSystem>();
                if(instance == null)
                {
                    instance = new GameObject("QuestSystem").AddComponent<QuestSystem>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }

            return instance;
        }
    }

    [SerializeField]
    private List<Quest> activeQuests = new List<Quest>();
    [SerializeField]
    private List<Quest> completedQuests = new List<Quest>();
    [SerializeField]
    private List<Quest> activeAchievements = new List<Quest>();
    [SerializeField]
    private List<Quest> completedAchievements = new List<Quest>();

    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompletedQuest => completedQuests;
    public IReadOnlyList<Quest> ActiveAchievements => activeAchievements;
    public IReadOnlyList<Quest> CompletedAchievements => completedAchievements;

    public event QuestRegisteredHandler onQuestRegistered;
    public event QuestCompletedHangler onQuestCompleted;
    public event QuestCanceledHandler onQuestCanceled;
    public event QuestCompletedOutPut onQuestSystemComplete;

    public event QuestRegisteredHandler onAchievementRegistered;
    public event QuestCompletedHangler onAchievementsCompleted;

    private QuestDatabase questDatabase;
    private QuestDatabase achievementDatabase;

    private void Awake()
    {
        questDatabase = Resources.Load<QuestDatabase>("Quest");
        achievementDatabase = Resources.Load<QuestDatabase>("Achievement");

        if (!Load())
        {
            Debug.Log("!Load()");
            foreach(var achievement in achievementDatabase.Quests)
            {
                QuestSystemRegister(achievement);
            }
        }
    }

    private void OnApplicationQuit()
    {
        // ������ ���װ� ���� ���� ����
        isApplicationQuitting = false;
    }

    public Quest QuestSystemRegister(Quest quest)
    {
        var newQuest = quest.Clone();

        if(newQuest is Achievement)
        {
            // ������ �޼��� ��츦 ����, �̺�Ʈ�� �Լ� �߰�
            newQuest.onQuestCompleted += OnAchievementComplet;

            // ���� ���� ���� ����Ʈ�� ���� �߰�
            activeAchievements.Add(newQuest);

            // ���� ����
            newQuest.QuestOnRegister();
            onAchievementRegistered?.Invoke(newQuest);
        }
        else
        {
            // ����Ʈ �޼� �Ǵ� ��Ҹ� �� ��츦 ����, �̺�Ʈ�� �Լ� �߰�
            newQuest.onQuestCompleted += OnQuestCompleted;
            newQuest.onQuestCanceled += OnQuestCanceld;
             
            // ���� ���� ����Ʈ ����Ʈ�� ����Ʈ �߰�
            activeQuests.Add(newQuest);

            // ����Ʈ ����
            newQuest.QuestOnRegister();
            onQuestRegistered?.Invoke(newQuest);
        }

        return newQuest;
    }

    // �ܺο��� ȣ��
    public void QuestSystemRecievereport(Category category, TaskTarget target, int successCount)
        => QuestSystemRecieveReport(category.CodeName, target.Value, successCount);

    // ���Ǽ��� ���� �߰�
    public void QuestSystemRecieveReport(string category, object target, int successCount)
    {
        QuestSystemRecievereport(activeQuests, category, target, successCount);
        QuestSystemRecievereport(activeAchievements, category, target, successCount);
    }

    public void QuestSystemRecievereport(List<Quest> quests, string category, object target, int successCount)
    {
        foreach(var quest in quests.ToArray())
        {
            quest.QuestRecieveReport(category, target, successCount);
        }
    }

    public void CompleteWaittingQeust()
    {
        foreach(var quest in activeQuests.ToList())
        {
            if (quest.IsCompletable)
            {
                quest.QuestComplete();
            }
        }
    }

    // ����Ʈ�� ��Ͽ� �ִ��� Ȯ��
    public bool ContainsActiveQuest(Quest quest) => activeQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsCompletedQuest(Quest quest) => completedQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsActiveAchievement(Quest quest) => activeAchievements.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsCompeltedAchievement(Quest quest) => completedAchievements.Any(x => x.CodeName == quest.CodeName);

    private JArray CreateSaveDatas(IReadOnlyList<Quest> quests)
    {
        var saveDatas = new JArray();

        foreach(var quest in quests)
        {
            if (quest.IsSavable)
            {
                Debug.Log("Data Saving: " + quest);
                saveDatas.Add(JObject.FromObject(quest.ToSaveData())); 
            }
        }

        return saveDatas;
    }

    public void Save()
    {
        Debug.Log("Save");
        var root = new JObject();
        root.Add(kActiveQuestSavePath, CreateSaveDatas(activeQuests));
        root.Add(kCompletedQuestSavePath, CreateSaveDatas(completedQuests));
        root.Add(kActiveAchievementSavePath, CreateSaveDatas(ActiveAchievements));
        root.Add(kCompletedAchievementSavePath, CreateSaveDatas(completedAchievements));
        
        PlayerPrefs.SetString(kSaveRootPath, root.ToString());
        PlayerPrefs.Save();
    }

    private void LoadSaveDatas(JToken dataToken, QuestDatabase database, System.Action<QuestSaveData, Quest> onSuccess)
    {
        var datas = dataToken as JArray;

        foreach(var data in datas)
        {
            var saveData = data.ToObject<QuestSaveData>();
            var quest = database.FindQuestBy(saveData.codeName);
            onSuccess.Invoke(saveData, quest);
        }
    }

    private void LoadActiveQuest(QuestSaveData saveData, Quest quest)
    {
        var newQeust = QuestSystemRegister(quest);
        newQeust.LoadFrom(saveData);
    }

    private void LoadCompletedQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = quest.Clone();
        newQuest.LoadFrom(saveData);

        if (newQuest is Achievement)
            completedAchievements.Add(newQuest);
        else
            completedQuests.Add(newQuest);
    }

    public bool Load()
    {
        if (PlayerPrefs.HasKey(kSaveRootPath))
        {
            // ������ ���¸� JSON���·� ����
            var root = JObject.Parse(PlayerPrefs.GetString(kSaveRootPath));

            LoadSaveDatas(root[kActiveQuestSavePath], questDatabase, LoadActiveQuest);
            LoadSaveDatas(root[kCompletedQuestSavePath], questDatabase, LoadCompletedQuest);

            LoadSaveDatas(root[kActiveAchievementSavePath], questDatabase, LoadActiveQuest);
            LoadSaveDatas(root[kCompletedAchievementSavePath], questDatabase, LoadCompletedQuest);

            return true;
        }

        else
            return false;
    }


    #region
    private void OnQuestCompleted(Quest quest)
    {
        onQuestSystemComplete?.Invoke();
        onQuestCompleted?.Invoke(quest);
        completedQuests.Add(quest);
        activeQuests.Remove(quest);

        foreach(var completed in onQuestCompleted.GetInvocationList())
        {
            Debug.Log("Method : " + completed);
        }

        Save();
    }

    private void OnQuestCanceld(Quest quest)
    {
        activeQuests.Remove(quest);

        onQuestCanceled?.Invoke(quest);

        Destroy(quest, Time.deltaTime);
    }

    private void OnAchievementComplet(Quest achievement)
    {
        activeAchievements.Remove(achievement);
        completedAchievements.Add(achievement);

        onAchievementsCompleted?.Invoke(achievement);
    }
    #endregion
}
