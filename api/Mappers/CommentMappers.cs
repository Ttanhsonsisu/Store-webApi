using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment commentModel) {
            return new CommentDto {
                ID = commentModel.ID,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreateOn = commentModel.CreateOn,
                CreatedBy = commentModel.AppUser.UserName,
                StockID = commentModel.StockID
            };
        }

       public static Comment ToCommentCreateDto(this CreateCommentDto commentDto, int stockId) {
            return new Comment {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockID = stockId
            };
        }

        public static Comment ToCommentUpdateDto(this UpdateCommentRequestDto commentDto, int stockId) {
            return new Comment {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockID = stockId,
            };
        }

    }
}