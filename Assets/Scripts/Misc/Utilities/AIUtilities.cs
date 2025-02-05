﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Класс для ИИ
/// </summary>
public static class AIUtilities
{
    //Слои в именах
    public static string CharsLayerName = "CharLayer";
    public static string EnemyLayerName = "EnemyLayer";
    public static string GroundLayerName = "GroundLayer";
    public static string InteractLayerName = "InteractLayer";
    public static string MiniMapLayerName = "MiniMapLayer";
    //Слои в цифрах
    public static int CharsLayer = LayerMask.NameToLayer(CharsLayerName);
    public static int EnemyLayer = LayerMask.NameToLayer(EnemyLayerName);
    public static int GroundLayer = LayerMask.NameToLayer(GroundLayerName);
    public static int InteractLayer = LayerMask.NameToLayer(InteractLayerName);
    public static int MiniMapLayer = LayerMask.NameToLayer(MiniMapLayerName);

    //Теги
    public static string CharsTag = "Character";
    public static string EnemyTag = "Enemy";

    /// <summary>
    /// Поиск ближайшего игрового объекта по тегу
    /// </summary>
    /// <param _name="transform"></param>
    /// <param _name="tag">Тег искомого объекта</param>
    /// <returns>Ближайший игровой объект</returns>
    public static GameObject FindNearestEntity(Transform transform, string tag)
    {
        GameObject ClosestEntity = null;
        float DistanceToClosestEntity = Mathf.Infinity;

        GameObject[] entities = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject entity in entities)
        {
            float Distance = (entity.transform.position - transform.position).sqrMagnitude;

            if (DistanceToClosestEntity > Distance)
            {
                DistanceToClosestEntity = Distance;
                if (entity.active)
                {
                    ClosestEntity = entity;
                }
            }
        }

        Color lineColor = transform.gameObject.TryGetComponent(out CharacterScript chara) ? Color.green : Color.red;
        Debug.DrawLine(transform.position, ClosestEntity.transform.position, lineColor);

        return ClosestEntity;
    }

    //TODO: ЗАМЕНИТЬ
    public static ITarget GetTarget(Transform transform, EntityStats stats)
    {
        ITarget target = null;

        Collider[] targets = Physics.OverlapSphere(transform.position, 30, 1 << stats.EnemyLayer);

        Debug.Log(stats.EntityLayer);

        var list = new List<ITarget>();

        foreach (var item in targets)
        {
            MiscUtilities.GetInterfaces(out List<ITarget> l, item.gameObject);
            list.Add(l.FirstOrDefault());
        }

        if (list.Count > 0)
        {
            int maxP = list.Max(x => x.Priority);
            target = list.Where(x => x.Priority == maxP).FirstOrDefault();
        }

        return target;
    }

    /// <summary>
    /// Метод, возвращающий то, достиг ли агент пункта назначения
    /// </summary>
    /// <param name="agent">Агент NavMesh</param>
    /// <returns>Достиг ли агент пункта назначения</returns>
    public static bool IsAgentReachedDistanation(NavMeshAgent agent)
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Поиск заданного игрового объекта в радиусе
    /// </summary>
    /// <param name="gameObject">Объект, являющийся центром области поиска</param>
    /// <param name="certainGameObject">Искомый объект</param>
    /// <param name="radius">Радиус поиска</param>
    /// <returns>Находится ли объект в радиусе</returns>
    public static bool IsCertainEntityInRadius(GameObject gameObject, GameObject certainGameObject, float radius)
    {
        return (certainGameObject.transform.position - gameObject.transform.position).sqrMagnitude < 3 * radius;
    }
}