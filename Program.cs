using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;
using System.Data.Entity.SqlServer;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using MySqlConnector;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Security.Cryptography;
using Mysqlx.Cursor;

namespace Recipe_Maker_Frontend;
public class ComputerVision
{
    static string key = "63427614a82a4216a781cb751802503f";
    static string endpoint = "https://cvforrecipes.cognitiveservices.azure.com/";

    //Change the password and database name to match your own
    const string connectionString = "Server=localhost; User ID=root; Password=computing; Database=test";


    public static async Task<List<Recipe>> Processing()
    {

        var conn = new MySqlConnection(connectionString);

        //list of user ingredients

        List<string> ingredient_list = new List<string>();
        try
        {
            await conn.OpenAsync(); // Open the connection once

            // Fetch ingredients from the database
            string stmt = "select ingredient_name from ingredients;";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = stmt;

            using (var res = await cmd.ExecuteReaderAsync())
            {
                while (await res.ReadAsync())
                {
                    ingredient_list.Add(res.GetString("ingredient_name"));
                }
            }

            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, key);

            var pc_Path = @"C:\Users\INCLUDE THE PATH OF THE ADDRESS WHERE THE PROGRAM IS LOCATED IN\";
            var folder = Path.Combine(pc_Path, "images");
            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                await AnalyzeImageFile(client, file, ingredient_list);
                ingredient_list.Add(await File.ReadAllTextAsync(Path.Combine(pc_Path, "ing.txt")));
            }

            // Uploading the list to the database
            int ingredient_id = 1;
            //Deletes all items from the user_ingredients table to prevent duplicate entries
            stmt = "delete from user_ingredients where user_id > 0";
            cmd.CommandText = stmt;
            await cmd.ExecuteNonQueryAsync();
            foreach (var item in ingredient_list)
            {
                stmt = "insert into user_ingredients (user_id, ingredients_id, name) values ('" + ingredient_id + "' , '" + ingredient_id + "' , '" + item + "');";
                cmd.CommandText = stmt;
                await cmd.ExecuteNonQueryAsync();
                ingredient_id++;
            }
        }
        finally
        {
            conn.Close(); // Close the connection
        }
        List<Recipe> recipes = FetchRecipes(ingredient_list);

        return recipes;
    }

    public static ComputerVisionClient Authenticate(string endpoint, string key)
    {
        ComputerVisionClient client =
          new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
          { Endpoint = endpoint };
        return client;
    }



    public static async Task AnalyzeImageFile(ComputerVisionClient client, string filename, List<string> list)
    {

        string ingredient_list = "";
        using (var imagestream = System.IO.File.OpenRead(filename))
        {


            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Tags
            };

            var results = await client.AnalyzeImageInStreamAsync(imagestream, visualFeatures: features);
            //check bit
            int parity = 1;
            foreach (var tag in results.Tags)
            {
                Console.WriteLine(tag.Name.ToString());

                foreach (var item in list)
                {

                    if (item.ToLower() == tag.Name.ToString())
                    {
                        parity = 0;
                        ingredient_list = (item);
                        break;
                    }
                }
                if (parity == 0)
                {
                    break;
                }
            }
            if (parity == 1)
            {
                ingredient_list = "None";
            }
            await File.WriteAllTextAsync(@"C:\Users\INCLUDE THE PATH OF THE ADDRESS WHERE THE PROGRAM IS LOCATED IN\ing.txt", ingredient_list);
        }

    }

    public static List<Recipe> FetchRecipes(List<string> ingredientList)
    {
        List<Recipe> recipes = new List<Recipe>();

        // Fetch recipes from the database
        string query = "SELECT DISTINCT r.recipe_id, r.recipe_name, r.recipe_instructions, r.recipe_complexity " +
                   "FROM recipe r " +
                   "JOIN recipe_ingredients ri ON r.recipe_id = ri.recipe_id " +
                   "JOIN ingredients i ON ri.ingredients_id = i.ingredients_id " +
                   "WHERE i.ingredient_name IN ('" + string.Join("', '", ingredientList) + "')";

        //Execute query and retrieve recipes
        var conn = new MySqlConnection(connectionString);
        try
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            using (var res = cmd.ExecuteReader())
            {
                while (res.Read())
                {
                    Recipe recipe = new Recipe();
                    recipe.Name = res.GetString("recipe_name");
                    recipe.Instructions = res.GetString("recipe_instructions");
                    recipe.Complexity = res.GetString("recipe_complexity");
                    recipes.Add(recipe);
                }
            }
        }
        finally
        {
            conn.Close();
        }

        return recipes;
    }

}