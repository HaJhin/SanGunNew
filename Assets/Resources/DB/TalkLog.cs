using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(ExcelName = "TalkLog")]
public class TalkLog : ScriptableObject
{
	public List<TalkLogSO> Story; // Replace 'EntityType' to an actual type that is serializable.
}
