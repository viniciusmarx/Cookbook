using Cookbook.Domain.Repositories.Recipe;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories;

public class RecipeRepositoryBuilder
{
    public static IRecipeRepository Build()
    {
        var mock = new Mock<IRecipeRepository>();

        return mock.Object;
    }
}
