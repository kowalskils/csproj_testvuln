﻿using NJsonSchema;
using NJsonSchema.NewtonsoftJson.Generation;
using NJsonSchema.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cve_2024_21409_repro
{
    internal class Program
    {
        private class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime Birthday { get; set; }
        }

        private class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }

        private static bool Validate<T>(string json)
        {
            var settings = new NewtonsoftJsonSchemaGeneratorSettings
            {
                FlattenInheritanceHierarchy = true,
                AlwaysAllowAdditionalObjectProperties = false,
            };

            var schema = JsonSchema.FromType<T>(settings);
            ICollection<ValidationError> errors = schema.Validate(json);
            foreach (ValidationError error in errors)
            {
                Console.WriteLine(error);
            }

            return !errors.Any();
        }

        private static void Main(string[] args)
        {
            var personDataSet = new[]
            {
                "{\"FirstName\":\"John\",\"LastName\":\"Smith\",\"Birthday\":\"1960-11-20\"}",
                "{\"FirstName\":\"Mary\",\"LastName\":\"Popys\",\"Birthday\":\"2000-01-01\",\"InvalidField\":\"true\"}",
            };

            foreach (var data in personDataSet)
            {
                Console.WriteLine($"{data}");
                Console.WriteLine(Validate<Person>(data) ? "VALID" : "INVALID");
                Console.WriteLine();
            }

            var productDataSet = new[]
            {
                "{\"Id\":1,\"Name\":\"Pastry-FrenchMiniAssorted\",\"Description\":\"BagelsPoppyseed\",\"Price\":5.00}",
                "{\"Id\":2,\"Name\":\"Cheese-Havarti,Salsa\",\"Description\":\"Pasta-PennePrimavera,Single\",\"Price\":\"invalid\"}",
            };

            foreach (var data in productDataSet)
            {
                Console.WriteLine($"{data}");
                Console.WriteLine(Validate<Product>(data) ? "VALID" : "INVALID");
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to finish...");
            Console.ReadKey();
        }
    }
}