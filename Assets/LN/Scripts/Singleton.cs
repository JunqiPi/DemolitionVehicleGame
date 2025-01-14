using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LN
{
    public class Singleton<T> where T : new()
    {
        static T instance;
        public static T Instance
        {
            get
            {
                return GetInstance();
            }
        }
        private static T GetInstance()
        {
            if (instance == null)
            {
                instance = new T();

            }
            return instance;
        }
       
    }

}
