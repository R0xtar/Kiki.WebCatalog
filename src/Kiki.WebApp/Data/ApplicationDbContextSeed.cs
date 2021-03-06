﻿namespace Kiki.WebApp.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ApplicationDbContextSeed
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationDbContextSeed(ApplicationDbContext context, 
                                        RoleManager<IdentityRole> roleManager, 
                                        UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            if ((await _context.Database.GetPendingMigrationsAsync().ConfigureAwait(false)).Any())
            {
                try
                {
                    await _context.Database.MigrateAsync().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    throw;
                    //TODO Log
                }
            }

            if (!await _context.Catalogs.AnyAsync().ConfigureAwait(false)) await SeedCatalogsAsync().ConfigureAwait(false);
            if (!await _context.DiscountRules.AnyAsync().ConfigureAwait(false)) await SeedDiscountRulesAsync().ConfigureAwait(false);
            if (!await _context.DiscountRules.AnyAsync(d => d.MarginGarage > 0).ConfigureAwait(false))
            {
                await UpdateDiscountRulesAsync().ConfigureAwait(false);
                await SeedRolesAsync().ConfigureAwait(false);
            }
        }

        private async Task SeedRolesAsync()
        {
            string[] roleNames = { "Admin", "Kiki", "Garage", "Client" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist) await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var user = await _userManager.FindByEmailAsync("info@kiki-pneus.ch");
            var user2 = await _userManager.FindByEmailAsync("info2@kiki-pneus.ch");

            await _userManager.AddToRoleAsync(user, "Kiki");
            await _userManager.AddToRoleAsync(user2, "Admin");
        }

        private async Task SeedCatalogsAsync()
        {
            var catalogs = Catalogs;
            try
            {
                await _context.Catalogs.AddRangeAsync(catalogs).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
                //TODO Log
            }
        }

        public static List<Catalog> Catalogs => new List<Catalog>
        {
            new Catalog
            {
                Name = "BF Goodrich",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "AA",
                ReferenceColumn = "V",
                EANColumn = "Z",
                DimensionColumn = "T",
                WidthColumn = "A",
                AspectRatioColumn = "B",
                DiameterColumn = "U",
                LoadIndexSpeedRatingColumn = "X",
                ProfilColumn = "W",
                Info1Column = "",
                Info2Column = "",
                StartLineNumber = 5,
                DiscountPercentage = 30,
                SizeFormat = 0,
                File = File.ReadAllBytes(@"..\..\docs\Prix été BF Goodrich.xlsx")
            },
            new Catalog
            {
                Name = "Bridgestone",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "O",
                ReferenceColumn = "",
                EANColumn = "",
                DimensionColumn = "B",
                WidthColumn = "C",
                AspectRatioColumn = "D",
                DiameterColumn = "E",
                LoadIndexSpeedRatingColumn = "F:G",
                ProfilColumn = "H",
                Info1Column = "I",
                Info2Column = "J",
                StartLineNumber = 11,
                DiscountPercentage = 52,
                SizeFormat = 0,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Bridgestone.xlsx")
            },
            new Catalog
            {
                Name = "Continental",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "K",
                ReferenceColumn = "",
                EANColumn = "A",
                DimensionColumn = "C",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "D",
                ProfilColumn = "F",
                Info1Column = "E",
                Info2Column = "",
                StartLineNumber = 5,
                DiscountPercentage = 56,
                SizeFormat = (SizeFormatEnum) 1,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Conti, Uni, Semp, Barum.xlsx")
            },
            new Catalog
            {
                Name = "Uniroyal",
                SheetIndex = 1,
                BrandColumn = "x",
                BasePriceColumn = "J",
                ReferenceColumn = "",
                EANColumn = "A",
                DimensionColumn = "B",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "C",
                ProfilColumn = "E",
                Info1Column = "D",
                Info2Column = "",
                StartLineNumber = 5,
                DiscountPercentage = 60,
                SizeFormat = (SizeFormatEnum) 1,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Conti, Uni, Semp, Barum.xlsx")
            },
            new Catalog
            {
                Name = "Semperit",
                SheetIndex = 2,
                BrandColumn = "x",
                BasePriceColumn = "K",
                ReferenceColumn = "",
                EANColumn = "A",
                DimensionColumn = "B",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "C",
                ProfilColumn = "F",
                Info1Column = "E",
                Info2Column = "",
                StartLineNumber = 5,
                DiscountPercentage = 60,
                SizeFormat = (SizeFormatEnum) 1,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Conti, Uni, Semp, Barum.xlsx")
            },
            new Catalog
            {
                Name = "Barum",
                SheetIndex = 3,
                BrandColumn = "x",
                BasePriceColumn = "K",
                ReferenceColumn = "",
                EANColumn = "A",
                DimensionColumn = "B",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "C",
                ProfilColumn = "F",
                Info1Column = "E",
                Info2Column = "",
                StartLineNumber = 5,
                DiscountPercentage = 60,
                SizeFormat = (SizeFormatEnum) 1,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Conti, Uni, Semp, Barum.xlsx")
            },
            new Catalog
            {
                Name = "Cooper",
                SheetIndex = 3,
                BrandColumn = "x",
                BasePriceColumn = "H",
                ReferenceColumn = "B",
                EANColumn = "",
                DimensionColumn = "C",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "D",
                ProfilColumn = "",
                Info1Column = "G",
                Info2Column = "",
                StartLineNumber = 18,
                DiscountPercentage = 52,
                SizeFormat = (SizeFormatEnum) 1,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Cooper.xlsx")
            },
            new Catalog
            {
                Name = "Firestone",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "M",
                ReferenceColumn = "",
                EANColumn = "",
                DimensionColumn = "A",
                WidthColumn = "B",
                AspectRatioColumn = "C",
                DiameterColumn = "D",
                LoadIndexSpeedRatingColumn = "F:G",
                ProfilColumn = "H",
                Info1Column = "I",
                Info2Column = "J",
                StartLineNumber = 11,
                DiscountPercentage = 52,
                SizeFormat = 0,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Firestone.xlsx")
            },
            new Catalog
            {
                Name = "Formula",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "D",
                ReferenceColumn = "A",
                EANColumn = "F",
                DimensionColumn = "C",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "",
                ProfilColumn = "",
                Info1Column = "G",
                Info2Column = "H",
                StartLineNumber = 2,
                DiscountPercentage = 66,
                SizeFormat = (SizeFormatEnum) 2,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Formula .xlsx")
            },
            new Catalog
            {
                Name = "Goodyear, Dunlop, Fulda, Sava",
                SheetIndex = 0,
                BrandColumn = "A",
                BasePriceColumn = "K",
                ReferenceColumn = "",
                EANColumn = "",
                DimensionColumn = "C",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "D:F",
                ProfilColumn = "G",
                Info1Column = "B",
                Info2Column = "",
                StartLineNumber = 2,
                DiscountPercentage = 46,
                SizeFormat = (SizeFormatEnum) 2,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Goodyear, Dunlop, Sava, Fulda.xlsx")
            },
            new Catalog
            {
                Name = "Kleber",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "AA",
                ReferenceColumn = "V",
                EANColumn = "Z",
                DimensionColumn = "T",
                WidthColumn = "A",
                AspectRatioColumn = "B",
                DiameterColumn = "U",
                LoadIndexSpeedRatingColumn = "X",
                ProfilColumn = "W",
                Info1Column = "",
                Info2Column = "",
                StartLineNumber = 5,
                DiscountPercentage = 30,
                SizeFormat = 0,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Kleber.xlsx")
            },
            new Catalog
            {
                Name = "Michelin",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "AA",
                ReferenceColumn = "V",
                EANColumn = "Z",
                DimensionColumn = "T",
                WidthColumn = "A",
                AspectRatioColumn = "B",
                DiameterColumn = "U",
                LoadIndexSpeedRatingColumn = "X",
                ProfilColumn = "W",
                Info1Column = "",
                Info2Column = "",
                StartLineNumber = 5,
                DiscountPercentage = 30,
                SizeFormat = 0,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Michelin .xlsx")
            },
            new Catalog
            {
                Name = "Nokian",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "G",
                ReferenceColumn = "",
                EANColumn = "H",
                DimensionColumn = "B",
                WidthColumn = "H",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "",
                ProfilColumn = "C",
                Info1Column = "",
                Info2Column = " ",
                StartLineNumber = 16,
                DiscountPercentage = 47,
                SizeFormat = (SizeFormatEnum) 2,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Nokian.xlsx")
            },
            new Catalog
            {
                Name = "Pirelli",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "D",
                ReferenceColumn = "A",
                EANColumn = "F",
                DimensionColumn = "C",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "",
                ProfilColumn = "",
                Info1Column = "G",
                Info2Column = "H",
                StartLineNumber = 2,
                DiscountPercentage = 55,
                SizeFormat = (SizeFormatEnum) 2,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Pirelli.xlsx")
            },
            new Catalog
            {
                Name = "Seiberling",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "I",
                ReferenceColumn = "",
                EANColumn = "",
                DimensionColumn = "A",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "B:C",
                ProfilColumn = "D",
                Info1Column = "E",
                Info2Column = "F",
                StartLineNumber = 11,
                DiscountPercentage = 48,
                SizeFormat = (SizeFormatEnum) 2,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Seiberling.xlsx")
            },
            new Catalog
            {
                Name = "Tigar",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "AA",
                ReferenceColumn = "V",
                EANColumn = "Z",
                DimensionColumn = "T",
                WidthColumn = "A",
                AspectRatioColumn = "B",
                DiameterColumn = "U",
                LoadIndexSpeedRatingColumn = "X",
                ProfilColumn = "W",
                Info1Column = "",
                Info2Column = "",
                StartLineNumber = 5,
                DiscountPercentage = 30,
                SizeFormat = 0,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Tigar TTC..xlsx")
            },
            new Catalog
            {
                Name = "Yokohama",
                SheetIndex = 0,
                BrandColumn = "x",
                BasePriceColumn = "G",
                ReferenceColumn = "",
                EANColumn = "H",
                DimensionColumn = "B",
                WidthColumn = "",
                AspectRatioColumn = "",
                DiameterColumn = "",
                LoadIndexSpeedRatingColumn = "C",
                ProfilColumn = "F",
                Info1Column = "",
                Info2Column = "",
                StartLineNumber = 12,
                DiscountPercentage = 64,
                SizeFormat = (SizeFormatEnum) 2,
                File = File.ReadAllBytes(@"..\..\docs\Prix été Yoko.xlsx")
            }
        };

        private async Task SeedDiscountRulesAsync()
        {
            try
            {
                await _context.DiscountRules.AddRangeAsync(Rules).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
                //Todo log
            }
        }

        private async Task UpdateDiscountRulesAsync()
        {
            try
            {
                _context.DiscountRules.RemoveRange(_context.DiscountRules.Select(d => new DiscountRule { Id = d.Id }));
                await _context.SaveChangesAsync().ConfigureAwait(false);
                await _context.DiscountRules.AddRangeAsync(Rules).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
                //Todo log
            }
        }

        public static List<DiscountRule> Rules => new List<DiscountRule>
        {

            new DiscountRule {MarginGarage = 10,  FromPrice = 0, ToPrice = 30, Size = 10, Margin = 30},
            new DiscountRule {MarginGarage = 10,  FromPrice = 30, ToPrice = 45, Size = 10, Margin = 32},
            new DiscountRule {MarginGarage = 12,  FromPrice = 45, ToPrice = 60, Size = 10, Margin = 35},
            new DiscountRule {MarginGarage = 12,  FromPrice = 60, ToPrice = 75, Size = 10, Margin = 37},
            new DiscountRule {MarginGarage = 15,  FromPrice = 75, ToPrice = 90, Size = 10, Margin = 39},
            new DiscountRule {MarginGarage = 15,  FromPrice = 90, ToPrice = 105, Size = 10, Margin = 41},
            new DiscountRule {MarginGarage = 15,  FromPrice = 105, ToPrice = 120, Size = 10, Margin = 43},
            new DiscountRule {MarginGarage = 15,  FromPrice = 120, ToPrice = 99999, Size = 10, Margin = 45},
            new DiscountRule {MarginGarage = 10,  FromPrice = 0, ToPrice = 30, Size = 11, Margin = 30},
            new DiscountRule {MarginGarage = 10,  FromPrice = 30, ToPrice = 45, Size = 11, Margin = 32},
            new DiscountRule {MarginGarage = 12,  FromPrice = 45, ToPrice = 60, Size = 11, Margin = 35},
            new DiscountRule {MarginGarage = 12,  FromPrice = 60, ToPrice = 75, Size = 11, Margin = 37},
            new DiscountRule {MarginGarage = 15,  FromPrice = 75, ToPrice = 90, Size = 11, Margin = 39},
            new DiscountRule {MarginGarage = 15,  FromPrice = 90, ToPrice = 105, Size = 11, Margin = 41},
            new DiscountRule {MarginGarage = 15,  FromPrice = 105, ToPrice = 120, Size = 11, Margin = 43},
            new DiscountRule {MarginGarage = 15,  FromPrice = 120, ToPrice = 99999, Size = 11, Margin = 45},
            new DiscountRule {MarginGarage = 10,  FromPrice = 0, ToPrice = 30, Size = 12, Margin = 30},
            new DiscountRule {MarginGarage = 10,  FromPrice = 30, ToPrice = 45, Size = 12, Margin = 32},
            new DiscountRule {MarginGarage = 12,  FromPrice = 45, ToPrice = 60, Size = 12, Margin = 35},
            new DiscountRule {MarginGarage = 12,  FromPrice = 60, ToPrice = 75, Size = 12, Margin = 37},
            new DiscountRule {MarginGarage = 15,  FromPrice = 75, ToPrice = 90, Size = 12, Margin = 39},
            new DiscountRule {MarginGarage = 15,  FromPrice = 90, ToPrice = 105, Size = 12, Margin = 41},
            new DiscountRule {MarginGarage = 15,  FromPrice = 105, ToPrice = 120, Size = 12, Margin = 43},
            new DiscountRule {MarginGarage = 15,  FromPrice = 120, ToPrice = 99999, Size = 12, Margin = 45},
            new DiscountRule {MarginGarage = 10,  FromPrice = 0, ToPrice = 30, Size = 13, Margin = 30},
            new DiscountRule {MarginGarage = 10,  FromPrice = 30, ToPrice = 45, Size = 13, Margin = 32},
            new DiscountRule {MarginGarage = 12,  FromPrice = 45, ToPrice = 60, Size = 13, Margin = 35},
            new DiscountRule {MarginGarage = 12,  FromPrice = 60, ToPrice = 75, Size = 13, Margin = 37},
            new DiscountRule {MarginGarage = 15,  FromPrice = 75, ToPrice = 90, Size = 13, Margin = 39},
            new DiscountRule {MarginGarage = 15,  FromPrice = 90, ToPrice = 105, Size = 13, Margin = 41},
            new DiscountRule {MarginGarage = 15,  FromPrice = 105, ToPrice = 120, Size = 13, Margin = 43},
            new DiscountRule {MarginGarage = 15,  FromPrice = 120, ToPrice = 99999, Size = 13, Margin = 45},
            new DiscountRule {MarginGarage = 10,  FromPrice = 0, ToPrice = 30, Size = 14, Margin = 30},
            new DiscountRule {MarginGarage = 10,  FromPrice = 30, ToPrice = 45, Size = 14, Margin = 32},
            new DiscountRule {MarginGarage = 12,  FromPrice = 45, ToPrice = 60, Size = 14, Margin = 35},
            new DiscountRule {MarginGarage = 12,  FromPrice = 60, ToPrice = 75, Size = 14, Margin = 37},
            new DiscountRule {MarginGarage = 12,  FromPrice = 75, ToPrice = 90, Size = 14, Margin = 39},
            new DiscountRule {MarginGarage = 13,  FromPrice = 90, ToPrice = 105, Size = 14, Margin = 41},
            new DiscountRule {MarginGarage = 13,  FromPrice = 105, ToPrice = 120, Size = 14, Margin = 43},
            new DiscountRule {MarginGarage = 15,  FromPrice = 120, ToPrice = 99999, Size = 14, Margin = 46},
            new DiscountRule {MarginGarage = 12,  FromPrice = 0, ToPrice = 30, Size = 15, Margin = 40},
            new DiscountRule {MarginGarage = 12,  FromPrice = 30, ToPrice = 45, Size = 15, Margin = 40},
            new DiscountRule {MarginGarage = 12,  FromPrice = 45, ToPrice = 60, Size = 15, Margin = 43},
            new DiscountRule {MarginGarage = 13,  FromPrice = 60, ToPrice = 75, Size = 15, Margin = 47},
            new DiscountRule {MarginGarage = 15,  FromPrice = 75, ToPrice = 90, Size = 15, Margin = 49},
            new DiscountRule {MarginGarage = 15,  FromPrice = 90, ToPrice = 105, Size = 15, Margin = 51},
            new DiscountRule {MarginGarage = 15,  FromPrice = 105, ToPrice = 120, Size = 15, Margin = 53},
            new DiscountRule {MarginGarage = 16,  FromPrice = 120, ToPrice = 135, Size = 15, Margin = 56},
            new DiscountRule {MarginGarage = 17,  FromPrice = 135, ToPrice = 99999, Size = 15, Margin = 59},
            new DiscountRule {MarginGarage = 10,  FromPrice = 0, ToPrice = 30, Size = 16, Margin = 40},
            new DiscountRule {MarginGarage = 10,  FromPrice = 30, ToPrice = 45, Size = 16, Margin = 43},
            new DiscountRule {MarginGarage = 10,  FromPrice = 45, ToPrice = 60, Size = 16, Margin = 45},
            new DiscountRule {MarginGarage = 13,  FromPrice = 60, ToPrice = 75, Size = 16, Margin = 50},
            new DiscountRule {MarginGarage = 15,  FromPrice = 75, ToPrice = 90, Size = 16, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 90, ToPrice = 105, Size = 16, Margin = 60},
            new DiscountRule {MarginGarage = 17,  FromPrice = 105, ToPrice = 120, Size = 16, Margin = 60},
            new DiscountRule {MarginGarage = 18,  FromPrice = 120, ToPrice = 135, Size = 16, Margin = 65},
            new DiscountRule {MarginGarage = 17,  FromPrice = 135, ToPrice = 150, Size = 16, Margin = 65},
            new DiscountRule {MarginGarage = 20,  FromPrice = 150, ToPrice = 165, Size = 16, Margin = 70},
            new DiscountRule {MarginGarage = 20,  FromPrice = 165, ToPrice = 180, Size = 16, Margin = 70},
            new DiscountRule {MarginGarage = 20,  FromPrice = 180, ToPrice = 99999, Size = 16, Margin = 75},
            new DiscountRule {MarginGarage = 12,  FromPrice = 0, ToPrice = 30, Size = 17, Margin = 45},
            new DiscountRule {MarginGarage = 12,  FromPrice = 30, ToPrice = 45, Size = 17, Margin = 48},
            new DiscountRule {MarginGarage = 15,  FromPrice = 45, ToPrice = 60, Size = 17, Margin = 51},
            new DiscountRule {MarginGarage = 15,  FromPrice = 60, ToPrice = 75, Size = 17, Margin = 54},
            new DiscountRule {MarginGarage = 15,  FromPrice = 75, ToPrice = 90, Size = 17, Margin = 57},
            new DiscountRule {MarginGarage = 17,  FromPrice = 90, ToPrice = 105, Size = 17, Margin = 60},
            new DiscountRule {MarginGarage = 17,  FromPrice = 105, ToPrice = 120, Size = 17, Margin = 63},
            new DiscountRule {MarginGarage = 17,  FromPrice = 120, ToPrice = 135, Size = 17, Margin = 66},
            new DiscountRule {MarginGarage = 18,  FromPrice = 135, ToPrice = 150, Size = 17, Margin = 69},
            new DiscountRule {MarginGarage = 20,  FromPrice = 150, ToPrice = 165, Size = 17, Margin = 71},
            new DiscountRule {MarginGarage = 20,  FromPrice = 165, ToPrice = 180, Size = 17, Margin = 73},
            new DiscountRule {MarginGarage = 20,  FromPrice = 180, ToPrice = 195, Size = 17, Margin = 75},
            new DiscountRule {MarginGarage = 20,  FromPrice = 195, ToPrice = 210, Size = 17, Margin = 77},
            new DiscountRule {MarginGarage = 25,  FromPrice = 210, ToPrice = 225, Size = 17, Margin = 78},
            new DiscountRule {MarginGarage = 25,  FromPrice = 225, ToPrice = 240, Size = 17, Margin = 79},
            new DiscountRule {MarginGarage = 25,  FromPrice = 240, ToPrice = 255, Size = 17, Margin = 80},
            new DiscountRule {MarginGarage = 25,  FromPrice = 255, ToPrice = 270, Size = 17, Margin = 81},
            new DiscountRule {MarginGarage = 25,  FromPrice = 270, ToPrice = 285, Size = 17, Margin = 83},
            new DiscountRule {MarginGarage = 30,  FromPrice = 285, ToPrice = 300, Size = 17, Margin = 85},
            new DiscountRule {MarginGarage = 30,  FromPrice = 300, ToPrice = 350, Size = 17, Margin = 88},
            new DiscountRule {MarginGarage = 30,  FromPrice = 350, ToPrice = 400, Size = 17, Margin = 90},
            new DiscountRule {MarginGarage = 30,  FromPrice = 400, ToPrice = 99999, Size = 17, Margin = 90},
            new DiscountRule {MarginGarage = 15,  FromPrice = 0, ToPrice = 30, Size = 18, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 30, ToPrice = 45, Size = 18, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 45, ToPrice = 60, Size = 18, Margin = 65},
            new DiscountRule {MarginGarage = 20,  FromPrice = 60, ToPrice = 75, Size = 18, Margin = 65},
            new DiscountRule {MarginGarage = 20,  FromPrice = 75, ToPrice = 90, Size = 18, Margin = 65},
            new DiscountRule {MarginGarage = 25,  FromPrice = 90, ToPrice = 105, Size = 18, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 105, ToPrice = 120, Size = 18, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 120, ToPrice = 135, Size = 18, Margin = 75},
            new DiscountRule {MarginGarage = 25,  FromPrice = 135, ToPrice = 150, Size = 18, Margin = 75},
            new DiscountRule {MarginGarage = 28,  FromPrice = 150, ToPrice = 165, Size = 18, Margin = 80},
            new DiscountRule {MarginGarage = 28,  FromPrice = 165, ToPrice = 180, Size = 18, Margin = 85},
            new DiscountRule {MarginGarage = 30,  FromPrice = 180, ToPrice = 195, Size = 18, Margin = 90},
            new DiscountRule {MarginGarage = 30,  FromPrice = 195, ToPrice = 210, Size = 18, Margin = 90},
            new DiscountRule {MarginGarage = 30,  FromPrice = 210, ToPrice = 225, Size = 18, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 225, ToPrice = 240, Size = 18, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 240, ToPrice = 255, Size = 18, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 255, ToPrice = 270, Size = 18, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 270, ToPrice = 285, Size = 18, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 285, ToPrice = 300, Size = 18, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 300, ToPrice = 315, Size = 18, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 315, ToPrice = 330, Size = 18, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 330, ToPrice = 345, Size = 18, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 345, ToPrice = 360, Size = 18, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 360, ToPrice = 99999, Size = 18, Margin = 105},
            new DiscountRule {MarginGarage = 15,  FromPrice = 0, ToPrice = 30, Size = 19, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 30, ToPrice = 45, Size = 19, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 45, ToPrice = 60, Size = 19, Margin = 60},
            new DiscountRule {MarginGarage = 15,  FromPrice = 60, ToPrice = 75, Size = 19, Margin = 60},
            new DiscountRule {MarginGarage = 17,  FromPrice = 75, ToPrice = 90, Size = 19, Margin = 65},
            new DiscountRule {MarginGarage = 20,  FromPrice = 90, ToPrice = 105, Size = 19, Margin = 70},
            new DiscountRule {MarginGarage = 20,  FromPrice = 105, ToPrice = 120, Size = 19, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 120, ToPrice = 135, Size = 19, Margin = 75},
            new DiscountRule {MarginGarage = 25,  FromPrice = 135, ToPrice = 150, Size = 19, Margin = 80},
            new DiscountRule {MarginGarage = 30,  FromPrice = 150, ToPrice = 165, Size = 19, Margin = 80},
            new DiscountRule {MarginGarage = 30,  FromPrice = 165, ToPrice = 180, Size = 19, Margin = 80},
            new DiscountRule {MarginGarage = 32,  FromPrice = 180, ToPrice = 195, Size = 19, Margin = 85},
            new DiscountRule {MarginGarage = 32,  FromPrice = 195, ToPrice = 210, Size = 19, Margin = 85},
            new DiscountRule {MarginGarage = 35,  FromPrice = 210, ToPrice = 225, Size = 19, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 225, ToPrice = 240, Size = 19, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 240, ToPrice = 255, Size = 19, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 255, ToPrice = 270, Size = 19, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 270, ToPrice = 285, Size = 19, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 285, ToPrice = 300, Size = 19, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 300, ToPrice = 315, Size = 19, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 315, ToPrice = 330, Size = 19, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 330, ToPrice = 345, Size = 19, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 345, ToPrice = 360, Size = 19, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 360, ToPrice = 99999, Size = 19, Margin = 105},
            new DiscountRule {MarginGarage = 15,  FromPrice = 0, ToPrice = 30, Size = 20, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 30, ToPrice = 45, Size = 20, Margin = 55},
            new DiscountRule {MarginGarage = 20,  FromPrice = 45, ToPrice = 60, Size = 20, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 60, ToPrice = 75, Size = 20, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 75, ToPrice = 90, Size = 20, Margin = 65},
            new DiscountRule {MarginGarage = 25,  FromPrice = 90, ToPrice = 105, Size = 20, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 105, ToPrice = 120, Size = 20, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 120, ToPrice = 135, Size = 20, Margin = 75},
            new DiscountRule {MarginGarage = 35,  FromPrice = 135, ToPrice = 150, Size = 20, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 150, ToPrice = 165, Size = 20, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 165, ToPrice = 180, Size = 20, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 180, ToPrice = 195, Size = 20, Margin = 85},
            new DiscountRule {MarginGarage = 35,  FromPrice = 195, ToPrice = 210, Size = 20, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 210, ToPrice = 225, Size = 20, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 225, ToPrice = 240, Size = 20, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 240, ToPrice = 255, Size = 20, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 255, ToPrice = 270, Size = 20, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 270, ToPrice = 285, Size = 20, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 285, ToPrice = 300, Size = 20, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 300, ToPrice = 315, Size = 20, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 315, ToPrice = 330, Size = 20, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 330, ToPrice = 345, Size = 20, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 345, ToPrice = 360, Size = 20, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 360, ToPrice = 375, Size = 20, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 375, ToPrice = 390, Size = 20, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 390, ToPrice = 99999, Size = 20, Margin = 110},
            new DiscountRule {MarginGarage = 15,  FromPrice = 0, ToPrice = 30, Size = 21, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 30, ToPrice = 45, Size = 21, Margin = 55},
            new DiscountRule {MarginGarage = 20,  FromPrice = 45, ToPrice = 60, Size = 21, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 60, ToPrice = 75, Size = 21, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 75, ToPrice = 90, Size = 21, Margin = 65},
            new DiscountRule {MarginGarage = 25,  FromPrice = 90, ToPrice = 105, Size = 21, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 105, ToPrice = 120, Size = 21, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 120, ToPrice = 135, Size = 21, Margin = 75},
            new DiscountRule {MarginGarage = 35,  FromPrice = 135, ToPrice = 150, Size = 21, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 150, ToPrice = 165, Size = 21, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 165, ToPrice = 180, Size = 21, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 180, ToPrice = 195, Size = 21, Margin = 85},
            new DiscountRule {MarginGarage = 35,  FromPrice = 195, ToPrice = 210, Size = 21, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 210, ToPrice = 225, Size = 21, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 225, ToPrice = 240, Size = 21, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 240, ToPrice = 255, Size = 21, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 255, ToPrice = 270, Size = 21, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 270, ToPrice = 285, Size = 21, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 285, ToPrice = 300, Size = 21, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 300, ToPrice = 315, Size = 21, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 315, ToPrice = 330, Size = 21, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 330, ToPrice = 345, Size = 21, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 345, ToPrice = 360, Size = 21, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 360, ToPrice = 375, Size = 21, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 375, ToPrice = 390, Size = 21, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 390, ToPrice = 99999, Size = 21, Margin = 110},
            new DiscountRule {MarginGarage = 15,  FromPrice = 0, ToPrice = 30, Size = 22, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 30, ToPrice = 45, Size = 22, Margin = 55},
            new DiscountRule {MarginGarage = 20,  FromPrice = 45, ToPrice = 60, Size = 22, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 60, ToPrice = 75, Size = 22, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 75, ToPrice = 90, Size = 22, Margin = 65},
            new DiscountRule {MarginGarage = 25,  FromPrice = 90, ToPrice = 105, Size = 22, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 105, ToPrice = 120, Size = 22, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 120, ToPrice = 135, Size = 22, Margin = 75},
            new DiscountRule {MarginGarage = 35,  FromPrice = 135, ToPrice = 150, Size = 22, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 150, ToPrice = 165, Size = 22, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 165, ToPrice = 180, Size = 22, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 180, ToPrice = 195, Size = 22, Margin = 85},
            new DiscountRule {MarginGarage = 35,  FromPrice = 195, ToPrice = 210, Size = 22, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 210, ToPrice = 225, Size = 22, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 225, ToPrice = 240, Size = 22, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 240, ToPrice = 255, Size = 22, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 255, ToPrice = 270, Size = 22, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 270, ToPrice = 285, Size = 22, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 285, ToPrice = 300, Size = 22, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 300, ToPrice = 315, Size = 22, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 315, ToPrice = 330, Size = 22, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 330, ToPrice = 345, Size = 22, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 345, ToPrice = 360, Size = 22, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 360, ToPrice = 375, Size = 22, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 375, ToPrice = 390, Size = 22, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 390, ToPrice = 99999, Size = 22, Margin = 110},
            new DiscountRule {MarginGarage = 15,  FromPrice = 0, ToPrice = 30, Size = 23, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 30, ToPrice = 45, Size = 23, Margin = 55},
            new DiscountRule {MarginGarage = 20,  FromPrice = 45, ToPrice = 60, Size = 23, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 60, ToPrice = 75, Size = 23, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 75, ToPrice = 90, Size = 23, Margin = 65},
            new DiscountRule {MarginGarage = 25,  FromPrice = 90, ToPrice = 105, Size = 23, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 105, ToPrice = 120, Size = 23, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 120, ToPrice = 135, Size = 23, Margin = 75},
            new DiscountRule {MarginGarage = 35,  FromPrice = 135, ToPrice = 150, Size = 23, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 150, ToPrice = 165, Size = 23, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 165, ToPrice = 180, Size = 23, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 180, ToPrice = 195, Size = 23, Margin = 85},
            new DiscountRule {MarginGarage = 35,  FromPrice = 195, ToPrice = 210, Size = 23, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 210, ToPrice = 225, Size = 23, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 225, ToPrice = 240, Size = 23, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 240, ToPrice = 255, Size = 23, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 255, ToPrice = 270, Size = 23, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 270, ToPrice = 285, Size = 23, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 285, ToPrice = 300, Size = 23, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 300, ToPrice = 315, Size = 23, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 315, ToPrice = 330, Size = 23, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 330, ToPrice = 345, Size = 23, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 345, ToPrice = 360, Size = 23, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 360, ToPrice = 375, Size = 23, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 375, ToPrice = 390, Size = 23, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 390, ToPrice = 99999, Size = 23, Margin = 110},
            new DiscountRule {MarginGarage = 15,  FromPrice = 0, ToPrice = 30, Size = 24, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 30, ToPrice = 45, Size = 24, Margin = 55},
            new DiscountRule {MarginGarage = 20,  FromPrice = 45, ToPrice = 60, Size = 24, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 60, ToPrice = 75, Size = 24, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 75, ToPrice = 90, Size = 24, Margin = 65},
            new DiscountRule {MarginGarage = 25,  FromPrice = 90, ToPrice = 105, Size = 24, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 105, ToPrice = 120, Size = 24, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 120, ToPrice = 135, Size = 24, Margin = 75},
            new DiscountRule {MarginGarage = 35,  FromPrice = 135, ToPrice = 150, Size = 24, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 150, ToPrice = 165, Size = 24, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 165, ToPrice = 180, Size = 24, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 180, ToPrice = 195, Size = 24, Margin = 85},
            new DiscountRule {MarginGarage = 35,  FromPrice = 195, ToPrice = 210, Size = 24, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 210, ToPrice = 225, Size = 24, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 225, ToPrice = 240, Size = 24, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 240, ToPrice = 255, Size = 24, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 255, ToPrice = 270, Size = 24, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 270, ToPrice = 285, Size = 24, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 285, ToPrice = 300, Size = 24, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 300, ToPrice = 315, Size = 24, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 315, ToPrice = 330, Size = 24, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 330, ToPrice = 345, Size = 24, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 345, ToPrice = 360, Size = 24, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 360, ToPrice = 375, Size = 24, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 375, ToPrice = 390, Size = 24, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 390, ToPrice = 99999, Size = 24, Margin = 110},
            new DiscountRule {MarginGarage = 15,  FromPrice = 0, ToPrice = 30, Size = 25, Margin = 55},
            new DiscountRule {MarginGarage = 15,  FromPrice = 30, ToPrice = 45, Size = 25, Margin = 55},
            new DiscountRule {MarginGarage = 20,  FromPrice = 45, ToPrice = 60, Size = 25, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 60, ToPrice = 75, Size = 25, Margin = 60},
            new DiscountRule {MarginGarage = 20,  FromPrice = 75, ToPrice = 90, Size = 25, Margin = 65},
            new DiscountRule {MarginGarage = 25,  FromPrice = 90, ToPrice = 105, Size = 25, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 105, ToPrice = 120, Size = 25, Margin = 70},
            new DiscountRule {MarginGarage = 25,  FromPrice = 120, ToPrice = 135, Size = 25, Margin = 75},
            new DiscountRule {MarginGarage = 35,  FromPrice = 135, ToPrice = 150, Size = 25, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 150, ToPrice = 165, Size = 25, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 165, ToPrice = 180, Size = 25, Margin = 80},
            new DiscountRule {MarginGarage = 35,  FromPrice = 180, ToPrice = 195, Size = 25, Margin = 85},
            new DiscountRule {MarginGarage = 35,  FromPrice = 195, ToPrice = 210, Size = 25, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 210, ToPrice = 225, Size = 25, Margin = 90},
            new DiscountRule {MarginGarage = 35,  FromPrice = 225, ToPrice = 240, Size = 25, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 240, ToPrice = 255, Size = 25, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 255, ToPrice = 270, Size = 25, Margin = 95},
            new DiscountRule {MarginGarage = 35,  FromPrice = 270, ToPrice = 285, Size = 25, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 285, ToPrice = 300, Size = 25, Margin = 100},
            new DiscountRule {MarginGarage = 35,  FromPrice = 300, ToPrice = 315, Size = 25, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 315, ToPrice = 330, Size = 25, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 330, ToPrice = 345, Size = 25, Margin = 105},
            new DiscountRule {MarginGarage = 35,  FromPrice = 345, ToPrice = 360, Size = 25, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 360, ToPrice = 375, Size = 25, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 375, ToPrice = 390, Size = 25, Margin = 110},
            new DiscountRule {MarginGarage = 35,  FromPrice = 390, ToPrice = 99999, Size = 25, Margin = 110}
        };
    }
}