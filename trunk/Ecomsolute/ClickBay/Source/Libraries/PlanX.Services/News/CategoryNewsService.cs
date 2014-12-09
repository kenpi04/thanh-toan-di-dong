using System;
using System.Collections.Generic;
using System.Linq;
using PlanX.Core;
using PlanX.Core.Caching;
using PlanX.Core.Data;
using PlanX.Core.Domain.News;
using PlanX.Services.Events;
using System.Transactions;
using System.Threading.Tasks;

namespace PlanX.Services.News
{
    public partial class CategoryNewsService : ICategoryNewsService
    {
        #region Constants

        private const string CATEGORIES_ALL = "Nop.newcategory.All-{0}";
        private const string CATEGORIES_BY_ID_KEY = "Nop.newcategory.id-{0}";
        private const string CATEGORIES_BY_PARENT_CATEGORY_ID_KEY = "Nop.newcategory.byparent-{0}-{1}";
        private const string NEWSCATEGORIES_ALLBYCATEGORYID_KEY = "Nop.newscategorymap.allbycategoryid-{0}-{1}-{2}-{3}";
        private const string NEWSCATEGORIES_ALLBYNEWSID_KEY = "Nop.newscategorymap.allbynewsid-{0}-{1}";
        private const string NEWSCATEGORIES_BY_ID_KEY = "Nop.newscategorymap.id-{0}";
        private const string CATEGORIES_PATTERN_KEY = "Nop.categorynew.";
        private const string NEWSCATEGORIES_PATTERN_KEY = "Nop.newscategorymap.";

        #endregion

        #region Fields

        private readonly IRepository<CategoryNews> _categoryRepository;
        private readonly IRepository<NewsCategoryNews> _newsCategoryRepository;
        private readonly IRepository<NewsItem> _newsRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        //private readonly IWebHelper _webHelper;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="categoryRepository">Category repository</param>
        /// <param name="newsCategoryRepository">NewsCategory repository</param>
        /// <param name="newsRepository">News repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public CategoryNewsService(ICacheManager cacheManager,
            IRepository<CategoryNews> categoryRepository,
            IRepository<NewsCategoryNews> newsCategoryRepository,
            IRepository<NewsItem> newsRepository,
            IEventPublisher eventPublisher, IWebHelper webHelper)
        {
            this._cacheManager = cacheManager;
            this._categoryRepository = categoryRepository;
            this._newsCategoryRepository = newsCategoryRepository;
            this._newsRepository = newsRepository;
            this._eventPublisher = eventPublisher;
            //this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete category news
        /// </summary>
        /// <param name="category">Category news</param>
        public virtual void DeleteCategory(CategoryNews category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            category.Deleted = true;
            UpdateCategory(category);
        }

        /// <summary>
        /// Updates the category news
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void UpdateCategory(CategoryNews category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            //validate category hierarchy
            var parentCategory = GetCategoryById(category.ParentCategoryNewsId);
            while (parentCategory != null)
            {
                if (category.Id == parentCategory.Id)
                {
                    category.ParentCategoryNewsId = 0;
                    break;
                }
                parentCategory = GetCategoryById(parentCategory.ParentCategoryNewsId);
            }

            _categoryRepository.Update(category);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern("categorynews");
            _cacheManager.RemoveByPattern(NEWSCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(category);
        }

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>A category news</returns>
        public virtual CategoryNews GetCategoryById(int categoryId)
        {
            if (categoryId == 0)
                return null;

            string key = string.Format(CATEGORIES_BY_ID_KEY, categoryId);
            return _cacheManager.Get(key, () =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
                {
                    var category = _categoryRepository.GetById(categoryId);
                    return category;
                }
            });
        }
        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>A category news</returns>
        public virtual async Task<CategoryNews> GetCategoryByIdAsync(int categoryId)
        {
            if (categoryId == 0)
                return null;
            return await System.Threading.Tasks.Task.Factory.StartNew<CategoryNews>(() =>
            {
                string key = string.Format(CATEGORIES_BY_ID_KEY, categoryId);
                return _cacheManager.Get(key, () =>
                {
                    using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }
                    ))
                    {
                        var category = _categoryRepository.GetById(categoryId);
                        return category;
                    }
                });
            });
        }
        /// <summary>
        /// Gets all categories news
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>List categories news</returns>
        public virtual IList<CategoryNews> GetAllCategories(bool showHidden = false)
        {
            return _cacheManager.Get(string.Format(CATEGORIES_ALL, showHidden), 30, () =>
            {
                return GetAllCategories(null, showHidden);
            });
        }
        /// <summary>
        /// Gets all categories news
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>List categories news</returns>
        public virtual async Task<IList<CategoryNews>> GetAllCategoriesAsync(bool showHidden = false)
        {
            return await _cacheManager.Get(string.Format(CATEGORIES_ALL, showHidden), 30, async() =>
            {
                return await GetAllCategoriesAsync(null, showHidden);
            });
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IList<CategoryNews> GetAllCategories(string categoryName, bool showHidden = false)
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
            {
                var query = _categoryRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Published);

                if (!String.IsNullOrWhiteSpace(categoryName))
                    query = query.Where(c => c.Name.Contains(categoryName));

                query = query.Where(c => !c.Deleted);

                query = query.OrderBy(c => c.ParentCategoryNewsId).ThenBy(c => c.DisplayOrder);

                var unsortedCategories = query.ToList();

                //sort categories
                var sortedCategories = unsortedCategories;//.SortCategoriesForTree();
                return sortedCategories;
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual async Task<IList<CategoryNews>> GetAllCategoriesAsync(string categoryName, bool showHidden = false)
        {
            return await System.Threading.Tasks.Task.Factory.StartNew<IList<CategoryNews>>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    var query = _categoryRepository.Table;
                    if (!showHidden)
                        query = query.Where(c => c.Published);

                    if (!String.IsNullOrWhiteSpace(categoryName))
                        query = query.Where(c => c.Name.Contains(categoryName));

                    query = query.Where(c => !c.Deleted);

                    query = query.OrderBy(c => c.ParentCategoryNewsId).ThenBy(c => c.DisplayOrder);

                    var unsortedCategories = query.ToList();

                    //sort categories
                    var sortedCategories = unsortedCategories;//.SortCategoriesForTree();
                    return sortedCategories;
                }
            });
        }
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IPagedList<CategoryNews> GetAllCategories(string categoryName,
            int pageIndex, int pageSize, bool showHidden = false)
        {
            var categories = GetAllCategories(categoryName, showHidden);
            //filter
            return new PagedList<CategoryNews>(categories, pageIndex, pageSize);
        }
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual async Task<IPagedList<CategoryNews>> GetAllCategoriesAsync(string categoryName,
            int pageIndex, int pageSize, bool showHidden = false)
        {
            var categories = await GetAllCategoriesAsync(categoryName, showHidden);
            //filter
            return new PagedList<CategoryNews>(categories, pageIndex, pageSize);
        }
        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category collection</returns>
        public IList<CategoryNews> GetAllCategoriesByParentCategoryId(int parentCategoryId, bool showHidden = false)
        {
            string key = string.Format(CATEGORIES_BY_PARENT_CATEGORY_ID_KEY, parentCategoryId, showHidden);
            return _cacheManager.Get(key, () =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
                {
                    var query = _categoryRepository.Table;
                    if (!showHidden)
                        query = query.Where(c => c.Published);
                    query = query.Where(c => c.ParentCategoryNewsId == parentCategoryId && !c.Deleted).OrderBy(x => x.DisplayOrder);

                    if (query == null)
                        return null;
                    return query.ToList();
                }
            });
        }
        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category collection</returns>
        public async Task<IList<CategoryNews>> GetAllCategoriesByParentCategoryIdAsync(int parentCategoryId, bool showHidden = false)
        {
            return await Task.Factory.StartNew<IList<CategoryNews>>(() =>
            {
                string key = string.Format(CATEGORIES_BY_PARENT_CATEGORY_ID_KEY, parentCategoryId, showHidden);
                return _cacheManager.Get(key, () =>
                {
                    using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }
                    ))
                    {
                        var query = _categoryRepository.Table;
                        if (!showHidden)
                            query = query.Where(c => c.Published);
                        query = query.Where(c => c.ParentCategoryNewsId == parentCategoryId && !c.Deleted).OrderBy(x => x.DisplayOrder);

                        if (query == null)
                            return null;
                        return query.ToList();
                    }
                });
            }).ConfigureAwait(false);
        }
        /// <summary>
        /// Inserts category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void InsertCategory(CategoryNews category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            _categoryRepository.Insert(category);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(NEWSCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(category);
        }

        /// <summary>
        /// Deletes a news category mapping
        /// </summary>
        /// <param name="newsCategory">News category</param>
        public virtual void DeleteNewsCategory(NewsCategoryNews newsCategory)
        {
            if (newsCategory == null)
                throw new ArgumentNullException("NewsCategoryMap");

            _newsCategoryRepository.Delete(newsCategory);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern("categorynews");
            _cacheManager.RemoveByPattern(NEWSCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(newsCategory);
        }

        /// <summary>
        /// Gets news category mapping collection
        /// </summary>
        /// <param name="newsId">News identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News a category mapping collection</returns>
        public virtual IPagedList<NewsCategoryNews> GetNewsCategoriesByCategoryId(int categoryId, int pageIndex, int pageSize, bool showHidden = false)
        {
            if (categoryId == 0)
                return new PagedList<NewsCategoryNews>(new List<NewsCategoryNews>(), pageIndex, pageSize);

            string key = string.Format(NEWSCATEGORIES_ALLBYCATEGORYID_KEY, showHidden, categoryId, pageIndex, pageSize);
            return _cacheManager.Get(key, () =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
                {
                    var query = from pc in _newsCategoryRepository.Table
                                join p in _newsRepository.Table on pc.NewsId equals p.Id
                                where pc.CategoryNewsId == categoryId &&
                                      (showHidden || p.Published)
                                select pc;
                    var newsCategories = new PagedList<NewsCategoryNews>(query, pageIndex, pageSize);
                    return newsCategories;
                }
            });
        }
        /// <summary>
        /// Gets news category mapping collection
        /// </summary>
        /// <param name="newsId">News identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News a category mapping collection</returns>
        public virtual async Task<IPagedList<NewsCategoryNews>> GetNewsCategoriesByCategoryIdAsync(int categoryId, int pageIndex, int pageSize, bool showHidden = false)
        {
            if (categoryId == 0)
                return new PagedList<NewsCategoryNews>(new List<NewsCategoryNews>(), pageIndex, pageSize);
            return await Task.Factory.StartNew<IPagedList<NewsCategoryNews>>(() =>
            {
                string key = string.Format(NEWSCATEGORIES_ALLBYCATEGORYID_KEY, showHidden, categoryId, pageIndex, pageSize);
                return _cacheManager.Get(key, () =>
                {
                    using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }
                    ))
                    {
                        var query = from pc in _newsCategoryRepository.Table
                                    join p in _newsRepository.Table on pc.NewsId equals p.Id
                                    where pc.CategoryNewsId == categoryId &&
                                          (showHidden || p.Published)
                                    select pc;
                        var newsCategories = new PagedList<NewsCategoryNews>(query, pageIndex, pageSize);
                        return newsCategories;
                    }
                });
            });
        }

        /// <summary>
        /// Gets a news category mapping collection
        /// </summary>
        /// <param name="NewsId">News identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News category mapping collection</returns>
        public virtual IList<NewsCategoryNews> GetNewsCategoriesByNewsId(int newsId, bool showHidden = false)
        {
            if (newsId == 0)
                return new List<NewsCategoryNews>();

            string key = string.Format(NEWSCATEGORIES_ALLBYNEWSID_KEY, showHidden, newsId);
            return _cacheManager.Get(key, () =>
            {
                var query = from pc in _newsCategoryRepository.Table
                            join c in _newsRepository.Table on pc.NewsId equals c.Id
                            where pc.NewsId == newsId &&
                                  (showHidden || c.Published)
                            select pc;

                if (query == null)
                    return null;
                return query.ToList();
            });
        }
        /// <summary>
        /// Gets a news category mapping collection
        /// </summary>
        /// <param name="NewsId">News identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News category mapping collection</returns>
        public virtual async Task<IList<NewsCategoryNews>> GetNewsCategoriesByNewsIdAsync(int newsId, bool showHidden = false)
        {
            if (newsId == 0)
                return new List<NewsCategoryNews>();
            return await Task.Factory.StartNew<IList<NewsCategoryNews>>(() =>
            {
                string key = string.Format(NEWSCATEGORIES_ALLBYNEWSID_KEY, showHidden, newsId);
                return _cacheManager.Get(key, () =>
                {
                    var query = from pc in _newsCategoryRepository.Table
                                join c in _newsRepository.Table on pc.NewsId equals c.Id
                                where pc.NewsId == newsId &&
                                      (showHidden || c.Published)
                                select pc;

                    if (query == null)
                        return null;
                    return query.ToList();
                });
            });
        }

        /// <summary>
        /// Gets a News category mapping 
        /// </summary>
        /// <param name="newsCategoryId">news category mapping identifier</param>
        /// <returns>news category mapping</returns>
        public virtual NewsCategoryNews GetNewsCategoryById(int newsCategoryId)
        {
            if (newsCategoryId == 0)
                return null;

            string key = string.Format(NEWSCATEGORIES_BY_ID_KEY, newsCategoryId);
            return _cacheManager.Get(key, () =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }
                ))
                {
                    return _newsCategoryRepository.GetById(newsCategoryId);
                }
            });
        }

        /// <summary>
        /// Gets a News category mapping 
        /// </summary>
        /// <param name="newsCategoryId">news category mapping identifier</param>
        /// <returns>news category mapping</returns>
        public virtual async Task<NewsCategoryNews> GetNewsCategoryByIdAsync(int newsCategoryId)
        {
            if (newsCategoryId == 0)
                return null;
            return await Task.Factory.StartNew<NewsCategoryNews>(() =>
            {
                string key = string.Format(NEWSCATEGORIES_BY_ID_KEY, newsCategoryId);
                return _cacheManager.Get(key, () =>
                {
                    using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }
                    ))
                    {
                        return _newsCategoryRepository.GetById(newsCategoryId);
                    }
                });
            });
        }
        /// <summary>
        /// Inserts a News category mapping
        /// </summary>
        /// <param name="newsCategory">>News category mapping</param>
        public virtual void InsertNewsCategory(NewsCategoryNews newsCategory)
        {
            if (newsCategory == null)
                throw new ArgumentNullException("newsCategoryMap");

            _newsCategoryRepository.Insert(newsCategory);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern("categorynews");
            _cacheManager.RemoveByPattern(NEWSCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(newsCategory);
        }

        /// <summary>
        /// Updates the news category mapping 
        /// </summary>
        /// <param name="newsCategory">>news category mapping</param>
        public virtual void UpdateNewsCategory(NewsCategoryNews newsCategory)
        {
            if (newsCategory == null)
                throw new ArgumentNullException("newsCategoryMap");

            _newsCategoryRepository.Update(newsCategory);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern("categorynews");
            _cacheManager.RemoveByPattern(NEWSCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(newsCategory);
        }

        #endregion

    }

}
