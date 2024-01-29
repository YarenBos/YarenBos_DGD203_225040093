using System;

public class Golem : Enemy
{

	private const int golemHealth = 20;

	private const int golemMinDamage = 3;
	private const int golemMaxDamage = 10;

	/*
	public int Damage
	{
		get
		{
			return golemMaxDamage;
			//Random newRandom = new Random();
			//return newRandom.Next(golemMinDamage, golemMaxDamage + 1);
		} protected set
		{
			Damage = value;
		}
	}
	*/

	public Golem()
	{
		Health = golemHealth;
		Damage = golemMaxDamage;
	}


}
