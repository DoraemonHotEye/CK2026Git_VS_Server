using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitySupportLibrary.Debug;
using UnitySupportLibrary.Game.Components.NativeComponent.Ai;
using UnitySupportLibrary.Game.Objects;

namespace UnitySupportLibrary.Game.Managers
{
    public class AiManager
    {
        private static AiManager Inst;
        public static AiManager Instance => Inst ?? (Inst = new AiManager());

        public List<AiNavParent> aiNavAgents = new List<AiNavParent>();

        public ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        DateTime _lastTickTime;
        public float LastDeltaTime = 0.0f;
        ReaderWriterLockSlim _rwlock = new ReaderWriterLockSlim();

        //Thread Tasks
        Thread tickThread = null;
        Thread aiThread = null;

        public void StartThread() //Warning 멀티스레드
        {
            _lastTickTime = DateTime.Now;

            tickThread = new Thread(() =>
            {
                UnitysSupportsDebug.PrintWriteLine("AiManager->Init Update Looping...");
                while (true)
                {
                    Tick();
                }
            });

            tickThread.IsBackground = true; // 프로그램 종료 시 자동 종료
            tickThread.Start();
        }

        public AiNavParent AddObject<T>() where T : AiNavParent, new()
        {
            _rwlock.EnterWriteLock();
            try
            {
                AiNavParent obj = new T();
                obj.OnEnable();

                aiNavAgents.Add(obj);

                UnitysSupportsDebug.PrintWriteLine($"{typeof(T).Name} Added Object - AIManager");

                return obj;
            }
            catch (Exception ex)
            {
                UnitysSupportsDebug.PrintWriteLine($"{ex.Message}");
                return null;
            }
            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        public bool AddObject(AiNavParent navObj)
        {
            _rwlock.EnterWriteLock();
            try
            {
                navObj.OnEnable();

                aiNavAgents.Add(navObj);

                UnitysSupportsDebug.PrintWriteLine($"{navObj.GetType().Name} Added Object - AIManager");

                return true;
            }
            catch (Exception ex)
            {
                UnitysSupportsDebug.PrintWriteLine($"{ex.Message}");
                return false;
            }
            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        public bool RemoveObject(AiNavParent obj)
        {
            _rwlock.EnterWriteLock();
            try
            {
                UnitysSupportsDebug.PrintWriteLine($"{obj.GetType().Name} Removed Object - AIManager");
                return aiNavAgents.Remove(obj);
            }
            catch (Exception ex)
            {
                UnitysSupportsDebug.PrintWriteLine($"{ex.Message}");
                return false;
            }
            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        private int _updateIndex = 0;

        private void Tick()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan delta = currentTime - _lastTickTime;
            float DeltaTime = (float)delta.TotalMilliseconds / 1000f;
            _lastTickTime = currentTime;

            if (DeltaTime != 0.0000000f)
                LastDeltaTime = DeltaTime;

            _rwlock.EnterReadLock();
            try
            {
                if (aiNavAgents.Count > 0)
                {
                    if (_updateIndex >= aiNavAgents.Count)
                    {
                        _updateIndex = 0;
                    }

                    // 한 번에 하나의 오브젝트만 업데이트
                    aiNavAgents[_updateIndex].OnUpdate(DeltaTime);

                    // 다음 인덱스로 이동 (순환)
                    _updateIndex = (_updateIndex + 1) % aiNavAgents.Count;
                }
            }
            catch (Exception ex)
            {
                UnitysSupportsDebug.PrintWriteLine($"{ex}");
            }
            finally
            {
                _rwlock.ExitReadLock();
            }
        }
    }
}
