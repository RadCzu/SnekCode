
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public static class MultiplayerData
{
  public static int mapWidth;
  public static int mapHeight;
  public static int plusSize;
  public static string mapType;
  public static List<Vector2> positions = new();
  public static bool anyHumans = true;

  public static void SetHeight(int height) {
      mapHeight = height;
  }

  public static void SetWidth(int width) {
      mapWidth = width;
  }

  public static void SetPlusSize(int size) {
      plusSize = size;
  }

  public static void SetMapType(string type) {
      mapType = type;
  }
  
  public static void SetHuman(bool value) {
      anyHumans = value;
  }

  public static void AddPlayer() {
      ScriptableObjectBuilder<SOPlayerPlacement> builder = new("Objects/PlayerPlacements");
      Vector2 pos = builder.GetObject($"P{positions.Count + 1}")?.position 
                    ?? new Vector2(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));
      positions.Add(pos);
  }

  public static void RemovePlayer() {
      if (positions.Count > 0) {
          positions.RemoveAt(positions.Count - 1);
      }
  }
}

