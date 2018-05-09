using System;

namespace Collect
{
	public interface IUIElement
	{
	  //draw element
	  void Draw(int mouseX, int mouseY);
	  //checks if the element is pointed at by mouse
	  bool IsPointed(int mouseX, int mouseY);
	  //defines what happens when clicked
	  void Click(int mouseX, int mouseY);
	}

	public class UIElementButton :  IUIElement
	{
	  public UIElementButton()
	  {
	  }

	  void Draw(int mouseX, int mouseY)
	  {
	  		return;
	  }

	  bool IsPointed(int mouseX, int mouseY)
	  {
	  	return false;
	  }

	  void Click(int mouseX, int mouseY)
	  {
	  		return;
	  }
	}
	class Ui 
	{
		static void Main(string[] args)
		{
			List<IUIElement> qwe = new List<IUIElement>();
			qwe.Add(new UIElementButton());
			qwe.Add(new UIElementLabel());
			qwe.Add(new UIElementSomethingElse());

			// some sample usages
			foreach(IUIElement element in qwe)
			{
			   element.Click();
			}

			qwe[0].Click(); //invokes UIElementButton.Click();		
		}
	}
}