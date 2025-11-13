using UnityEngine;

namespace Common.Scripts
{
    public interface IPoolable
    {
        void Initialize(PoolObject pool, Vector3 position);

        void Reset();
    }
}