using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Services.Events;
using System.Threading.Tasks;
using System.Transactions;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Product template service
    /// </summary>
    public partial class ProductTemplateService : IProductTemplateService
    {
        #region Fields

        private readonly IRepository<ProductTemplate> _productTemplateRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion
        
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="productTemplateRepository">Product template repository</param>
        /// <param name="eventPublisher">Event published</param>
        public ProductTemplateService(IRepository<ProductTemplate> productTemplateRepository,
            IEventPublisher eventPublisher)
        {
            _productTemplateRepository = productTemplateRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void DeleteProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException("productTemplate");

            _productTemplateRepository.Delete(productTemplate);

            //event notification
            _eventPublisher.EntityDeleted(productTemplate);
        }

        /// <summary>
        /// Gets all product templates
        /// </summary>
        /// <returns>Product templates</returns>
        public virtual IList<ProductTemplate> GetAllProductTemplates()
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
                ))
            {
                var query = from pt in _productTemplateRepository.Table
                            orderby pt.DisplayOrder
                            select pt;

                var templates = query.ToList();
                return templates;
            }
        }
        public virtual async Task<IList<ProductTemplate>> GetAllProductTemplatesAsync()
        {
            return await Task.Factory.StartNew<IList<ProductTemplate>>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    var query = from pt in _productTemplateRepository.Table
                                orderby pt.DisplayOrder
                                select pt;

                    var templates = query.ToList();
                    return templates;
                }
            });
        }

        /// <summary>
        /// Gets a product template
        /// </summary>
        /// <param name="productTemplateId">Product template identifier</param>
        /// <returns>Product template</returns>
        public virtual ProductTemplate GetProductTemplateById(int productTemplateId)
        {
            if (productTemplateId == 0)
                return null;
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
                ))
            {
                return _productTemplateRepository.GetById(productTemplateId);
            }
        }
        public virtual async Task<ProductTemplate> GetProductTemplateByIdAsync(int productTemplateId)
        {
            if (productTemplateId == 0)
                return null;
            return await Task.Factory.StartNew<ProductTemplate>(() =>
            {
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }
                    ))
                {
                    return _productTemplateRepository.GetById(productTemplateId);
                }
            });
        }

        /// <summary>
        /// Inserts product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void InsertProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException("productTemplate");

            _productTemplateRepository.Insert(productTemplate);

            //event notification
            _eventPublisher.EntityInserted(productTemplate);
        }

        /// <summary>
        /// Updates the product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void UpdateProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException("productTemplate");

            _productTemplateRepository.Update(productTemplate);

            //event notification
            _eventPublisher.EntityUpdated(productTemplate);
        }
        
        #endregion
    }
}
