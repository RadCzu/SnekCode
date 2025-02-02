using System;
using System.Diagnostics;
using System.Linq;

public class PickScaledRandom : IMovePickingStrategy
{
    public int PickMove(float[] qValues)
    {
        float minValue = qValues.Min();
        float[] tempQValues = (float[])qValues.Clone();

        // Adjust values based on the minimum value
        if (minValue > 0)
        {
            for (int i = 0; i < tempQValues.Length; i++)
            {
                tempQValues[i] -= Math.Abs(minValue);
            }
        }
        else
        {
            for (int i = 0; i < tempQValues.Length; i++)
            {
                tempQValues[i] += Math.Abs(minValue);
            }
        }

        float ansSum = tempQValues.Sum();

        // If the sum of adjusted Q-values is 0, return the index of the highest Q-value
        if (ansSum == 0)
        {
            return Array.IndexOf(tempQValues, tempQValues.Max());
        }

        // Compute chances based on normalized Q-values
        float[] chances = tempQValues.Select(x => x / ansSum).ToArray();
        float choice = (float)new Random().NextDouble();  // Random float between 0 and 1

        float cumulatedChance = 0.0f;
        int bestMoveIndex = 0;

        // Iterate through the chances array and select the best move based on randomness
        for (int i = 0; i < chances.Length; i++)
        {
            cumulatedChance += chances[i];
            if (choice < cumulatedChance)
            {
                bestMoveIndex = i;
                break;
            }
        }

        return bestMoveIndex;
    }
}