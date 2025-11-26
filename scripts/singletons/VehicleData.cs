using Godot;
using System.Runtime.InteropServices;

// C# structs are value types, which are stored efficiently in memory.
// This struct will hold the minimum information needed for the GPU to draw and move a unit.
[StructLayout(LayoutKind.Sequential)]
public struct VehicleData // <-- Note: This is a struct, not a class
{
    // The position is all the GPU needs to draw the vehicle.
    public Vector2 Position; 
    
    // We add a 'velocity' for the movement logic (the 'system').
    public Vector2 Velocity; 
    
    // An ID helps us debug and track the vehicle.
    public int ID;
}