using System.Collections.Generic;
using Nop.Core.Domain.Directory;
using System.Threading.Tasks;

namespace Nop.Services.Directory
{
    /// <summary>
    /// State province service interface
    /// </summary>
    public partial interface IStateProvinceService
    {
        /// <summary>
        /// Deletes a state/province
        /// </summary>
        /// <param name="stateProvince">The state/province</param>
        void DeleteStateProvince(StateProvince stateProvince);

        /// <summary>
        /// Gets a state/province
        /// </summary>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <returns>State/province</returns>
        StateProvince GetStateProvinceById(int stateProvinceId);
        Task<StateProvince> GetStateProvinceByIdAsync(int stateProvinceId);
        /// <summary>
        /// Gets a state/province 
        /// </summary>
        /// <param name="abbreviation">The state/province abbreviation</param>
        /// <returns>State/province</returns>
        StateProvince GetStateProvinceByAbbreviation(string abbreviation);
        Task<StateProvince> GetStateProvinceByAbbreviationAsync(string abbreviation);
        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>State/province collection</returns>
        IList<StateProvince> GetStateProvincesByCountryId(int countryId, bool showHidden = false);
        Task<IList<StateProvince>> GetStateProvincesByCountryIdAsync(int countryId, bool showHidden = false);
        /// <summary>
        /// Inserts a state/province
        /// </summary>
        /// <param name="stateProvince">State/province</param>
        void InsertStateProvince(StateProvince stateProvince);

        /// <summary>
        /// Updates a state/province
        /// </summary>
        /// <param name="stateProvince">State/province</param>
        void UpdateStateProvince(StateProvince stateProvince);

        IList<District> GetDistHCM(int stateId = 23, bool showHidden = false);
        Task<IList<District>> GetDistrictByStateProvinceIdAsync(int stateId = 23, bool showHidden = false);
        District GetDistrictById(int districtId);
        Task<District> GetDistrictByIdAsync(int districtId);

        void InsertDistrict(District district);

        void UpdateDistrict(District district);

        void DeleteDistrict(District district);

        Ward GetWardById(int wardId);
        Task<Ward> GetWardByIdAsync(int wardId);
        IList<Ward> GetWardByDistrictId(int districtId);
        Task<IList<Ward>> GetWardByDistrictIdAsync(int districtId);

        Street GetStreetById(int streetId);
        Task<Street> GetStreetByIdAsync(int streetId);
        IList<Street> GetStreetByDistrictId(int districtId);
        Task<IList<Street>> GetStreetByDistrictIdAsync(int districtId);
    }
}
