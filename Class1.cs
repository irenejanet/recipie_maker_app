using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Maker_Frontend;


// entity class
// table -> ingredients
public class Ingredients
{
    public int Id { get; set; }
    public string Property { get; set; }
}


//data base context

public class Ingredients_table: DbContext
{
    


    public DbSet<Ingredients> Ingredients { get; set; }
}
