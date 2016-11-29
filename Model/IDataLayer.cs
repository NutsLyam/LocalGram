using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalGram.Model
{
   public interface IDataLayer
    {
        Users AddUser(Users user);
        Posts AddPost(Posts post);
        Users GetUser(Guid id);
        Posts GetPost(Guid postId);
        Comments AddComment(Comments comment);
        Comments GetComment(Guid id);
        Hashtags AddHashtag(Hashtags hashtag);
        Guid GetHashtagId(string name);
       void AddHashtagToPost(Guid PostId, Guid HashtagId);
        Hashtags GetHashtag(Guid id);
        void Mention();
        void SetLike();
        void DeletePost(Guid id);
        void DeleteComments(Guid id);
        void DeleteUser(Guid id);


    }
}
