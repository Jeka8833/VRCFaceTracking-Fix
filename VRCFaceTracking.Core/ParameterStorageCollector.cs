using System.Collections.Concurrent;
using VRCFaceTracking.Core.Params.Data;

namespace VRCFaceTracking.Core;

public class ParameterStorageCollector
{
    private static readonly UnifiedTrackingData CombinedData = new();
    
    private static ConcurrentDictionary<Thread, UnifiedTrackingData> _trackingData = new();
    private static ConcurrentDictionary<Thread, ExtTrackingModule> _trackingModules = new();
    
    public static void RegisterModule(ExtTrackingModule module, Thread thread)
    {
        _trackingData.TryAdd(thread, new UnifiedTrackingData());
        _trackingModules.TryAdd(thread, module);
    }
    
    public static UnifiedTrackingData GetTrackingData()
    {
        return _trackingData.GetValueOrDefault(Thread.CurrentThread, CombinedData);
    }

    public static void ModuleEndedUpdate()
    {
        UnifiedTrackingData data = _trackingData[Thread.CurrentThread];
        ExtTrackingModule module = _trackingModules[Thread.CurrentThread];
        
        Console.WriteLine($"{module.ModuleInformation.Name} ended update -> {string.Join(",", data.Shapes)}");
        
        
        // Mutate CombinedData to send data to other components of VRCFT
    }
}