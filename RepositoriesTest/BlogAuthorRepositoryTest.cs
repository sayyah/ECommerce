using API.DataContext;
using API.Repository;
using Entities;
using Entities.Helper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace RepositoriesTest
{
    public class BlogAuthorRepositoryTest
    {
        public SunflowerECommerceDbContext context;
        public CancellationToken cancellationToken = new CancellationToken();
        public BlogAuthorRepository repository;
        public IEnumerable<BlogAuthor?> blogAuthors;
        public BlogAuthorRepositoryTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SunflowerECommerceDbContext>()
                .EnableSensitiveDataLogging(true)
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new SunflowerECommerceDbContext(optionsBuilder.Options, new EphemeralDataProtectionProvider(), new ConfigurationManager());
            repository = new BlogAuthorRepository(context);
            AddRange();
        }

        public async void AddRange()
        {
            blogAuthors = Setup();
            await repository.AddRangeAsync(blogAuthors, cancellationToken);
        }

        [Fact]
        public async void SearchExistedBlogAuthor()
        {
            //Arrange
            PaginationParameters paginationParameters = new PaginationParameters();
            //paginationParameters.PageSize = 10;
            //paginationParameters.PageNumber = 1;
            paginationParameters.Search = "علی";
            //Act
            var result = await repository.Search(paginationParameters, cancellationToken);
            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }
        [Fact]
        public async void GetByNameReturnTrue()
        {
            //Arrange
            string TrueName = "مجتبی خامسی";
            //Act
            var result = await repository.GetByName(TrueName,cancellationToken);
            //Assert
            Assert.Equal(1,result.Id);
        }
        public IEnumerable<BlogAuthor?> Setup()
        {
            var BlogAuthors = new List<BlogAuthor>()
            {
                new BlogAuthor(){Id=1,Name="مجتبی خامسی", EnglishName="mojtaba khamesi", ImagePath="/Images/BlogAuthors/dd3aa2d4-6d55-4d76-b59b-7a3dcf50e96c.jpg",Description="dsp"},
                new BlogAuthor(){Id=2,Name="سامان رئوفی", EnglishName="saman raufi", ImagePath="/Images/BlogAuthors/dd3aa2d4-6d55-4d76-b59b-7a3dcf50e96c.jpg",Description="dsp"},
                new BlogAuthor(){Id=3,Name="علی کریمی", EnglishName="ali karimi", ImagePath="/Images/BlogAuthors/dd3aa2d4-6d55-4d76-b59b-7a3dcf50e96c.jpg",Description="dsp"},
                new BlogAuthor(){Id=4,Name="علیرضا", EnglishName="alireza", ImagePath="/Images/BlogAuthors/dd3aa2d4-6d55-4d76-b59b-7a3dcf50e96c.jpg",Description="dsp"},
                new BlogAuthor(){Id=5,Name="حسین", EnglishName="hosein", ImagePath="/Images/BlogAuthors/dd3aa2d4-6d55-4d76-b59b-7a3dcf50e96c.jpg",Description="dsp"},
                new BlogAuthor(){Id=6,Name="رویا", EnglishName="roya", ImagePath="/Images/BlogAuthors/dd3aa2d4-6d55-4d76-b59b-7a3dcf50e96c.jpg",Description=null}
            };
            return BlogAuthors;
        }
    }
}
