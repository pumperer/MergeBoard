using System;
using System.Collections.Generic;
using alpoLib.Data;
using alpoLib.UI.Scene;
using alpoLib.Util;
using MergeBoard.Data.Table;
using MergeBoard.VFX;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MergeBoard.Scenes
{
    public class DataLoadCompleteHolder : GameState
    {
    }
    
    public interface ILoadingProgressChangeListener
    {
        void OnLoadingProgressChanged(LoadingTaskBase task);
    }
    
    public class LoadingTaskMachine : SequentialStateMachineBase
    {
        public ILoadingProgressChangeListener Listener { get; }
        
        public LoadingTaskMachine(ILoadingProgressChangeListener listener)
        {
            Listener = listener;
        }
    }
    
    public abstract class LoadingTaskBase : SequentialStateBase<LoadingTaskMachine>
    {
        public abstract int Progress { get; }
        public abstract string ProgressMessage { get; }
        
        private bool _isCompleted;
        
        public override void OnEnter(LoadingTaskMachine machine)
        {
            machine.Listener.OnLoadingProgressChanged(this);
            AwaitableHelper.Run(() => ProcessLoadingAsync(machine));
        }

        private async Awaitable ProcessLoadingAsync(LoadingTaskMachine machine)
        {
            await OnLoadingAsync(machine);
            machine.DoNextState();
        }

        protected abstract Awaitable OnLoadingAsync(LoadingTaskMachine machine);
    }
    
    public class LoadingTaskHello : LoadingTaskBase
    {
        public override int Progress => 0;
        public override string ProgressMessage => "Hello!";
        
        protected override async Awaitable OnLoadingAsync(LoadingTaskMachine machine)
        {
            await Awaitable.WaitForSecondsAsync(1f);
        }
    }
    
    public class LoadingTaskLoadTableData : LoadingTaskBase
    {
        public override int Progress => 20;
        public override string ProgressMessage => "Load Table Data...";
        
        protected override async Awaitable OnLoadingAsync(LoadingTaskMachine machine)
        {
            await alpoLib.Data.Module.LoadTableDataAsync();
            await Awaitable.WaitForSecondsAsync(0.25f);
        }
    }
    
    public class LoadingTaskLoadUserData : LoadingTaskBase
    {
        public override int Progress => 40;
        public override string ProgressMessage => "Load User Data...";
        
        protected override async Awaitable OnLoadingAsync(LoadingTaskMachine machine)
        {
            await alpoLib.Data.Module.LoadUserDataAsync();
            await Awaitable.WaitForSecondsAsync(0.25f);
        }
    }
    
    public class LoadingTaskShowMenu : LoadingTaskBase
    {
        private class TemporaryDataHolder : DataManagerHolder
        {
            public List<BoardDefineBase> GetBoardDefines()
            {
                return TableDataManager.GetLoader<IBoardDefineTableMapper>().GetBoardDefineBaseList();
            }
        }
        private TemporaryDataHolder _temporaryDataHolder;
        
        public override int Progress => 100;
        public override string ProgressMessage => "Let's Go!";
        
        private readonly Action<List<BoardDefineBase>> _showMenuSeq;
        
        public LoadingTaskShowMenu(Action<List<BoardDefineBase>> showMenuSeq)
        {
            _temporaryDataHolder = new TemporaryDataHolder();
            _showMenuSeq = showMenuSeq;

            GameStateManager.Instance.GetState<DataLoadCompleteHolder>();
        }
        
        protected override async Awaitable OnLoadingAsync(LoadingTaskMachine machine)
        {
            await Addressables.InstantiateAsync("Addr/VFX/VfxResourceHolderOutGame.prefab").Wait();
            await VfxResourceHolder.Instance.LoadAsync();
            var list = _temporaryDataHolder.GetBoardDefines();
            _showMenuSeq?.Invoke(list);
        }
    }
}