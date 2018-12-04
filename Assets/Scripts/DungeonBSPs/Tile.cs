using UnityEngine;
using System.Collections;

public class Tile {
	// Tile Types
	public const int TILE_EMPTY = 0;
	public const int TILE_FLOOR = 1;
	
	// Tile ID
	public int id;
	
	public Tile ( int _id )
	{
		this.id = _id;
	}
}