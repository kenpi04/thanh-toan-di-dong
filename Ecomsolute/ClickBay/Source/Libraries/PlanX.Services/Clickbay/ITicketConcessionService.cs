using System.Collections.Generic;
using PlanX.Core;
using PlanX.Core.Domain.ClickBay;

namespace PlanX.Services.ClickBay
{
    /// <summary>
    /// TicketConcession service interface
    /// </summary>
    public partial interface ITicketConcessionService
    {

        void DeleteTicketConcession(TicketConcession TicketConcession);

        TicketConcession GetTicketConcessionById(int TicketConcessionId);
        
        IPagedList<TicketConcession> GetAllTicketConcession(int pageIndex, int pageSize);

        void InsertTicketConcession(TicketConcession TicketConcession);

        void UpdateTicketConcession(TicketConcession TicketConcession);

        void DeleteTicketType(TicketType TicketType);

        TicketType GetTicketTypeById(int TicketTypeId);

         List<string> GetAllTicketType();

        void InsertTicketType(TicketType TicketType);

        void UpdateTicketType(TicketType TicketType);

        void DeletePlace(Place Place);

        Place GetPlaceById(int PlaceId);

        List<string> GetAllPlace();

        void InsertPlace(Place Place);

        void UpdatePlace(Place Place);

        IPagedList<TicketConcession> SearchTicketConcession(int pageIndex, int pageSize, string PassengerNameSearch = "", string FromPlaceSearch = "", string ToPlaceSearch = "", string TicketTypeSearch = "", string DepartDateSearch = "");

    }
}
