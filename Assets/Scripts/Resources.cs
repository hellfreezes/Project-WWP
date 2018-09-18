using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResArgs : EventArgs
{
    public List<ResourceName> Names { get; set; }
}

public enum ResourceName
{
    WOOD,
    IRON,
    HUMAN
}

/// <summary>
/// Класс содержащий набор ресурсов
/// </summary>

public class Resources {
    private Dictionary<ResourceName, int> resource;

    public EventHandler<ResArgs> ResourcesChanged;

    public Resources()
    {
        resource = new Dictionary<ResourceName, int>();
    }

    public Resources(ResourceName name, int value)
    {
        resource = new Dictionary<ResourceName, int>();
        resource.Add(name, value);
    }

    public Resources(Dictionary<ResourceName, int> res)
    {
        resource = new Dictionary<ResourceName, int>(res);
    }

    public void SetResource(ResourceName name, int value)
    {
        if (!resource.ContainsKey(name))
            resource.Add(name, 0);

        resource[name] = value;
        List<ResourceName> names = new List<ResourceName>();
        names.Add(name);

        OnResourcesChanged(names);
    }

    public void Add(Resources res)
    {
        List<ResourceName> names = new List<ResourceName>();
        
        foreach (ResourceName name in res.Keys())
        {
            if (!resource.ContainsKey(name))
                resource.Add(name, 0);

            resource[name] += res.GetResourceValue(name);
            names.Add(name);
        }

        OnResourcesChanged(names);
    }

    public void Spend(Resources res)
    {
        if (IsEnough(res) == false)
        {
            Debug.LogError("Недостаточно ресурсов!");
            return;
        }
        Substract(res);
    }

    public void Substract(Resources res)
    {
        if (res == null)
            return; //FIXME: костыль!!!!

        List<ResourceName> names = new List<ResourceName>();

        foreach (ResourceName name in res.Keys())
        {
            if (!resource.ContainsKey(name))
            {
                Debug.LogError("Ресурса " + name.ToString() + "нет в списке.");
                return;
            }

            resource[name] -= res.GetResourceValue(name);
            names.Add(name);
        }

        OnResourcesChanged(names);
    }

    /// <summary>
    /// Каких ресурсов не хватает и сколько.
    /// TODO: придумать название получше
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    public Resources NotEnough(Resources res)
    {
        if (IsEnough(res))
            return null;

        Resources result = new Resources();
                
        foreach (ResourceName name in res.Keys())
        {
            if (!resource.ContainsKey(name)) {
                result.Add(new Resources(name, res.GetResourceValue(name)));
                continue;
            }
            if (resource[name] < res.GetResourceValue(name))
            {
                result.Add(new Resources(name, res.GetResourceValue(name) - resource[name]));
            }
        }

        return result;
    }


    public bool IsEnough(Resources res)
    {
        if (res == null)
            return true; //FIXME: костыль!!!!

        foreach (ResourceName name in res.Keys())
        {
            if (!resource.ContainsKey(name))
                return false;
            if (resource[name] < res.GetResourceValue(name))
                return false;
        }

        return true;
    }

    public ResourceName[] Keys()
    {
        return resource.Keys.ToArray();
    }

    public int GetResourceValue(ResourceName name)
    {
        if (!resource.ContainsKey(name))
            return 0;
        return resource[name];
    }

    /// <summary>
    /// Вызывает всех подписчиков если количество ресурсов изменилось
    /// </summary>
    /// <param name="names">Список тех ресурсов, которые изменились</param>
    protected virtual void OnResourcesChanged(List<ResourceName> names)
    {
        if (ResourcesChanged != null)
        {
            ResourcesChanged(this, new ResArgs() { Names = names });
        }
    }
}
