using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Category", fileName = "Category_")]
public class Category : ScriptableObject
{
    [SerializeField]
    private string codeName;

    [SerializeField]
    private string displayName;

    public string CodeName => codeName;
    public string DisplayName => displayName;


    public bool Equals(Category category)
    {
        if(category is null)
        {
            return false;
        }

        if(GetType() != category.GetType())
        {
            return false;
        }

        if(ReferenceEquals(category, this))
        {
            return true;
        }

        return category.codeName == codeName;
    }
}
