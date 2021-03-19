using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public interface IConfigLoader
{
    object Load(string path);
}
public class ConfigMgr
{

    IConfigLoader m_assets = null;
    Dictionary<Type, object> cfgDic = null;
    public ConfigMgr(IConfigLoader loader, params Type[] preloads)
    {
        m_assets = loader;
        cfgDic = new Dictionary<Type, object>();
        for (int i = 0; i < preloads.Length; i++)
        {
            Type t = preloads[i];
            LoadCfg(t);
        }
    }

    public T Get<T>(bool reload = false)
    {
        return (T)Get(typeof(T), reload);
    }
    public object Get(Type t, bool reload = false)
    {
        if (!cfgDic.ContainsKey(t) || reload)
        {
            LoadCfg(t);
        }
        return cfgDic[t];
    }
    public void Remove<T>()
    {
        Remove(typeof(T));
    }
    public void Remove(Type t)
    {
        if (cfgDic.ContainsKey(t))
        {
            cfgDic.Remove(t);
        }
    }

    Dictionary<Type, string> pathdic = new Dictionary<Type, string>();
    public string GetPath(Type type)
    {
        if (pathdic.ContainsKey(type))
        {
            return pathdic[type];
        }
        CfgAttribute ca = type.GetCustomAttribute<CfgAttribute>(false);
        string path = "";
        if (ca != null)
        {
            path = ca.path;
            pathdic[type] = path;
        }
        else
        {
            Debug.LogError(type.Name + " no CfgAttribute");
        }
        return path;

    }
    public string GetPath<T>()
    {
        Type type = typeof(T);
        return GetPath(type);
    }
    public T LoadCfgNoCache<T>(string path)
    {
        T re = (T)LoadRes(path, typeof(T));
        return re;
    }
    void LoadCfg(Type t)
    {
        string path = GetPath(t);
        if (string.IsNullOrEmpty(path)) return;
        cfgDic[t] = LoadRes(path, t);
    }
    const string TYPE_XML = ".xml";
    const string TYPE_JSON = ".json";
    const string TYPE_UNITY = ".asset";
    object LoadRes(string path, Type t)
    {
        object re = null;
        if (path.EndsWith(TYPE_XML))
        {
            string content = m_assets.Load(path).ToString();
            using (StringReader sr = new StringReader(content))
            {
                XmlSerializer xmldes = new XmlSerializer(t);
                re = xmldes.Deserialize(sr);
            }
        }
        else if (path.EndsWith(TYPE_JSON))
        {
            string content = m_assets.Load(path).ToString();
            re = Newtonsoft.Json.JsonConvert.DeserializeObject(content, t);
        }
        else if (path.EndsWith(TYPE_UNITY))
        {
            object asset = m_assets.Load(path);
            re = UnityEngine.Object.Instantiate((UnityEngine.Object)asset);
        }
        else
        {
            re = Activator.CreateInstance(t);
        }

        MethodInfo mi = t.GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        if (mi != null)
        {
            mi.Invoke(re, null);
        }

        return re;
    }
}
[AttributeUsage(AttributeTargets.Class)]
public class CfgAttribute : Attribute
{
    readonly public string path;
    public CfgAttribute(string path)
    {
        this.path = path;
    }
}

