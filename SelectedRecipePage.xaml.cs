namespace Recipe_Maker_Frontend;

public partial class SelectedRecipePage : ContentPage
{
	public SelectedRecipePage(Recipe selectedRecipe)
	{
		InitializeComponent();
		RecipeDisplay.BindingContext = selectedRecipe;
	}

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
		
		//closes page after back button pressed
		await Navigation.PopModalAsync();
    }
}