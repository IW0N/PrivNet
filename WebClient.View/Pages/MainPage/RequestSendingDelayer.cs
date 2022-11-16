namespace WebClient.View.Pages.MainPage
{
    internal class ActionDelayer
    {
        readonly int delayMilliseconds;
        readonly Action delayedAction;
        CancellationTokenSource cancelSource;
        Thread procThread;//processing thread
        bool firstLaunch=true;
        bool actionComplited = true;
        public ActionDelayer(int delayMilliseconds,Action delayedAction)
        {
            this.delayMilliseconds =delayMilliseconds;
            this.delayedAction =delayedAction;
            cancelSource = new CancellationTokenSource();
        }
        public void Reboot() 
        {
            if(!firstLaunch&&!actionComplited)
                cancelSource.Cancel();
            Invoke();
            firstLaunch = false;
        }
        public async Task Invoke()
        {
            try
            {
                actionComplited = false;
                await Task.Delay(delayMilliseconds,cancelSource.Token);
                if(!actionComplited)
                    delayedAction();
            }
            catch(Exception e)
            {
                cancelSource.Dispose();
                cancelSource = new CancellationTokenSource();
            }
            actionComplited = true;
        }
    }
}
