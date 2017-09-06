using System;
using System.Collections.Generic;
using System.Text;
using InMemDb.Data;
using InMemDb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BennysPizza.UnitTests
{
    public class IngredientServiceTest
    {
        private readonly IServiceProvider _serviceProvider;

        public IngredientServiceTest()
        {
            var efServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(b => b.UseInMemoryDatabase("Pizzadatabas")
                .UseInternalServiceProvider(efServiceProvider));
            services.AddTransient<IngredientService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void All_Are_Sorted()
        {
            var _ingredients = _serviceProvider.GetService<IngredientService>();
            var ings = _ingredients.AllIngredients();
            Assert.Equal(ings.Count, 0);
        }
    }
}
