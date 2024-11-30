using static PMS.Common.EntityValidationConstants;

namespace PMS.Common
{
    public class Position
    {
        public string? Key { get; set; }
        public string? DisplayValue { get; set; }
        
        public static List<Position> GetPositions()
        {
            return new List<Position>
            {
                new Position { Key = PMSPositions[0], DisplayValue = PMSPositions[0] },
                new Position { Key = PMSPositions[1], DisplayValue = PMSPositions[1] },
                new Position { Key = PMSPositions[2], DisplayValue = PMSPositions[2] }
            };
        }
    }
}
