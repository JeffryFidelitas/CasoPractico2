

namespace CoreLibrary.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public int TotalEvents { get; set; }
        public int TotalActiveUsers { get; set; }
        public int TotalAttendeesThisMonth { get; set; }
        public List<EventPopularity> Top5PopularEvents { get; set; }
        public class EventPopularity
        {
            public string EventName { get; set; }
            public int AttendeeCount { get; set; }
        }
    }
}
