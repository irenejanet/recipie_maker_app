using System.Collections.ObjectModel; //for observable collections

namespace Recipe_Maker_Frontend;

public partial class FavRecipesPage : ContentPage
{
    
    
    public ObservableCollection<Recipe> FavRecipes { get; set; }
    public FavRecipesPage()
	{
		InitializeComponent();
        
        FavRecipes = new ObservableCollection<Recipe>();

        //another test result (while we dont have backend, made-up result to test ui)
        var testRecipe = new Recipe
        {
            Name = "Pasta",
            Instructions = "Put pasta in pan of boiling water",
            Complexity = "Easy"
        };


        FavRecipes.Add(testRecipe);

        favRecipeList.BindingContext = this;

    }
    private async void OnRecipeSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Handle recipe selection

        if (e.SelectedItem == null)
        {
            return;
        }

        //similar to main page, will create a pop up page with the selected recipes info
        Recipe selectedRecipe = e.SelectedItem as Recipe;

        var popupPage = new SelectedRecipePage(selectedRecipe);
        await Navigation.PushModalAsync(popupPage);

    }

    //might not end up needing this, but is here for testing purposes
    private void OnRefreshClicked(object sender, EventArgs e)
    {
        
        
        FavRecipes.Clear();

        //another test result (while we dont have backend, made-up result to test ui)
        var testRecipe = new Recipe
        {
            Name = "Pasta",
            Instructions = "Put pasta in pan",
            Complexity = "20"
        };


        FavRecipes.Add(testRecipe);
    }
}