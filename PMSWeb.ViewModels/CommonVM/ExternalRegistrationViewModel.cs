using static PMS.Common.EntityValidationConstants;


namespace PMSWeb.ViewModels.CommonVM
{
    public class ExternalRegistrationViewModel
    {
        public string? Email { get; set; }
        public string? Position { get; set; }
        public string? UserName { get; set; }

        public List<Position> PositionOptions { get; set; } = new List<Position>()
        {
                    new(){ Key = PMSPositions[0] , Value = PMSPositions[0]},
                    new(){ Key = PMSPositions[1] , Value = PMSPositions[1]},
                    new(){ Key = PMSPositions[2] , Value = PMSPositions[2]},
        };

    }

    public class Position
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }

}

