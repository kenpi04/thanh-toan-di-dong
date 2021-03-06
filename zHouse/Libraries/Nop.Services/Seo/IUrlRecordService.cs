using Nop.Core;
using Nop.Core.Domain.Seo;
using System.Threading.Tasks;

namespace Nop.Services.Seo
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial interface  IUrlRecordService
    {
        /// <summary>
        /// Deletes an URL record
        /// </summary>
        /// <param name="urlRecord">URL record</param>
        void DeleteUrlRecord(UrlRecord urlRecord);

        /// <summary>
        /// Gets an URL record
        /// </summary>
        /// <param name="urlRecordId">URL record identifier</param>
        /// <returns>URL record</returns>
        UrlRecord GetUrlRecordById(int urlRecordId);
        Task<UrlRecord> GetUrlRecordByIdAsync(int urlRecordId);
        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="urlRecord">URL record</param>
        void InsertUrlRecord(UrlRecord urlRecord);

        /// <summary>
        /// Updates the URL record
        /// </summary>
        /// <param name="urlRecord">URL record</param>
        void UpdateUrlRecord(UrlRecord urlRecord);

        /// <summary>
        /// Find URL record
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <returns>Found URL record</returns>
        UrlRecord GetBySlug(string slug);
        Task<UrlRecord> GetBySlugAsync(string slug);
        /// <summary>
        /// Find URL record
        /// </summary>
        /// <param name="slug">slug</param>
        /// <param name="categoryId">Category output</param>
        /// <param name="streetId">Street output</param>
        /// <param name="wardId">Ward out put</param>
        /// <param name="districtId">District out put</param>
        /// <param name="stateProvinceId">Stateprovince out put</param>
        /// <returns></returns>
        UrlRecord GetBySlug(string slug, out int categoryId, out int streetId, out int wardId, out int districtId, out int stateProvinceId, out string priceString, out string areaString, out string specAttributeOptionIds, out string keywords);

        /// <summary>
        /// Gets all URL records
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Customer collection</returns>
        IPagedList<UrlRecord> GetAllUrlRecords(string slug, int pageIndex, int pageSize);
        Task<IPagedList<UrlRecord>> GetAllUrlRecordsAsync(string slug, int pageIndex, int pageSize);
        /// <summary>
        /// Find slug
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="entityName">Entity name</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Found slug</returns>
        string GetActiveSlug(int entityId, string entityName, int languageId);
        Task<string> GetActiveSlugAsync(int entityId, string entityName, int languageId);
        /// <summary>
        /// Save slug
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="slug">Slug</param>
        /// <param name="languageId">Language ID</param>
        void SaveSlug<T>(T entity, string slug, int languageId) where T : BaseEntity, ISlugSupported;

        /// <summary>
        /// Get Slug from id elements
        /// </summary>
        /// <param name="domainName">Domain name: zhouse.com</param>
        /// <param name="categoryId">Category Id</param>
        /// <param name="stateProvinceId">Stateprovince id</param>
        /// <param name="districtId">District id</param>
        /// <param name="wardId">Ward id</param>
        /// <param name="streetId">Street id</param>
        /// <param name="priceString">Price string: 1000-1500</param>
        /// <param name="attributeOptionIds">id specification options id:  1-2-3-4-5</param>
        /// <returns>Link request: http://zhouse.com/nha-o-quan-1_pr-1000-15000_sa-1-2-3-4-5</returns>
        string GetSlugFromId(string domainName, int categoryId = 0, int stateProvinceId = 0, int districtId = 0, int wardId = 0, int streetId = 0, string priceString = "", string areaString = "", string attributeOptionIds = "", string sku = "");
        Task<string> GetSlugFromIdAsync(string domainName, int categoryId = 0, int stateProvinceId = 0, int districtId = 0, int wardId = 0, int streetId = 0, string priceString = "", string areaString = "", string attributeOptionIds = "", string sku = "");
    }
}