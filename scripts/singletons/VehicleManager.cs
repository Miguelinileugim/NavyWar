using Godot;
using System;
// using System.Collections.Generic;
using System.Linq;

public partial class VehicleManager : Node2D
{
	// --- Config ---
	private const int VehicleCount = 10000;
	private const float Speed = 50.0f;
	private const string SHIP_MESH_PATH = "res://ShipMesh.res"; // Path to your ship mesh resource

	// --- Core Data Structures ---
	// Assumes VehicleData struct is defined in VehicleData.cs
	private VehicleData[] _vehicleData = new VehicleData[VehicleCount];
	
	// CORRECT: Array of Godot's Transform2D struct
	private Transform2D[] _transformArray = new Transform2D[VehicleCount]; 

	// --- Godot Nodes/Resources ---
	private MultiMeshInstance2D _multiMeshInstance;
	private Mesh _shipMesh; 

	// --- Initialization: Runs once on scene load ---
	public override void _Ready()
    {
        // 1. Load the custom mesh resource
        // NOTE: Ensure ShipMesh.res is in your project and is a valid Mesh
        _shipMesh = GD.Load<Mesh>(SHIP_MESH_PATH);

        // 2. Set up the MultiMesh node
        // NOTE: Ensure you have a MultiMeshInstance2D node named 'MultiMeshInstance2D' in your scene
        _multiMeshInstance = GetNode<MultiMeshInstance2D>("MultiMeshInstance2D");
        _multiMeshInstance.Multimesh = new MultiMesh();
        
        // Configuration
        _multiMeshInstance.Multimesh.Mesh = _shipMesh; 
        _multiMeshInstance.Multimesh.InstanceCount = VehicleCount;
        _multiMeshInstance.Multimesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform2D;
        
        // 3. Initialize the vehicle data
        Random random = new Random();
        for (int i = 0; i < VehicleCount; i++)
        {
            _vehicleData[i] = new VehicleData
            {
                ID = i,
                Position = new Vector2(random.Next(100, 900), random.Next(100, 500)),
                Velocity = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1)).Normalized() * Speed 
            };
        }
    }


	// --- Core Logic: Runs every frame ---
	public override void _Process(double delta)
	{
		float d = (float)delta;
		
		// --- 3. THE LOGIC LOOP (CPU Side) ---
		for (int i = 0; i < VehicleCount; i++)
		{
			// A. Update Movement (Simple Physics)
			_vehicleData[i].Position += _vehicleData[i].Velocity * d;
			
			// B. Simple Boundary Check
			if (_vehicleData[i].Position.X < 0 || _vehicleData[i].Position.X > 1024)
			{
				_vehicleData[i].Velocity.X *= -1;
			}
			if (_vehicleData[i].Position.Y < 0 || _vehicleData[i].Position.Y > 600)
			{
				_vehicleData[i].Velocity.Y *= -1;
			}

			// --- 4. Prepare Data for GPU ---
			
			float rotation = _vehicleData[i].Velocity.Angle() + Mathf.DegToRad(90); 
			Transform2D transform = new Transform2D(rotation, _vehicleData[i].Position); 
			
			float scale = 20.0f; 
			transform = transform.Scaled(new Vector2(scale, scale)); 

			// 5. Store the final Transform2D object in the array
			_transformArray[i] = transform;
		}

        // // Fixing bug below
        // // Assuming _transformArray is your Godot.Transform2D[]
        // Transform2D[] transforms = _transformArray;

        // // The one-line conversion:
        // Vector2[] positions = transforms
        // .Select(t => t.Origin) // Selects the Vector2 'Origin' (position) from each Transform2D
        // .ToArray();           // Converts the resulting collection into a Vector2 array

		// --- 6. Send Data to the GPU (Single Command) ---
		// Godot 4 syntax: Assign the Transform2D array property directly.
		_multiMeshInstance.Multimesh.SetInstanceTransform2D(_transformArray);
	}
}

// NOTE: You must have the 'VehicleData' struct correctly defined in a separate file (VehicleData.cs).