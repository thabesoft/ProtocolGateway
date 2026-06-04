namespace System.Threading;


public static class SemaphoreSlimExtensions
{
    extension(SemaphoreSlim semaphoreSlim)
    {
        public Disposer<SemaphoreSlim> Lock()
        {
            semaphoreSlim.Wait();
            return Disposer.Create(semaphoreSlim, x => x.Release());
        }

        public async ValueTask<Disposer<SemaphoreSlim>> LockAsync()
        {
            await semaphoreSlim.WaitAsync();
            return Disposer.Create(semaphoreSlim, x => x.Release());
        }

        public async ValueTask<Disposer<SemaphoreSlim>> LockAsync(CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);
            return Disposer.Create(semaphoreSlim, x => x.Release());
        }
    }
}
