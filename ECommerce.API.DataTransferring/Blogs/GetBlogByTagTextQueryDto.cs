using ECommerce.Application.Services.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.API.DataTransferObject.Blogs
{
    public class GetBlogByTagTextQueryDto
    {
        public PaginationParameters PaginationParameters { get; set; } = new();
        public string TagText { get; set; }
    }
}
