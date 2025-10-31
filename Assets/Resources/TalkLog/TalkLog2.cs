using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class TalkLog2 : ScriptableObject
{
	public List<TalkLogSO> Story; // Replace 'EntityType' to an actual type that is serializable.
}
