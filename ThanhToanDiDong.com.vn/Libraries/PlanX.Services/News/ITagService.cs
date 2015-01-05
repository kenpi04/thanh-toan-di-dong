using System.Collections.Generic;
using PlanX.Core.Domain.News;
using System.Threading.Tasks;

namespace PlanX.Services.News
{
    public interface ITagService
    {
        /// <summary>
        /// Delete a news tag
        /// </summary>
        /// <param name="tag">tag</param>
        void DeleteTag(Tag tag);

        /// <summary>
        /// Gets all tags
        /// </summary>
        /// <returns>tags</returns>
        IList<Tag> GetAllTags(bool isShowHomePage = false);
        Task<IList<Tag>> GetAllTagsAsync(bool isShowHomePage = false);
        /// <summary>
        /// Get all tags by category news id
        /// </summary>
        /// <param name="categoryNewsId">category news id</param>
        /// <param name="pageSize">pagesizes</param>
        /// <returns>tags</returns>
        IList<Tag> GetAllTagsByCategoryNewsId(int categoryNewsId, int pageSize = 10);
        /// <summary>
        /// Gets tag
        /// </summary>
        /// <param name="tagId">Tag identifier</param>
        /// <returns>Tag</returns>
        Tag GetTagById(int tagId);

        /// <summary>
        /// Gets tag by name
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <returns>Tag</returns>
        Tag GetTagByName(string name);

        /// <summary>
        /// Inserts a tag
        /// </summary>
        /// <param name="tag">Tag</param>
        void InsertTag(Tag tag);

        /// <summary>
        /// Updates the tag
        /// </summary>
        /// <param name="tag">Tag</param>
        void UpdateTag(Tag tag);

        /// <summary>
        /// Get number of news
        /// </summary>
        /// <param name="tagId">Tag identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Number of products</returns>
        int GetNewsCount(int tagId, int storeId);
    }
}
