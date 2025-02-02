using System.Linq;

class PickBest : IMovePickingStrategy
{
  public int PickMove(float[] QValues)
  {
      return QValues.ToList().IndexOf(QValues.Max());
  }
}
