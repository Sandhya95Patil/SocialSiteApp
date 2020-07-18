﻿using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ICommentRL
    {
        Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById);
        IList<CommentsModel> GetAllComments(int userId, int postId);

    }
}
