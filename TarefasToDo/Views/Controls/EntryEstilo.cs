namespace TarefasToDo.Views.Controls;

public class EntryEstilo : ContentView
{
	public EntryEstilo()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
				}
			}
		};
	}
}