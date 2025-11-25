# This is a custom resource or class that defines the structure and logic of a single card object, independent of its visual representation.

# scripts/classes/Card.gd
extends Resource
class_name Value

# Exports
@export var value_id: String
@export var value_name: String = "Unnamed Value"
@export var description: String
@export var influence_cost: int = 0
@export var tags: Array # e.g., ["Economy", "Disaster"]

# Add a function to execute the card's effect
func execute_effect():
    print("Executing effect for: " + value_name)
    # Your core game logic goes here
    pass