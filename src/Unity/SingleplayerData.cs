
using Unity.VisualScripting;

public static class SingleplayerData
{
    public static int mapWidth;
    public static int mapHeight;
    public static int plusSize;
    public static string mapType;

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
}
