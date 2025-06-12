using System;
using alpoLib.UI.Scene;
using alpoLib.Util;
using UnityEngine;

namespace MergeBoard.Scenes
{
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
            _ = ProcessLoadingAsync(machine);
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
            await alpoLib.Data.Module.LoadTableAsync();
            await Awaitable.WaitForSecondsAsync(0.5f);
        }
    }
    
    public class LoadingTaskLoadUserData : LoadingTaskBase
    {
        public override int Progress => 40;
        public override string ProgressMessage => "Load User Data...";
        
        protected override async Awaitable OnLoadingAsync(LoadingTaskMachine machine)
        {
            await Awaitable.WaitForSecondsAsync(0.5f);
        }
    }
    
    public class LoadingTaskShowMenu : LoadingTaskBase
    {
        public override int Progress => 100;
        public override string ProgressMessage => "Let's Go!";
        
        private readonly Action _switchToMenuAction;
        
        public LoadingTaskShowMenu(Action switchToMenuAction)
        {
            _switchToMenuAction = switchToMenuAction;
        }
        
        protected override async Awaitable OnLoadingAsync(LoadingTaskMachine machine)
        {
            await Awaitable.WaitForSecondsAsync(0.5f);
            _switchToMenuAction?.Invoke();
        }
    }
}