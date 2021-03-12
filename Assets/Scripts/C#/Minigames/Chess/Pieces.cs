using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : Chessman
{
	public override bool[,] PossibleMove()
	{
		bool[,] r = new bool[8, 8];

		Chessman c;
		int i, j;

		//Top Left
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i--;
			j++;
			if (i < 0 || j >= 8)
			{
				break;
			}

			c = BoardManager.Instance.Chessmans[i, j];
			if (c == null)
			{
				if (i == 7 || i == 0 || j == 7 || j == 0)
				{
					r[i, j] = true;
				}
			}
			else
			{
				if (isEnemy != c.isEnemy)
				{
					r[i, j] = true;
				}

				break;
			}
		}

		//Top Right
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i++;
			j++;
			if (i >= 8 || j >= 8)
			{
				break;
			}

			c = BoardManager.Instance.Chessmans[i, j];
			if (c == null)
			{
				if (i == 7 || i == 0 || j == 7 || j == 0)
				{
					r[i, j] = true;
				}
			}
			else
			{
				if (isEnemy != c.isEnemy)
				{
					r[i, j] = true;
				}

				break;
			}
		}

		//Down Left
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i--;
			j--;
			if (i < 0 || j < 0)
			{
				break;
			}

			c = BoardManager.Instance.Chessmans[i, j];
			if (c == null)
			{
				if (i == 7 || i == 0 || j == 7 || j == 0)
				{
					r[i, j] = true;
				}
			}
			else
			{
				if (isEnemy != c.isEnemy)
				{
					r[i, j] = true;
				}

				break;
			}
		}

		//Down Right
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i++;
			j--;
			if (i >= 8 || j < 0)
			{
				break;
			}

			c = BoardManager.Instance.Chessmans[i, j];
			if (c == null)
			{
				if (i == 7 || i == 0 || j == 7 || j == 0)
				{
					r[i, j] = true;
				}
			}
			else
			{
				if (isEnemy != c.isEnemy)
				{
					r[i, j] = true;
				}

				break;
			}
		}

		return r;
	}
}
