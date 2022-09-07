using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SurfsUp.Data;
using SurfsUp.Models;
using System;
using System.Linq;

namespace SurfsUp.Models

{

    public static class SeedData

    {

        public static void Initialize(IServiceProvider serviceProvider)

        {

            using (var context = new SurfsUpContext(

            serviceProvider.GetRequiredService<

            DbContextOptions<SurfsUpContext>>()))

            {

                // Look for any boards.

                if (context.Board.Any())

                {

                    return; // DB has been seeded

                }

                context.Board.AddRange(

                    new Board

                    {

                        BoardName = "The Minilog",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/N/S/NSP04COFSSURVC604CS_78695.jpg",

                        Length = 6,

                        Width = 21,

                        Thickness = 2.75,

                        Volume = 38.8,

                        Type = "Shortboard",

                        Price = 565M,

                        Equipment = "",

                        Order = null
                        
                    },

                    new Board

                    {

                        BoardName = "The Wide Glider",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/S/U/SUP1-0HH400_66254.jpg",

                        Length = 7.1,

                        Width = 21.75,

                        Thickness = 2.75,

                        Volume = 44.16,

                        Type = "Funboard",

                        Price = 685M,

                        Equipment = "",

                        Order = null

                    },

                    new Board

                    {

                        BoardName = "The Golden Ratio",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/2/0/2005210101003_82988.jpg",

                        Length = 6.3,

                        Width = 21.85,

                        Thickness = 2.9,

                        Volume = 43.22,

                        Type = "Funboard",

                        Price = 695M,

                        Equipment = "",

                        Order = null

                    },

                    new Board

                    {

                        BoardName = "Mahi Mahi",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/H/S/HSFF-HG0600-FU1_112977.jpg",

                        Length = 5.4,

                        Width = 20.75,

                        Thickness = 2.3,

                        Volume = 29.39,

                        Type = "Fish",

                        Price = 645M,

                        Equipment = "",

                        Order = null

                    },

                    new Board

                    {

                        BoardName = "The Emerald Glider",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/N/U/NUU0607_77330.jpg",

                        Length = 9.2,

                        Width = 22.8,

                        Thickness = 2.8,

                        Volume = 65.4,

                        Type = "Longboard",

                        Price = 895M,

                        Equipment = "",

                        Order = null
                    },


					new Board

					{

						BoardName = "The Bomb",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/H/S/HSFF-HK0602-FCB_112990.jpg",

                        Length = 5.5,

						Width = 21,

						Thickness = 2.5,

						Volume = 33.7,

						Type = "Shortboard",

						Price = 645M,

						Equipment = "",

                        Order = null

                    },

					new Board

					{

						BoardName = "Walden Magic",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/N/U/NUU0606_77327.jpg",

                        Length = 9.6,

						Width = 19.4,

						Thickness = 3,

						Volume = 80,

						Type = "Longboard",

						Price = 1025M,

						Equipment = "",

                        Order = null

                    },

					new Board

					{

						BoardName = "Naish One",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/2/0/2020210401002_101445.jpg",

                        Length = 12.6,

						Width = 30,

						Thickness = 6,

						Volume = 301,

						Type = "SUP",

						Price = 854M,

						Equipment = "Paddle",

                        Order = null

                    },

					new Board

					{

						BoardName = "Six Tourer",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/2/0/2020210401001_101437.jpg",

                        Length = 11.6,

						Width = 32,

						Thickness = 6,

						Volume = 270,

						Type = "SUP",

						Price = 611M,

						Equipment = "Fin, Paddle, Pump, Leash",
                        
                        Order = null

                    },

					new Board

					{

						BoardName = "Naish Maliko",

                        Picture = "https://www.oneopensky.dk/img/310/300/resize/G/L/GLPRO126S-21_102222.jpg",

                        Length = 14,

						Width = 25,

						Thickness = 6,

						Volume = 330,

						Type = "SUP",

						Price = 1304M,

						Equipment = "Fin, Paddle, Pump, Leash",

                        Order = null

                    }



				);

                context.SaveChanges();
            }
        }

    }

}