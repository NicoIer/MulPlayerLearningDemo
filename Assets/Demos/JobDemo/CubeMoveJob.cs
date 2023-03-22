using System.ComponentModel;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace JobDemo
{
    [BurstCompile]
    public struct CubeMoveJob : IJobParallelForTransform
    {
        [Unity.Collections.ReadOnly] public float time;

        public void Execute(int index, TransformAccess transform)
        {
            var pos = transform.position;
            pos.y = math.sin(time + pos.x + pos.z);
            transform.position = pos;
            for (int j = 0; j < 10000; j++)
            {
                math.sqrt(j);
            }
        }
    }
}