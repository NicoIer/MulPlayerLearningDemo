using System;
using System.Collections.Generic;

namespace Nico.MVC
{
    public interface IModel
    {
    }

    public abstract class AbstractModel : IModel
    {
        internal AbstractModel()
        {
        }
    }

    public static class ModelManager
    {
        internal static readonly Dictionary<Type, IModel> ModelDic = new Dictionary<Type, IModel>();

        public static T Get<T>() where T : IModel, new()
        {
            if (!ModelDic.ContainsKey(typeof(T)))
            {
                ModelDic.Add(typeof(T), new T());
            }

            return (T)ModelDic[typeof(T)];
        }
    }
}