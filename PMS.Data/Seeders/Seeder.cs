using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Data.Seeders
{
    public class Seeder()
    {

        public List<Consumable> GetConsumables(string adminId)
        {
                List<Consumable> Consumables = new List<Consumable>() 
                {
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Fuel",
                        Units = "Liters",
                        Description = "Diesel fuel for generators",
                        CreatorId = adminId,
                        Price = 1.25m,
                        ROB = 1000.0,
                        IsDeleted = false,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Oil",
                        Units = "Liters",
                        Description = "Engine oil for machines",
                        CreatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        Price = 15.50m,
                        ROB = 500.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Grease",
                        Units = "Kg",
                        Description = "Grease for greaseng elements",
                        CreatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        Price = 7.75m,
                        ROB = 200.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Rubber sheet",
                        Units = "Pcs",
                        Description = "Rubber sheet for gaskets",
                        CreatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        Price = 20.95m,
                        ROB = 20.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "DestilateWater",
                        Units = "Liters",
                        Description = "Portable water for special use",
                        CreatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        Price = 0.10m,
                        ROB = 10000.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Glue",
                        Units = "Litres",
                        Description = "Glue fast drying for securing threads",
                        CreatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        Price = 80.00m,
                        ROB = 100.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Nuts",
                        Units = "Kg",
                        Description = "Nuts, variety of sizes",
                        CreatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        Price = 10.00m,
                        ROB = 300.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Bolts Stainless",
                        Units = "Kg",
                        Description = "Bolts stainless general use",
                        CreatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        Price = 7.50m,
                        ROB = 450.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Rags White Sterilized",
                        Units = "Bags",
                        Description = "Cleaning material for common use",
                        CreatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        Price = 50.00m,
                        ROB = 20.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Paint",
                        Units = "Liters",
                        Description = "Exterior paint for finishing",
                        CreatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        Price = 25.00m,
                        ROB = 300.0,
                        IsDeleted = false
                    }
                };
            
             return Consumables;
            
        }

        public List<Maker> GetMakers(string adminId)
        {
            List<Maker> Makers = new()
            {
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "John Deere",
                    Description = "Manufacturer of agricultural machinery and heavy equipment.",
                    Email = "contact@johndeere.com",
                    Phone = "+1-800-1234567",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Caterpillar",
                    Description = "Leading manufacturer of construction and mining equipment.",
                    Email = "info@caterpillar.com",
                    Phone = "+1-800-7654321",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Komatsu",
                    Description = "Global supplier of earth-moving equipment and mining machinery.",
                    Email = "support@komatsu.com",
                    Phone = "+1-800-9876543",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Hitachi",
                    Description = "Manufactures construction and industrial equipment.",
                    Email = "contact@hitachi.com",
                    Phone = "+1-800-8765432",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Volvo",
                    Description = "Known for manufacturing trucks, buses, and construction equipment.",
                    Email = "info@volvo.com",
                    Phone = "+1-800-6543210",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Liebherr",
                    Description = "Provides cranes, trucks, and earthmoving equipment.",
                    Email = "contact@liebherr.com",
                    Phone = "+1-800-5432109",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "JCB",
                    Description = "Produces equipment for construction, agriculture, and waste handling.",
                    Email = "support@jcb.com",
                    Phone = "+1-800-4321098",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Bobcat",
                    Description = "Manufacturer of construction and landscaping equipment.",
                    Email = "help@bobcat.com",
                    Phone = "+1-800-3210987",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Kubota",
                    Description = "Japanese manufacturer of tractors and heavy equipment.",
                    Email = "contact@kubota.com",
                    Phone = "+1-800-2109876",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Maker
                {
                    MakerId = Guid.NewGuid(),
                    MakerName = "Terex",
                    Description = "Provides lifting and material-handling solutions.",
                    Email = "info@terex.com",
                    Phone = "+1-800-1098765",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDeleted = false
                }
            };

            return Makers;
    
        }

        public List<City> GetCities()
        {
            List<City> Cities = new List<City>() {
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Sofia"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Varna"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Pusan"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Shanghai"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Boston"},
                new City(){ CityId = Guid.NewGuid(),CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow, Name = "Paris"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Mestre"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Genova"},
                new City(){ CityId = Guid.NewGuid(),CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow, Name = "Hamburg"},
                new City(){ CityId = Guid.NewGuid(),CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow, Name = "Vienna"}
            };

            return Cities;

        }

        public List<Country> GetCountries()
        {
            List<Country> Countries = new List<Country>() {
                new Country(){ CountryId = Guid.NewGuid(),CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Bulgaria"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "China"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Britain"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Germany"},
                new Country(){ CountryId = Guid.NewGuid(),CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow, Name = "S.Korea"},
                new Country(){ CountryId = Guid.NewGuid(),CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow, Name = "Singapore"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Poland"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "France"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Italy"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,Name = "Spain"},
            };

            return Countries;   
        }

        public List<Supplier> GetSuppliers(List<string> cityIds, List<string> countryIds,  string adminId)
        {
            List<Supplier> suppliers = new List<Supplier>() {
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "ABC Machinery Co.",
                    Address = "123 Industrial Park",
                    Email = "contact@abcmachinery.com",
                    PhoneNumber = "+1-555-1234567",
                    CityId = Guid.Parse(cityIds[0]), 
                    CountryId = Guid.Parse(countryIds[0]), 
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                },
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "Global Equipments Ltd.",
                    Address = "456 Main Street",
                    Email = "sales@globalequip.com",
                    PhoneNumber = "+1-555-7654321",
                    CityId = Guid.Parse(cityIds[1]),
                    CountryId = Guid.Parse(countryIds[1]),
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                },
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "Precision Tools Inc.",
                    Address = "789 Innovation Drive",
                    Email = "info@precisiontools.com",
                    PhoneNumber = "+1-555-2345678",
                   CityId = Guid.Parse(cityIds[2]),
                    CountryId = Guid.Parse(countryIds[2]),
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                },
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "Green Tech Supplies",
                    Address = "202 Eco Street",
                    Email = "info@greentech.com",
                    PhoneNumber = "+1-555-6543210",
                  CityId = Guid.Parse(cityIds[3]),
                    CountryId = Guid.Parse(countryIds[3]),
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                },
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "TopGear Industrial",
                    Address = "321 Equipment Lane",
                    Email = "contact@topgear.com",
                    PhoneNumber = "+1-555-3210987",
                    CityId = Guid.Parse(cityIds[4]),
                    CountryId = Guid.Parse(countryIds[4]),
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                },
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "Pinnacle Manufacturing",
                    Address = "789 Fabrication Rd",
                    Email = "info@pinnaclemfg.com",
                    PhoneNumber = "+1-555-8765432",
                   CityId = Guid.Parse(cityIds[5]),
                    CountryId = Guid.Parse(countryIds[5]),
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                },
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "Alpha Solutions",
                    Address = "101 Tech Lane",
                    Email = "support@alphasolutions.com",
                    PhoneNumber = "+1-555-4321098",
                   CityId = Guid.Parse(cityIds[6]),
                    CountryId = Guid.Parse(countryIds[6]),
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                },
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "Mega Industrial Supplies",
                    Address = "303 Supplies Avenue",
                    Email = "info@megaisupplies.com",
                    PhoneNumber = "+1-555-5432109",
                   CityId = Guid.Parse(cityIds[7]),
                    CountryId = Guid.Parse(countryIds[7]),
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                },
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "Industrial Parts Corp.",
                    Address = "909 Component St",
                    Email = "contact@industrialparts.com",
                    PhoneNumber = "+1-555-6543210",
                   CityId = Guid.Parse(cityIds[8]),
                    CountryId = Guid.Parse(countryIds[8]),
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                },
                new Supplier
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "Heavy Duty Solutions",
                    Address = "101 Heavy Load Blvd",
                    Email = "support@heavydutysolutions.com",
                    PhoneNumber = "+1-555-8765432",
                    CityId = Guid.Parse(cityIds[9]),
                    CountryId = Guid.Parse(countryIds[9]),
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    IsDleted = false
                }
            };

            return suppliers;

        }

        public List<Equipment> GetEquipments(List<string> makersIds, string adminId)
        {
            List<Equipment> equipments = new List<Equipment>() {
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Hydraulic Press",
                    Description = "A machine for applying hydraulic pressure to form or shape materials.",
                    CreatorId = adminId,
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    MakerId = Guid.Parse(makersIds[0]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "CNC Lathe",
                    Description = "A high-precision lathe used for machining metal and other materials.",
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[1]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Power Generator",
                    Description = "A machine driven by diesel motor used to generate electrical power",
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[2]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Laser Cutter",
                    Description = "A precision machine used to cut materials using a laser beam.",
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[3]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Industrial Robot Arm",
                    Description = "A robotic arm used in industrial automation for assembly, welding, and more.",
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[4]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Forklift",
                    Description = "A powered industrial vehicle used to lift and move materials over short distances.",
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[5]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Welding Machine",
                    Description = "A machine used to fuse materials together by applying heat and pressure.",
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[6]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Air Compressor",
                    Description = "A machine that converts power into potential energy stored in pressurized air.",
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[7]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Conveyor Belt",
                    Description = "A machine used for transporting materials from one place to another in a continuous flow.",
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[8]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Drill Press",
                    Description = "A machine used to drill holes in materials with precise control and power.",
                    CreatedOn = DateTime.UtcNow,
                    EditedOn = DateTime.UtcNow,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[9]),
                    IsDeleted = false
                }
            };

            return equipments;

        }

        public List<Sparepart> GetSpareparts(List<string> equipmentIds, string adminId)
        {
            List<Sparepart> spareparts = new List<Sparepart>() {
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "Hydraulic Pump",
                        Description = "A pump that generates hydraulic pressure for the system.",
                        ROB = 15,
                        Price = 450.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[0]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "CNC Lathe Motor",
                        Description = "High-torque motor for CNC lathe operations.",
                        ROB = 25,
                        Price = 320.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[1]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "FuelInjector",
                        Description = "Replacement Injector for Power Generator.",
                        ROB = 100,
                        Price = 15.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[2]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "Laser Cutter Lens",
                        Description = "Optical lens for high-precision laser cutting.",
                        ROB = 50,
                        Price = 120.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[3]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "Industrial Robot Arm Joint",
                        Description = "High-torque joint for industrial robot arm.",
                        ROB = 30,
                        Price = 900.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[4]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "Forklift Tires",
                        Description = "Heavy-duty tires for forklifts.",
                        ROB = 60,
                        Price = 200.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[5]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "Welding Machine Electrode",
                        Description = "Replacement electrodes for welding operations.",
                        ROB = 200,
                        Price = 5.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[6]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "Air Compressor Filter",
                        Description = "High-efficiency filter for air compressors.",
                        ROB = 150,
                        Price = 25.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[7]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "Conveyor Belt Motor",
                        Description = "High-torque motor for conveyor belt systems.",
                        ROB = 40,
                        Price = 600.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[8]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "Drill Press Chuck",
                        Description = "Replacement chuck for drill presses.",
                        ROB = 75,
                        Price = 50.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[9]),
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        CreatorId = adminId,
                        IsDeleted = false
                    }
            };

            return spareparts;
        }

        public List<RoutineMaintenance> GetRoutineMaintenances(string adminId)
        {
            var routineMaintenances = new List<RoutineMaintenance>
            { 
                    new RoutineMaintenance
                    {
                        Name = "Oil Change",
                        Description = "Changing the oil in machinery for optimal performance.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-90),
                        Interval = 90,
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Filter Replacement",
                        Description = "Replace air filters to prevent dust accumulation.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-60),
                        Interval = 60,
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Inspection of Safety Equipment",
                        Description = "Inspect all safety equipment for compliance.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-30),
                        Interval = 180,
                        ResponsiblePosition = "Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Lubrication of Machinery",
                        Description = "Lubricate machinery parts to prevent wear and tear.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-45),
                        Interval = 45,
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Battery Check",
                        Description = "Check and recharge batteries for uninterrupted power supply.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-10),
                        Interval = 30,
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Calibration of Sensors",
                        Description = "Calibrate sensors to maintain accurate measurements.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-180),
                        Interval = 365,
                        ResponsiblePosition = "Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "HVAC System Cleaning",
                        Description = "Clean and maintain HVAC systems for optimal performance.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-120),
                        Interval = 120,
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Fire Extinguisher Inspection",
                        Description = "Inspect fire extinguishers for readiness and recharge if needed.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-365),
                        Interval = 365,
                        ResponsiblePosition = "Manager",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Pump Maintenance",
                        Description = "Inspect and repair pumps to avoid breakdowns.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-180),
                        Interval = 180,
                        ResponsiblePosition = "Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Lighting System Check",
                        Description = "Check and replace faulty lighting to ensure workplace safety.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-15),
                        Interval = 30,
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    }
            };

            return routineMaintenances;
        }

        public List<SpecificMaintenance> GetSpecificMaintenances(List<String> eqIds, string adminId)
        {
                List<SpecificMaintenance> specificMaintenances = new List<SpecificMaintenance>  
                {
                    new SpecificMaintenance
                    {
                        Name = "Hydraulic Pump Check",
                        Description = "Routine check of hydraulic pump functionality.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-90),
                        Interval = 90,
                        EquipmentId = Guid.Parse(eqIds[0]),
                        ResponsiblePosition = "Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Cooling System Flush",
                        Description = "Flush and clean the cooling system to prevent overheating.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-60),
                        Interval = 180,
                        EquipmentId = Guid.Parse(eqIds[1]),
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Electrical Panel Inspection",
                        Description = "Inspect electrical panels and ensure connections are secure.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-30),
                        Interval = 365,
                        EquipmentId = Guid.Parse(eqIds[2]),
                        ResponsiblePosition = "Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Air Filter Replacement",
                        Description = "Replace air filters to improve air quality.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-45),
                        Interval = 30,
                        EquipmentId = Guid.Parse(eqIds[3]),
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Emergency Power Test",
                        Description = "Test emergency power systems to ensure backup readiness.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-180),
                        Interval = 180,
                        EquipmentId = Guid.Parse(eqIds[4]),
                        ResponsiblePosition = "Manager",
                        CReatorId = adminId,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Proximity Alarm System Test",
                        Description = "Run a test of the proximity alarm for proper functioning.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-365),
                        Interval = 365,
                        EquipmentId = Guid.Parse(eqIds[5]),
                        ResponsiblePosition = "Manager",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Cooling Fan Maintenance",
                        Description = "Check and clean cooling fans to ensure ventilation.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-15),
                        Interval = 60,
                        EquipmentId = Guid.Parse(eqIds[6]),
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Lubricating System Cleanout",
                        Description = "Remove any residues and clean the luricating system.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-120),
                        Interval = 180,
                        EquipmentId = Guid.Parse(eqIds[7]),
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "El.Motor Maintenance",
                        Description = "Regular maintenance of the Electric Motor.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-270),
                        Interval = 270,
                        EquipmentId = Guid.Parse(eqIds[8]),
                        ResponsiblePosition = "Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Machanical Interlock Inspection",
                        Description = "Inspection and maintenance of mechanical interlocks for safety.",
                        LastCompletedDate = DateTime.UtcNow.AddDays(-75),
                        Interval = 90,
                        EquipmentId = Guid.Parse(eqIds[9]),
                        ResponsiblePosition = "Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.UtcNow,
                        EditedOn = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };

            return specificMaintenances;
        }

        public List<Manual> GetManuals(string adminId, List<string> eqIds, List<string> makerIds)
        {
            var manualsList = new List<Manual>
                {
                    new Manual
                    {
                        ManualName = "User Guide and Spres List",
                        MakerId = Guid.Parse(makerIds[0]),
                        EquipmentId = Guid.Parse(eqIds[0]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddMonths(-6),
                        EditedOn = DateTime.Now.AddDays(-10),
                        ContentURL = "MANUAL1.pdf",
                        IsDeleted = false
                    },
                    new Manual
                    {
                        ManualName = "Maintenance Guide Y-1000",
                        MakerId = Guid.Parse(makerIds[1]),
                        EquipmentId = Guid.Parse(eqIds[1]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddMonths(-12),
                        EditedOn = DateTime.Now.AddDays(-3),
                        ContentURL = "MANUAL2.pdf",
                        IsDeleted = false
                    },
                    new Manual
                    {
                        ManualName = "Installation Manual Z200",
                        MakerId =  Guid.Parse(makerIds[2]),
                        EquipmentId = Guid.Parse(eqIds[2]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddYears(-1),
                        EditedOn = DateTime.Now,
                        ContentURL = "MANUAL3.pdf",
                        IsDeleted = false
                    },
                    new Manual
                    {
                        ManualName = "Operation And Service Manual X",
                        MakerId = Guid.Parse(makerIds[3]),
                        EquipmentId = Guid.Parse(eqIds[3]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddMonths(-2),
                        EditedOn = DateTime.Now.AddDays(-15),
                        ContentURL = "MANUAL4.pdf",
                        IsDeleted = false
                    },
                    new Manual
                    {
                        ManualName = "Service Manual 2023 Edition",
                        MakerId = Guid.Parse(makerIds[4]),
                        EquipmentId = Guid.Parse(eqIds[4]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddYears(-2),
                        EditedOn = DateTime.Now.AddMonths(-1),
                        ContentURL = "MANUAL5.pdf",
                        IsDeleted = false
                    },
                    new Manual
                    {
                        ManualName = "Quick Start Guide Q-Lite",
                        MakerId = Guid.Parse(makerIds[5]),
                        EquipmentId = Guid.Parse(eqIds[5]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddMonths(-5),
                        EditedOn = DateTime.Now.AddDays(-30),
                        ContentURL = "MANUAL6.pdf",
                        IsDeleted = false
                    },
                    new Manual
                    {
                        ManualName = "Troubleshooting Handbook Alpha",
                        MakerId = Guid.Parse(makerIds[6]),
                        EquipmentId = Guid.Parse(eqIds[6]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddMonths(-9),
                        EditedOn = DateTime.Now.AddDays(-7),
                        ContentURL = "MANUAL7.pdf",
                        IsDeleted = false
                    },
                    new Manual
                    {
                        ManualName = "Safety Instructions for Beta Series",
                        MakerId = Guid.Parse(makerIds[7]),
                        EquipmentId = Guid.Parse(eqIds[7]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddYears(-3),
                        EditedOn = DateTime.Now.AddMonths(-4),
                        ContentURL = "MANUAL8.pdf",
                        IsDeleted = false
                    },
                    new Manual
                    {
                        ManualName = "Guide to Operate All Systems",
                        MakerId = Guid.Parse(makerIds[8]),
                        EquipmentId = Guid.Parse(eqIds[8]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddYears(-1).AddMonths(-2),
                        EditedOn = DateTime.Now.AddDays(-25),
                        ContentURL = "MANUAL9.pdf",
                        IsDeleted = false
                    },
                    new Manual
                    {
                        ManualName = "Electric Motor Setup Guide",
                        MakerId = Guid.Parse(makerIds[9]),
                        EquipmentId = Guid.Parse(eqIds[9]),
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now.AddMonths(-8),
                        EditedOn = DateTime.Now.AddDays(-5),
                        ContentURL = "MANUAL10.pdf",
                        IsDeleted = false
                    }
            };
            return manualsList; 
        }

        public List<JobOrder> GetJobOrders()
        {
            ///
            /// JobId
            /// JobName
            /// JobDescription
            /// DueDate - utc
            /// LastDoneDate - utc
            /// Interval
            /// Type - Routine / Specific
            /// ResponsiblePosition - Manager/Engineer/Technician
            /// CreatorId
            /// EquipmentId
            /// MaintenanceId
            /// IsDeleted
            /// IsHistory
            /// CompletedBy - if completed / History
            ///
            return new List<JobOrder>();
        }
    }
}
