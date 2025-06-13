using alpoLib.Data;
using alpoLib.UI.Hud;
using UnityEngine;

namespace MergeBoard.UI.Hud
{
    public abstract class HudItemBaseWithData : HudItemBase
    {
        protected class Data : DataManagerHolder
        {
            public static T GetTableLoader<T>() where T : ITableDataMapperBase
            {
                return TableDataManager.GetLoader<T>();
            }
            
            public static T GetUserDataLoader<T>() where T : IUserDataMapperBase
            {
                return UserDataManager.GetLoader<T>();
            }
        }
    }
}