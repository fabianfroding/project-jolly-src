using System.Collections.Generic;

public class UpdateManager : Manager<UpdateManager>
{
    private static readonly List<ILogicUpdate> LogicUpdateObservers = new();
    private static readonly List<ILogicUpdate> PendingLogicUpdateObservers = new();
    private static int currentLogicUpdateIndex;
    
    private static readonly List<IPhysicsUpdate> PhysicsUpdateObservers = new();
    private static readonly List<IPhysicsUpdate> PendingPhysicsUpdateObservers = new();
    private static int currentPhysicsUpdateIndex;

    private void Update()
    {
        for (currentLogicUpdateIndex = LogicUpdateObservers.Count - 1; currentLogicUpdateIndex >= 0; currentLogicUpdateIndex--)
        {
            LogicUpdateObservers[currentLogicUpdateIndex].LogicUpdate();
        }

        LogicUpdateObservers.AddRange(PendingLogicUpdateObservers);
        PendingLogicUpdateObservers.Clear();
    }

    private void FixedUpdate()
    {
        for (currentPhysicsUpdateIndex = PhysicsUpdateObservers.Count - 1; currentPhysicsUpdateIndex >= 0; currentPhysicsUpdateIndex--)
        {
            PhysicsUpdateObservers[currentPhysicsUpdateIndex].PhysicsUpdate();
        }

        PhysicsUpdateObservers.AddRange(PendingPhysicsUpdateObservers);
        PendingPhysicsUpdateObservers.Clear();
    }

    public static void RegisterLogicUpdate(ILogicUpdate logicUpdate)
    {
        PendingLogicUpdateObservers.Add(logicUpdate);
    }

    public static void UnregisterLogicUpdate(ILogicUpdate logicUpdate)
    {
        LogicUpdateObservers.Remove(logicUpdate);
        currentLogicUpdateIndex--;
    }
    
    public static void RegisterPhysicsUpdate(IPhysicsUpdate physicsUpdate)
    {
        PendingPhysicsUpdateObservers.Add(physicsUpdate);
    }

    public static void UnregisterPhysicsUpdate(IPhysicsUpdate physicsUpdate)
    {
        PhysicsUpdateObservers.Remove(physicsUpdate);
        currentPhysicsUpdateIndex--;
    }
}