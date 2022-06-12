using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExampleDataItem
{
    public string str1;
    public float float1;
}

[Serializable]
public class BuildingBlocksGameDataList
{
    public List<ExampleDataItem> BuildingBlocksGameDatas;
}