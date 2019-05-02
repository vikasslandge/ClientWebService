using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ClientWebService
{
    /// <summary>
    /// Summary description for ClientWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ClientWebService : System.Web.Services.WebService
    {
        ProvidersWS_DBEntities entities = new ProvidersWS_DBEntities();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public GetCityDetails_Result[] GetCityDetails()
        {
            return entities.GetCityDetails().ToArray();
        }
        

        [WebMethod]
        public GetRouteDetails_Result[] GetRouteDetails(int sourceId, int destinationId, DateTime dateOfJourney)
        {
            var result = entities.GetRouteDetails(sourceId, destinationId, dateOfJourney);



            return result.ToArray();

        }
        [WebMethod]
        public GetBookingStatus_Result[] GetBookingStatuses(int busId)
        {

            return entities.GetBookingStatus(busId).ToArray();
        }

        [WebMethod]
        public Boolean BookTicket(int seatNo, int busId)
        {
            var result = entities.BookSeat(seatNo, busId, "Book");


            if (result > 0)
            {
                return true;

            }
            return false;
        }
        [WebMethod]
        public Boolean AddTicketDetails(int routeId, int totalNumberOfSeats)
        {


            TicketDetail ticketDetail = new TicketDetail()
            {
                RouteId = routeId,
                TotalSelectedSeats = totalNumberOfSeats,
                Amount = totalNumberOfSeats * (entities.RouteDetails.Where(id => id.RouteId == routeId).Select(p => p.Price).Single())
            };



            var result2 = entities.TicketDetails.Add(ticketDetail);

            entities.SaveChanges();

            if (result2 != null)
            {
                return true;

            }
            return false;
        }

        [WebMethod]
        public Boolean AddPassanger(string name, int age, string gender, string number, int seatNo)
        {
            PassengerDetail passenger = new PassengerDetail()
            {
                Name = name,
                Age = age,
                Gender = gender,
                Phone = number,
                TicketId = entities.TicketDetails.Max(i => i.TicketId),
                SeatNo = seatNo
            };
            entities.PassengerDetails.Add(passenger);
            var result = entities.SaveChanges();
            if (result > 0)
            {
                return true;

            }
            return false;
        }
        [WebMethod]
        public GetPassangerDetails_Result[] GetPassengerDetails(int ticketId)
        {
            return entities.GetPassangerDetails(ticketId).ToArray();
        }
    }
}
