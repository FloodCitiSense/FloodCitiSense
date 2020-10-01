namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class GetFilterLocationForView
    {
        public LocationDto Location { get; set; }
        public bool IsCurrentUserLocation => CreatedUserUserId == LoggedInUserId;

        public long? CreatedUserUserId { get; set; }
        public long? LoggedInUserId { get; set; }

        public GetFilterLocationForView()
        {

        }
    }
}