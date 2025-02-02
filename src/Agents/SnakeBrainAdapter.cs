using UnityEngine;
using Unity.Barracuda;
using System.Collections.Generic;

public class SnakeBrainAdapter
{
    private NNModel modelAsset;
    private IWorker worker;

    public SnakeBrainAdapter(string modelPath)
    {
        modelAsset = Resources.Load<NNModel>(modelPath);
        if (modelAsset == null)
        {
            Debug.LogError($"Failed to load model from path: {modelPath}");
            return;
        }
        worker = ModelLoader.Load(modelAsset).CreateWorker(WorkerFactory.Device.GPU);
    }

public float Run(
    Tensor mapSummary, 
    Tensor localMap,   
    Tensor distanceFromWalls, 
    Tensor foodInfo, 
    Tensor moveHistory, 
    Tensor moveDirection 
)
{
    if (worker == null)
    {
        worker = ModelLoader.Load(modelAsset).CreateWorker(WorkerFactory.Device.GPU);
    }

    var inputs = new Dictionary<string, Tensor>
    {
        { "map_summary", mapSummary },
        { "local_map", localMap },
        { "distance_from_walls", distanceFromWalls },
        { "food_info", foodInfo },
        { "move_history", moveHistory },
        { "move_direction", moveDirection }
    };

    // LOG: Before Execution (Check inputs being sent)
    Debug.Log("===== Input Tensors BEFORE Execution =====");
    foreach (var input in inputs)
    {
        Debug.Log($"{input.Key}: {input.Value.DataToString()}");
    }

    // Execute the network
    worker.Execute(inputs);

    // Get the output tensor
    var output = worker.PeekOutput("output");
    float qValue = output[0];

    // Dispose of tensors but KEEP the worker
    output.Dispose();
    moveDirection.Dispose();

    return qValue;
}


public void Dispose()
{
    worker?.Dispose();
    worker = null;
}
}