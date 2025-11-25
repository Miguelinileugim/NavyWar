# This will be your autoload (singleton) script. It manages the global state, game flow, resources (like the deck data), and acts as a central hub for signals.

# scripts/singletons/Game_Manager.gd
extends Node

# Global variables for game state
var current_turn: int = 0
var player_deck: Array = []

func _ready():
    # Initialize the game data
    pass 

# Example function:
func end_turn():
    current_turn += 1
    print("Starting turn: " + str(current_turn))