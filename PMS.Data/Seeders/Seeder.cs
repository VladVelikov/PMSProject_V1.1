using Microsoft.EntityFrameworkCore;
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Oil",
                        Units = "Liters",
                        Description = "Engine oil for machines",
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        Price = 15.50m,
                        ROB = 500.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Cement",
                        Units = "Bags",
                        Description = "Construction cement",
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        Price = 7.75m,
                        ROB = 200.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Steel",
                        Units = "Kilograms",
                        Description = "Reinforcement steel bars",
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        Price = 0.95m,
                        ROB = 1500.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Water",
                        Units = "Liters",
                        Description = "Portable water for construction use",
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        Price = 0.10m,
                        ROB = 10000.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Concrete",
                        Units = "Cubic Meters",
                        Description = "Ready mix concrete",
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        Price = 80.00m,
                        ROB = 100.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Gravel",
                        Units = "Tons",
                        Description = "Gravel for road construction",
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        Price = 10.00m,
                        ROB = 600.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Sand",
                        Units = "Cubic Meters",
                        Description = "Fine sand for concrete",
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        Price = 7.50m,
                        ROB = 450.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Bitumen",
                        Units = "Barrels",
                        Description = "Bitumen for asphalt works",
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        Price = 120.00m,
                        ROB = 50.0,
                        IsDeleted = false
                    },
                    new ()
                    {
                        ConsumableId = Guid.NewGuid(),
                        Name = "Paint",
                        Units = "Liters",
                        Description = "Exterior paint for finishing",
                        CreatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    IsDeleted = false
                }
            };

            return Makers;
    
        }

        public List<City> GetCities()
        {
            List<City> Cities = new List<City>() {
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Sofia"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Varna"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Pusan"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Shanghai"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Boston"},
                new City(){ CityId = Guid.NewGuid(),CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now, Name = "Paris"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Mestre"},
                new City(){ CityId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Genova"},
                new City(){ CityId = Guid.NewGuid(),CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now, Name = "Hamburg"},
                new City(){ CityId = Guid.NewGuid(),CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now, Name = "Vienna"}
            };

            return Cities;

        }

        public List<Country> GetCountries()
        {
            List<Country> Countries = new List<Country>() {
                new Country(){ CountryId = Guid.NewGuid(),CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Bulgaria"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "China"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Britain"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Germany"},
                new Country(){ CountryId = Guid.NewGuid(),CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now, Name = "S.Korea"},
                new Country(){ CountryId = Guid.NewGuid(),CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now, Name = "Singapore"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Poland"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "France"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Italy"},
                new Country(){ CountryId = Guid.NewGuid(), CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,Name = "Spain"},
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    MakerId = Guid.Parse(makersIds[0]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "CNC Lathe",
                    Description = "A high-precision lathe used for machining metal and other materials.",
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[1]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "3D Printer",
                    Description = "A machine for creating 3D objects by layering material based on a digital model.",
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[2]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Laser Cutter",
                    Description = "A precision machine used to cut materials using a laser beam.",
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[3]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Industrial Robot Arm",
                    Description = "A robotic arm used in industrial automation for assembly, welding, and more.",
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[4]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Forklift",
                    Description = "A powered industrial vehicle used to lift and move materials over short distances.",
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[5]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Welding Machine",
                    Description = "A machine used to fuse materials together by applying heat and pressure.",
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[6]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Air Compressor",
                    Description = "A machine that converts power into potential energy stored in pressurized air.",
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[7]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Conveyor Belt",
                    Description = "A machine used for transporting materials from one place to another in a continuous flow.",
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
                    CreatorId = adminId,
                    MakerId = Guid.Parse(makersIds[8]),
                    IsDeleted = false
                },
                new Equipment
                {
                    EquipmentId = Guid.NewGuid(),
                    Name = "Drill Press",
                    Description = "A machine used to drill holes in materials with precise control and power.",
                    CreatedOn = DateTime.Now,
                    EditedOn = DateTime.Now,
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        CreatorId = adminId,
                        IsDeleted = false
                    },
                    new Sparepart
                    {
                        SparepartId = Guid.NewGuid(),
                        SparepartName = "3D Printer Nozzle",
                        Description = "Replacement nozzle for precision 3D printing.",
                        ROB = 100,
                        Price = 15.00m,
                        Units = "pieces",
                        EquipmentId = Guid.Parse(equipmentIds[2]),
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        LastCompletedDate = DateTime.Now.AddDays(-90),
                        Interval = 90,
                        ResponsiblePosition = "Maintenance Supervisor",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Filter Replacement",
                        Description = "Replace air filters to prevent dust accumulation.",
                        LastCompletedDate = DateTime.Now.AddDays(-60),
                        Interval = 60,
                        ResponsiblePosition = "Facilities Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Inspection of Safety Equipment",
                        Description = "Inspect all safety equipment for compliance.",
                        LastCompletedDate = DateTime.Now.AddDays(-30),
                        Interval = 180,
                        ResponsiblePosition = "Safety Officer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Lubrication of Machinery",
                        Description = "Lubricate machinery parts to prevent wear and tear.",
                        LastCompletedDate = DateTime.Now.AddDays(-45),
                        Interval = 45,
                        ResponsiblePosition = "Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Battery Check",
                        Description = "Check and recharge batteries for uninterrupted power supply.",
                        LastCompletedDate = DateTime.Now.AddDays(-10),
                        Interval = 30,
                        ResponsiblePosition = "Electrical Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Calibration of Sensors",
                        Description = "Calibrate sensors to maintain accurate measurements.",
                        LastCompletedDate = DateTime.Now.AddDays(-180),
                        Interval = 365,
                        ResponsiblePosition = "Instrumentation Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "HVAC System Cleaning",
                        Description = "Clean and maintain HVAC systems for optimal performance.",
                        LastCompletedDate = DateTime.Now.AddDays(-120),
                        Interval = 120,
                        ResponsiblePosition = "HVAC Specialist",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Fire Extinguisher Inspection",
                        Description = "Inspect fire extinguishers for readiness and recharge if needed.",
                        LastCompletedDate = DateTime.Now.AddDays(-365),
                        Interval = 365,
                        ResponsiblePosition = "Fire Safety Officer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Pump Maintenance",
                        Description = "Inspect and repair pumps to avoid breakdowns.",
                        LastCompletedDate = DateTime.Now.AddDays(-180),
                        Interval = 180,
                        ResponsiblePosition = "Mechanical Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new RoutineMaintenance
                    {
                        Name = "Lighting System Check",
                        Description = "Check and replace faulty lighting to ensure workplace safety.",
                        LastCompletedDate = DateTime.Now.AddDays(-15),
                        Interval = 30,
                        ResponsiblePosition = "Electrician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
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
                        LastCompletedDate = DateTime.Now.AddDays(-90),
                        Interval = 90,
                        EquipmentId = Guid.Parse(eqIds[0]),
                        ResponsiblePosition = "Maintenance Supervisor",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Cooling System Flush",
                        Description = "Flush and clean the cooling system to prevent overheating.",
                        LastCompletedDate = DateTime.Now.AddDays(-60),
                        Interval = 180,
                        EquipmentId = Guid.Parse(eqIds[1]),
                        ResponsiblePosition = "HVAC Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Electrical Panel Inspection",
                        Description = "Inspect electrical panels and ensure connections are secure.",
                        LastCompletedDate = DateTime.Now.AddDays(-30),
                        Interval = 365,
                        EquipmentId = Guid.Parse(eqIds[2]),
                        ResponsiblePosition = "Electrical Engineer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Air Filter Replacement",
                        Description = "Replace air filters to improve air quality.",
                        LastCompletedDate = DateTime.Now.AddDays(-45),
                        Interval = 30,
                        EquipmentId = Guid.Parse(eqIds[3]),
                        ResponsiblePosition = "Facilities Manager",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Emergency Power Test",
                        Description = "Test emergency power systems to ensure backup readiness.",
                        LastCompletedDate = DateTime.Now.AddDays(-180),
                        Interval = 180,
                        EquipmentId = Guid.Parse(eqIds[4]),
                        ResponsiblePosition = "Power Systems Engineer",
                        CReatorId = adminId,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Fire Alarm System Test",
                        Description = "Run a test of the fire alarm system for compliance.",
                        LastCompletedDate = DateTime.Now.AddDays(-365),
                        Interval = 365,
                        EquipmentId = Guid.Parse(eqIds[5]),
                        ResponsiblePosition = "Safety Officer",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Exhaust Fan Maintenance",
                        Description = "Check and clean exhaust fans to ensure ventilation.",
                        LastCompletedDate = DateTime.Now.AddDays(-15),
                        Interval = 60,
                        EquipmentId = Guid.Parse(eqIds[6]),
                        ResponsiblePosition = "Ventilation Specialist",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Fuel System Cleanout",
                        Description = "Remove any residues and clean the fuel system.",
                        LastCompletedDate = DateTime.Now.AddDays(-120),
                        Interval = 180,
                        EquipmentId = Guid.Parse(eqIds[7]),
                        ResponsiblePosition = "Mechanical Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Generator Maintenance",
                        Description = "Regular maintenance of the standby generator.",
                        LastCompletedDate = DateTime.Now.AddDays(-270),
                        Interval = 270,
                        EquipmentId = Guid.Parse(eqIds[8]),
                        ResponsiblePosition = "Generator Technician",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    },
                    new SpecificMaintenance
                    {
                        Name = "Lift Inspection",
                        Description = "Routine inspection of building lifts for safety.",
                        LastCompletedDate = DateTime.Now.AddDays(-75),
                        Interval = 90,
                        EquipmentId = Guid.Parse(eqIds[9]),
                        ResponsiblePosition = "Lift Inspector",
                        CReatorId = adminId,
                        CreatedOn = DateTime.Now,
                        EditedOn = DateTime.Now,
                        IsDeleted = false
                    }
                };

            return specificMaintenances;
        }

    }
}
