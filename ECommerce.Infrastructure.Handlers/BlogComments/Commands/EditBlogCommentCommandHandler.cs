using AutoMapper;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogComments.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogComments.Commands
{
    public class EditBlogCommentCommandHandler(IMapper mapper, IUnitOfWork unitOfWork) :
        ICommandHandler<EditBlogCommentCommand, bool>
    {
        private readonly IBlogCommentRepository _blogCommentRepository = unitOfWork.GetRepository<BlogCommentRepository, BlogComment>();
        private BlogComment _blogComment = new();

        public async Task<bool> HandleAsync(EditBlogCommentCommand command, CancellationToken cancellationToken)
        {
            _blogComment = mapper.Map<BlogComment>(command);

            BlogComment? commentAnswer;
            if (command.AnswerId != null)
            {
                commentAnswer = await _blogCommentRepository.GetByIdAsync(cancellationToken, command.AnswerId);
                if (commentAnswer != null)
                {
                    commentAnswer.DateTime = DateTime.Now;
                    _blogCommentRepository.Update(commentAnswer);
                }
            }
            else
            {
                if (command.Answer?.Text != null)
                {
                    command.Answer.Name = "پاسخ ادمین";
                    command.Answer.IsAccepted = false;
                    command.Answer.IsRead = false;
                    command.Answer.IsAnswered = false;
                    command.Answer.DateTime = DateTime.Now;
                    commentAnswer = await _blogCommentRepository.AddAsync(command.Answer, cancellationToken);

                    if (commentAnswer != new BlogComment())
                    {
                        command.Answer = commentAnswer;
                        command.AnswerId = commentAnswer.Id;
                    }
                }
            }
            _blogCommentRepository.Update(_blogComment);
            await unitOfWork.SaveAsync(cancellationToken);
            return true;
        }
    }
}
