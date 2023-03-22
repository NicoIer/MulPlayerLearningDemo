using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace JobDemo
{
    public class CubeController : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab;

        public int cubeCount = 1000;

        public bool usingJob = false;

        [SerializeField] List<Transform> cubeTransforms = new List<Transform>();

        //这是一个用于显示JobSystem的Demo
        private void Start()
        {
            //按照正方形,以(0,0,0)为中心 , 生成cubeCount个 _cubePrefab 
            int a = (int)math.sqrt(cubeCount);

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < a; j++)
                {
                    var obj = Instantiate(cubePrefab, new Vector3(i, 0, j), quaternion.identity);
                    cubeTransforms.Add(obj.transform);
                }
            }
        }

        private void Update()
        {
            if (usingJob)
            {
                var transformAccessArray = new TransformAccessArray(cubeTransforms.ToArray());
                var moveJob = new CubeMoveJob
                {
                    time = Time.time
                };
                var jobHandle = moveJob.Schedule(transformAccessArray);
                jobHandle.Complete();
                transformAccessArray.Dispose();
            }
            else
            {
                //按照正弦 曲线移动 所有cube
                for (int i = 0; i < cubeTransforms.Count; i++)
                {
                    Vector3 pos = cubeTransforms[i].position;
                    pos.y = math.sin(Time.time + pos.x + pos.z);
                    cubeTransforms[i].position = pos;
                    //做一些cpu耗时
                    for (int j = 0; j < 10000; j++)
                    {
                        math.sqrt(j);
                    }
                }
            }
        }
    }
}