using System;
using System.Numerics;
using System.ComponentModel;
using System.Diagnostics;
using DGD203_2;

public class Map
{
	private Game _theGame;

	private Vector2 _coordinates;

	private int[] _widthBoundaries;
	private int[] _heightBoundaries;

	private Location[] _locations;


	public Map(Game game, int width, int height)
	{
		_theGame = game;

		// Setting the width boundaries
		int widthBoundary = (width - 1) / 2;

        _widthBoundaries = new int[2];
        _widthBoundaries[0] = -widthBoundary;
		_widthBoundaries[1] = widthBoundary;

		// Setting the height boundaries
        int heightBoundary = (height - 1) / 2;

        _heightBoundaries = new int[2];
		_heightBoundaries[0] = -heightBoundary;
		_heightBoundaries[1] = heightBoundary;

		// Setting starting coordinates
        _coordinates = new Vector2(0,0);

		GenerateLocations();

		// Display result message
		Console.WriteLine($"Created map with size {width}x{height}");
    }

    #region Coordinates

    public Vector2 GetCoordinates()
	{
		return _coordinates;
	}

	public void SetCoordinates(Vector2 newCoordinates)
	{
		_coordinates = newCoordinates;
	}

	#endregion

	#region Movement

	public void MovePlayer(int x, int y)
	{
		int newXCoordinate = (int)_coordinates[0] + x;
        int newYCoordinate = (int)_coordinates[1] + y;

		if (!CanMoveTo(newXCoordinate, newYCoordinate)) 
		{
            Console.WriteLine("You can't go that way");
            return;
        }

		_coordinates[0] = newXCoordinate;
		_coordinates[1] = newYCoordinate;

		CheckForLocation(_coordinates);
    }

	private bool CanMoveTo(int x, int y)
	{
		return !(x < _widthBoundaries[0] || x > _widthBoundaries[1] || y < _heightBoundaries[0] || y > _heightBoundaries[1]);
	}

	#endregion

	#region Locations

	private void GenerateLocations()
	{
        _locations = new Location[11];

        Vector2 marikaLocation = new Vector2(0, 0);
		List<Item> marikaItems = new List<Item>();
		marikaItems.Add(Item.Crystal);
        Location marika = new Location("Marika", LocationType.City, marikaLocation, marikaItems);
        _locations[0] = marika;

        Vector2 siofraLocation = new Vector2(-2, 2);
		List<Item> siofraItems = new List<Item>();
		siofraItems.Add(Item.Talisman);
        Location siofra = new Location("Siofra", LocationType.City, siofraLocation, siofraItems);
        _locations[1] = siofra;

        Vector2 caelidLocation = new Vector2(1, -2);
		List<Item> caelidItems = new List<Item>();
		caelidItems.Add(Item.Rune);
        Location caelid = new Location("Caelid", LocationType.City, caelidLocation, caelidItems);
        _locations[2] = caelid;

        Vector2 leyndellLocation = new Vector2(1, 1);
        Location leyndell = new Location("Leyndell", LocationType.City, leyndellLocation);
		_locations[3] = leyndell;

		Vector2 altusLocation = new Vector2(-1, -1);
        List<Item> altusItems = new List<Item>();
        altusItems.Add(Item.Rune);
        Location altus = new Location("Altus", LocationType.Cave, altusLocation, altusItems);
        _locations[4] = altus;

        Vector2 ainselLocation = new Vector2(2, 3);
        List<Item> ainselItems = new List<Item>();
        ainselItems.Add(Item.Talisman);
        Location ainsel = new Location("Ainsel", LocationType.City, ainselLocation, ainselItems);
        _locations[5] = ainsel;

        Vector2 liurniaLocation = new Vector2(-3, -3);
        List<Item> liurniaItems = new List<Item>();
		liurniaItems.Add(Item.Remembrance);
        Location liurnia = new Location("Liurnia", LocationType.City, liurniaLocation, liurniaItems);
        _locations[6] = liurnia;

        Vector2 firstCombatLocation = new Vector2(-2, 1);
		Location firstCombat = new Location("First Combat", LocationType.Combat, firstCombatLocation);
		_locations[7] = firstCombat;

        Vector2 secondCombatLocation = new Vector2(0, -2);
        Location secondCombat = new Location("Second Combat", LocationType.Combat, secondCombatLocation);
        _locations[8] = secondCombat;

		Vector2 thirdCombatLocation = new Vector2(-1, 3);
		Location thirdCombat = new Location("Third Combat", LocationType.Combat, thirdCombatLocation);
		_locations[9] = thirdCombat;

        Vector2 fourthCombatLocation = new Vector2(3, 1);
        Location fourthCombat = new Location("Fourth Combat", LocationType.Combat, fourthCombatLocation);
        _locations[10] = fourthCombat;

    }

	public void CheckForLocation(Vector2 coordinates)
	{
        Console.WriteLine($"You are now standing on {_coordinates[0]},{_coordinates[1]}");

        if (IsOnLocation(_coordinates, out Location location))
        {
            if (location.Type == LocationType.Combat)
			{
				if (location.CombatAlreadyHappened) return;

				Console.WriteLine("Prepare to fight!");
				Combat combat = new Combat(_theGame, location);

				combat.StartCombat();

			} else
			{
				Console.WriteLine($"You are in {location.Name} {location.Type}");

				if (HasItem(location))
				{
					Console.WriteLine($"There is a {location.ItemsOnLocation[0]} here");
				}
			}
        }
    }

	private bool IsOnLocation(Vector2 coords, out Location foundLocation)
	{
		for (int i = 0; i < _locations.Length; i++)
		{
			if (_locations[i].Coordinates == coords)
			{
				foundLocation = _locations[i];
				return true;
			}
		}

		foundLocation = null;
		return false;
	}

	private bool HasItem(Location location)
	{
		return location.ItemsOnLocation.Count != 0;

		// ---- THE LONG FORM ----
		//if (location.ItemsOnLocation.Count == 0)
		//{
		//	return false;
		//} else
		//{
		//	return true;
		//}
	}

	public void TakeItem(Location location)
	{

	}

	public void TakeItem(Player player, Vector2 coordinates)
	{
		if (IsOnLocation(coordinates, out Location location))
		{
			if (HasItem(location))
			{
				Item itemOnLocation = location.ItemsOnLocation[0];

				player.TakeItem(itemOnLocation);
				location.RemoveItem(itemOnLocation);

				Console.WriteLine($"You took the {itemOnLocation}");

				return;
			}
		}

		Console.WriteLine("There is nothing to take here!");
	}

	public void RemoveItemFromLocation(Item item)
	{
		for (int i = 0; i < _locations.Length; i++)
		{
			if (_locations[i].ItemsOnLocation.Contains(item))
			{
				_locations[i].RemoveItem(item);
			}
		}
	}

	#endregion
}