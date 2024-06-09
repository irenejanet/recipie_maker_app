using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel; // for observable collections

namespace Recipe_Maker_Frontend;

public partial class MainPage : ContentPage
{
    private readonly ImagePicker imagePicker;

    //using observable collections instead of lists because they can automatically update ui
    public ObservableCollection<Recipe> Recipes { get; set; }
    public MainPage()
    {
        InitializeComponent();
        dietaryPreferences.SelectedIndex = 0;
        imagePicker = new ImagePicker();
        Recipes = new ObservableCollection<Recipe>();
        recipeList.BindingContext = this;
    }

    private async void OnScanButtonClicked(object sender, EventArgs e)
    {
        // Shows when button is clicked
        scanButton.BackgroundColor = Color.FromRgb(211, 211, 211);
        Recipes.Clear();

        // Handle ingredient scanning

        // Process the scanned image
        List<Recipe> recipes = await ComputerVision.Processing();

        //Prints the list of ingredients
        foreach (var recipe in recipes)
        {
            Recipes.Add(recipe);
        }

        //resets button color after being clicked
        scanButton.BackgroundColor = Color.FromRgb(169, 169, 169);

    }

    private async void OnRecipeSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Handle recipe selection


        if (e.SelectedItem == null)
        {
            return;
        }


        //set the recipe selected by user to a variable
        Recipe selectedRecipe = e.SelectedItem as Recipe;

        //create a popup page using the selected recipe
        var popupPage = new SelectedRecipePage(selectedRecipe);
        await Navigation.PushModalAsync(popupPage);

    }
    private void OnFavouriteButtonClicked(object sender, EventArgs e)
    {

        //if not in favourites table then add to the favourites table (kinda need backend for this one)

        //if in favourites table then remove from favourites table
        
        //(possibly change color of button depending on if in fav table yet)


    }
}