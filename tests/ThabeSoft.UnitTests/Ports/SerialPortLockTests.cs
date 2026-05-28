using System;
using System.Collections.Generic;
using System.Text;

namespace ThabeSoft.Ports;


[TestClass]
public class SerialPortLockTests
{
    public TestContext TestContext { get; set; }

    [DataRow(DuplexMode.HalfDuplex)]
    [DataRow(DuplexMode.FullDuplex)]
    [TestMethod]
    public async Task Debug_LockBehavior(DuplexMode mode)
    {
        var @lock = new SerialPortLock();

        // read
        var read_task = Task.Run(async () =>
        {
            Console.WriteLine("进入 >> 读");
            using var _ = await @lock.GetReadLockAsync(mode);

            Console.WriteLine("开始 >> 读");
            await Task.Delay(500);
            Console.WriteLine("离开 << 读");
        });

        // write
        var write_task = Task.Run(async () =>
        {
            Console.WriteLine("进入 >> 写");
            using var _ = await @lock.GetWriteLockAsync(mode);

            Console.WriteLine("开始 >> 写");
            await Task.Delay(500);
            Console.WriteLine("离开 << 写");
        });

        var configure_task = Task.Run(async () =>
        {
            Console.WriteLine("进入 >> 配置");
            using var _ = await @lock.GetConfigLockAsync();

            Console.WriteLine("开始 >> 配置");
            await Task.Delay(500);
            Console.WriteLine("离开 << 配置");
        });


        var read_task1 = Task.Run(async () =>
        {
            Console.WriteLine("进入 >> 读1");
            using var _ = await @lock.GetReadLockAsync(mode);

            Console.WriteLine("开始 >> 读1");
            await Task.Delay(500);
            Console.WriteLine("离开 << 读1");
        });


        var read_task2 = Task.Run(async () =>
        {
            Console.WriteLine("进入 >> 读2");
            using var _ = await @lock.GetReadLockAsync(mode);

            Console.WriteLine("开始 >> 读2");
            await Task.Delay(500);
            Console.WriteLine("离开 << 读2");
        });

        var write_task1 = Task.Run(async () =>
        {
            Console.WriteLine("进入 >> 写1");
            using var _ = await @lock.GetWriteLockAsync(mode);

            Console.WriteLine("开始 >> 写1");
            await Task.Delay(500);
            Console.WriteLine("离开 << 写1");
        });

        var write_task2 = Task.Run(async () =>
        {
            Console.WriteLine("进入 >> 写2");
            using var _ = await @lock.GetWriteLockAsync(mode);

            Console.WriteLine("开始 >> 写2");
            await Task.Delay(500);
            Console.WriteLine("离开 << 写2");
        });


        // 等待所有完成
        await Task.WhenAll(configure_task, read_task, read_task1, write_task);
    }


    [TestMethod]
    public async Task Test_Concurrent_NoDeadlock()
    {
        var duplex_mode = DuplexMode.FullDuplex;
        var @lock = new SerialPortLock();
        var completed = 0;
        var tasks = new List<Task>();

        // 启动多个并发任务
        for (int i = 0; i < 10; i++)
        {
            var id = i;
            tasks.Add(Task.Run(async () =>
            {
                for (int j = 0; j < 100; j++)
                {
                    if (id % 3 == 0)
                    {
                        using var _ = await @lock.GetReadLockAsync(duplex_mode);
                        await Task.Delay(1);
                    }
                    else if (id % 3 == 1)
                    {
                        using var _ = await @lock.GetWriteLockAsync(duplex_mode);
                        await Task.Delay(1);
                    }
                    else
                    {
                        using var _ = await @lock.GetConfigLockAsync();
                        await Task.Delay(1);
                    }
                }
                Interlocked.Increment(ref completed);
            }));
        }

        // 所有任务应该在合理时间内完成（不会死锁）
        var timeout = Task.Delay(30000);
        var completedTask = Task.WhenAll(tasks);

        var result = await Task.WhenAny(completedTask, timeout);
        Assert.AreSame(completedTask, result, "测试超时，可能发生死锁");

        Assert.AreEqual(10, completed);
    }

    [TestMethod]
    public async Task Test_HalfDuplex_ReadWrite_Mutex()
    {
        var duplex_mode = DuplexMode.HalfDuplex;
        var @lock = new SerialPortLock();

        var readHolding = new TaskCompletionSource<bool>();
        var writeStarted = false;

        // 读任务
        var read_task = Task.Run(async () =>
        {
            using var _ = await @lock.GetReadLockAsync(duplex_mode);
            Console.WriteLine($"{DateTime.Now:mm:ss.fff} Read started");
            readHolding.SetResult(true);
            await Task.Delay(500);
            Console.WriteLine($"{DateTime.Now:mm:ss.fff} Read ended");
        });

        await readHolding.Task;

        // 写任务（半双工下应该等待）
        var write_task = Task.Run(async () =>
        {
            Console.WriteLine($"{DateTime.Now:mm:ss.fff} Write waiting...");
            using var _ = await @lock.GetWriteLockAsync(duplex_mode);
            writeStarted = true;
            Console.WriteLine($"{DateTime.Now:mm:ss.fff} Write started");
        });

        await Task.Delay(100);

        // 验证写还没开始
        Assert.IsFalse(writeStarted);

        // 等待读结束
        await read_task;

        // 等待写开始
        await Task.Delay(100);
        Assert.IsTrue(writeStarted);
    }
}
