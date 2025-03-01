
using System.Collections.Generic;
using UnityEngine;

public static class MultiplayerData
{
  public static int mapWidth;
  public static int mapHeight;
  public static int plusSize;
  public static string mapType;
  public static List<Vector2> positions = new();
  public static List<Color> colors = new();
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
      var obj = builder.GetObject($"P{positions.Count + 1}");
      Vector2 pos = obj?.position 
                    ?? new Vector2(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));
      Color col = obj?.playerColor ?? Color.white;
      positions.Add(pos);
      colors.Add(col);
  }

  public static void RemovePlayer() {
      if (positions.Count > 0) {
          positions.RemoveAt(positions.Count - 1);
      }
  }

  public static void Reset()
  {
      positions = new();
      colors = new();
      mapWidth = 0;
      mapHeight = 0;
      plusSize = 0;
      anyHumans = true;
      mapType = "";
  }
}

