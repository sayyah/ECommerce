using API.DataContext;
using API.Repository;
using Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RepositoriesTest
{
    public class ColorRepositoryTest
    {
        public SunflowerECommerceDbContext context;
        public CancellationToken cancellationToken = new CancellationToken();
        public ColorRepository repository;
        public IEnumerable<Color?> colors;
        public ColorRepositoryTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SunflowerECommerceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new SunflowerECommerceDbContext(optionsBuilder.Options, new EphemeralDataProtectionProvider(),new ConfigurationManager());
            repository = new ColorRepository(context);
            colors = Setup();
            AddRang();
        }

        public async void AddRang()
        {
            colors = Setup();
            await repository.AddRangeAsync(colors, cancellationToken);
        }

        [Fact]
        public async void AddAsync()
        {
            //Arrange 
            Color? color = new Color();
            color.Id = 18; color.Name = "NewColor"; color.ColorCode = "#f58120";

            //Act
            var result = await repository.AddAsync(color, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("NewColor", result.Name);
        }

        [Theory]
        [InlineData(1, "DarkBlue", "#856984")]
        [InlineData(18, "Black", "#587541")]
        [InlineData(18, "DarkBlue", "#00FFFF")]
        public async void AddAsyncWhithDuplicateValuesRetunNull(int id, string name, string colorcode)
        {
            //Arrange 
            Color? color = new Color();
            color.Id = id; color.Name = name; color.ColorCode = colorcode;

            //Act
            var result = await repository.AddAsync(color, cancellationToken);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetById()
        {
            //Arrange 


            //Act
            var result = await repository.GetByIdAsync(cancellationToken, 1);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Aqua");
        }
        public IEnumerable<Color?> Setup()
        {
            var Colors = new List<Color>()
            {
                new Color() {Id=1,Name="Aqua",ColorCode="#00FFFF"},
                new Color() {Id=2,Name="Black",ColorCode="#000000"},
                new Color() {Id=3,Name="Blue",ColorCode="#0000FF"},
                new Color() {Id=4,Name="Fuchsia",ColorCode="#FF00FF"},
                new Color() {Id=5,Name="Gray",ColorCode="#808080"},
                new Color() {Id=6,Name="Green",ColorCode="#008000"},
                new Color() {Id=7,Name="Lime",ColorCode="#00FF00"},
                new Color() {Id=8,Name="Maroon",ColorCode="#800000"},
                new Color() {Id=9,Name="Navy",ColorCode="#000080"},
                new Color() {Id=10,Name="Olive",ColorCode="#808000"},
                new Color() {Id=11,Name="Orange",ColorCode="#FFA500"},
                new Color() {Id=12,Name="Purple",ColorCode="#800080"},
                new Color() {Id=13,Name="Red",ColorCode="#FF0000"},
                new Color() {Id=14,Name="Silver",ColorCode="#C0C0C0"},
                new Color() {Id=15,Name="Teal",ColorCode="#008080"},
                new Color() {Id=16,Name="White",ColorCode="#FFFFFF"},
                new Color() {Id=17,Name="Yellow",ColorCode="#FFFF00"}
            };
            return Colors;
        }
    }
}