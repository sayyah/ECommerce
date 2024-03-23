using AutoFixture;
using AutoMapper;
using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Handlers.Blogs.Commands;
using ECommerce.Tests.Handler.Base;
using NSubstitute;

namespace ECommerce.Tests.Handler.BlogTests.CommandTests
{
    public class CreateBlogCommandHandlerTests :
        CommandHandlerTests<CreateBlogCommandHandler, CreateBlogCommand, bool>
    {
        private IMapper _mapperMock;
        private IUnitOfWork _unitOfWorkMock;
        private readonly Fixture _fixture = new();

        protected override void InitializeDependenciesMocks()
        {
            //Substitute.
        }

        protected override CreateBlogCommandHandler InitializeCommandHandler()
        {
            return new CreateBlogCommandHandler(_mapperMock, _unitOfWorkMock);
        }

        protected override CreateBlogCommand CreateValidCommand()
        {
            return _fixture.Build<CreateBlogCommand>().Create();
        }

        protected override void SetupDependenciesMocks()
        {

        }

    }
}
