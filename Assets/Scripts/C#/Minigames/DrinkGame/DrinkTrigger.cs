/// <summary>
/// Interact with Drinkobject
/// </summary>
public class DrinkTrigger : Interactable
{ 
	public override void Interact()
	{
	 	GetComponent<Drink>().DrinkBottle();
	}
}
