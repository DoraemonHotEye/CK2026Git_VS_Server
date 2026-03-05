using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitySupportLibrary.Debug;
using UnitySupportLibrary.Game.Objects;

namespace UnitySupportLibrary.Game.Managers
{
    public class GameManager
    {
        private static GameManager Inst = null;
        public static GameManager Instance => Inst ?? (Inst = new GameManager());
        public GameManager()
        {

        }

        private List<GameObject> objectlist = new List<GameObject>();

        public List<GameObject> GetObjectList { get { return objectlist; } }

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
                UnitysSupportsDebug.PrintWriteLine("GameManager->Init Update Looping...");
                while (true)
                {
                    Tick();
                }
            });

            tickThread.IsBackground = true; // 프로그램 종료 시 자동 종료
            tickThread.Start();
        }

        public GameObject AddObject<T>() where T : GameObject, new()
        {
            _rwlock.EnterWriteLock();
            try
            {
                GameObject obj = new T();
                obj.NativeOnEnable();

                objectlist.Add(obj);

                UnitysSupportsDebug.PrintWriteLine($"{typeof(T).Name} Added Object");

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

        public bool RemoveObject(GameObject obj)
        {
            _rwlock.EnterWriteLock();
            try
            {
                UnitysSupportsDebug.PrintWriteLine($"{obj.GetType().Name} Removed Object");
                obj.NativeOnDestroy();
                return objectlist.Remove(obj);
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

            if(DeltaTime != 0.0000000f)
                LastDeltaTime = DeltaTime;

            _rwlock.EnterReadLock();
            try
            {
                if (objectlist.Count > 0)
                {
                    // 한 번에 하나의 오브젝트만 업데이트
                    objectlist[_updateIndex].NativeUpdate(DeltaTime);

                    // 다음 인덱스로 이동 (순환)
                    _updateIndex = (_updateIndex + 1) % objectlist.Count;
                }
            }
            catch (Exception ex)
            {
                UnitysSupportsDebug.PrintWriteLine($"{ex.Message}");
            }
            finally
            {
                _rwlock.ExitReadLock();
            }
        }

    }
}
