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

                        Lenght = 6,

                        Width = 21,

                        Thickness = 2.75,

                        Volume = 38.8,

                        Type = "Shortboard",

                        Price = 565M,

                        Equipment = "",

                    },

                    new Board

                    {

                        BoardName = "The Wide Glider",

                        Lenght = 7.1,

                        Width = 21.75,

                        Thickness = 2.75,

                        Volume = 44.16,

                        Type = "Funboard",

                        Price = 685M,

                        Equipment = "",

                    },

                    new Board

                    {

                        BoardName = "The Golden Ratio",

                        Lenght = 6.3,

                        Width = 21.85,

                        Thickness = 2.9,

                        Volume = 43.22,

                        Type = "Funboard",

                        Price = 695M,

                        Equipment = "",

                    },

                    new Board

                    {

                        BoardName = "Mahi Mahi",

                        Lenght = 5.4,

                        Width = 20.75,

                        Thickness = 2.3,

                        Volume = 29.39,

                        Type = "Fish",

                        Price = 645M,

                        Equipment = "",

                    },

                    new Board

                    {

                        BoardName = "The Emerald Glider",

                        Lenght = 9.2,

                        Width = 22.8,

                        Thickness = 2.8,

                        Volume = 65.4,

                        Type = "Longboard",

                        Price = 895M,

                        Equipment = "",

                    },


					new Board

					{

						BoardName = "The Bomb",

						Lenght = 5.5,

						Width = 21,

						Thickness = 2.5,

						Volume = 33.7,

						Type = "Shortboard",

						Price = 645M,

						Equipment = "",

					},

					new Board

					{

						BoardName = "Walden Magic",

						Lenght = 9.6,

						Width = 19.4,

						Thickness = 3,

						Volume = 80,

						Type = "Longboard",

						Price = 1025M,

						Equipment = "",

					},

					new Board

					{

						BoardName = "Naish One",

						Lenght = 12.6,

						Width = 30,

						Thickness = 6,

						Volume = 301,

						Type = "SUP",

						Price = 854M,

						Equipment = "Paddle",

					},

					new Board

					{

						BoardName = "Six Tourer",

						Lenght = 11.6,

						Width = 32,

						Thickness = 6,

						Volume = 270,

						Type = "SUP",

						Price = 611M,

						Equipment = "Fin, Paddle, Pump, Leash",

					},

					new Board

					{

						BoardName = "Naish Maliko",

						Lenght = 14,

						Width = 25,

						Thickness = 6,

						Volume = 330,

						Type = "SUP",

						Price = 1304M,

						Equipment = "Fin, Paddle, Pump, Leash",

					}



				);

                context.SaveChanges();
            }
        }

    }

}