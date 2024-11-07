using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace PMS.Common
{
    public static class EntityValidationConstants
    {
        public const string PMSRequiredDateFormat = "dd-MM-yyyy";
        public const string PMSRequiredDateTimeFormat = "dd-MM-yyyy HH:mm";
        public const int UserNameMaxLength = 50;
        public const int UserNameMinLength = 3;
        public static readonly string[] PMSPositions = { "Manager", "Technician", "Supervisor", "Engineer", "Operator" };

        public static class EquipmentConstants
        {
            public const int EquipmentNameMaxLength = 50;
            public const int EquipmentNameMinLength = 2;
            public const int EquipmentDescriptionMaxLength = 150;
            public const int EquipmentDescriptionMinLength = 5;
            public const int EquipmentMakerIdMaxLength = 30;
            public const int EquipmentMakerIdMinLength = 1;
            public const int EquipmentCreatorIdMaxLength = 30;
            public const string EquipmentRequiredDateFromat = PMSRequiredDateFormat;
           
        }

        public static class ConsumableConstants
        {
            public const int ConsumableNameMaxLength = 50;
            public const int ConsumableNameMinLength = 2;
            public const int ConsumableDescriptionMaxLength = 150;
            public const int ConsumableDescriptionMinLength = 5;
            public const int ConsumableCreatorIdMaxLength = 30;
            public const int ConsumableUnitStringMaxLength = 20;
            public const int ConsumableUnitStringMinLength = 1;
            public const int ConsumableROBMaxValue = 5000000;

        }

        public static class SupplierConstatnts
        {
            public const int SupplierNameMaxLength = 60;
            public const int SupplierNameMinLength = 3;
            public const int SupplierAddressMaxLength = 90;
            public const int SupplierAddressMinLength = 5;
            public const int SupplierCreatorIdMaxLength = 30;
            public const int SupplierEmailMaxLength = 100;
            public const int SupplierEmailMinLength = 7;
            public const int SupplierPhoneMaxLength = 20;
            public const int SupplierPhoneMinLength = 6;
            public const int SupplierIdStringMaxLength = 30;
        }

        public static class SparePartConstants
        {
            public const int SparePartNameMaxLength = 100;
            public const int SparePartNameMinLength = 3;
            public const int SparePartDescriptionMaxLength = 90;
            public const int SparePartDescriptionMinLength = 5;
            public const int SparePartCreatorIdMaxLength = 30;
            public const int SparePartUnitsMaxLength = 20;
            public const int SparePartUnitsMinLength = 1;
            public const int SparePartIdStringMaxLength = 30;
            public const int SparePartROBMaxValue = 5000000;
            public const int SparePartPriceMaxValue = 5000000;
            public const int SparePartImageURLMaxLength = 500;

        }

        public static class MakerConstatnts
        {
            public const int MakerNameMaxLength = 100;
            public const int MakerNameMinLength = 3;
            public const int MakerDescriptionMaxLength = 90;
            public const int MakerDescriptionMinLength = 5;
            public const int MakerEmailMaxLength = 100;
            public const int MakerEmailMinLength = 7;
            public const int MakerPhoneMaxLength = 20;
            public const int MakerPhoneMinLength = 6;
            public const int MakerCreatorIdStringMaxLength = 30;
        }

        public static class ManualClassConstants
        {
            public const int ManualNameMaxLength = 150;
            public const int ManualNameMinLength = 3;
            public const int ManualIdStringMaxLength = 30;
            public const int ManualURLMaxLength = 250;
            public const int ManualURLMinLength = 4;

        }

        public class MaintenanceConstants
        {
            public const int MaintenanceIdStringMaxLength = 30;
            public const int MaintenanceNameMaxLength = 150;
            public const int MaintenanceNameMinLength = 3;
            public const int MaintenancePositionMaxLength = 150;
            public const int MaintenancePositionMinLength = 3;
            public const int MaintenanceDescriptionMaxLength = 90;
            public const int MaintenanceDescriptionMinLength = 5;


        }
    }
}
