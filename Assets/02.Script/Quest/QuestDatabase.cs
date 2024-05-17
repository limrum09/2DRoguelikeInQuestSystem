using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName =("Quest/QuestDatabase"))]
public class QuestDatabase : ScriptableObject
{
    [SerializeField]
    private List<Quest> quests;

    public IReadOnlyList<Quest> Quests => quests;

    public Quest FindQuestBy(string questCodeName) => quests.FirstOrDefault(x => x.CodeName == questCodeName);

    [ContextMenu("FindQuest")]
    private void FindQuest()
    {
        FindQuestBy<Quest>();
    }

    [ContextMenu("FindAchievement")]
    private void FindAchievement()
    {
        FindQuestBy<Achievement>();
    }

    private void FindQuestBy<T>() where T : Quest
    {
        quests = new List<Quest>();

        // ã�ƺ� ��
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

        foreach(var guid in guids)
        {
            // ã�ƺ� ��
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var quest = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if(quest.GetType() == typeof(T))
            {
                quests.Add(quest);
            }

            // ã�ƺ� ��
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
