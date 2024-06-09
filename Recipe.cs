using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Maker_Frontend
{
    public class Recipe
    {
        
        //might need to alter this depending on the data given by backend
        
        //set the recipe name to a variable
        public string Name { get; set; }
        
        //Set the instructions to a variable
        public string Instructions { get; set; }
        
        // Set the complexity of a recipe
        public string Complexity { get; set; }

    }
}
