using Geolocation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;

namespace GeolocationNetStandard.UnitTests
{
    [TestFixture]
    public class ExpressionExtensionsTests
    {
        private TestDbContext _dbcontext;

        [OneTimeSetUp]
        public void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connection)
                //.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=TestingGeolocationExpressions")
                .Options;
            _dbcontext = new TestDbContext(options);

            _dbcontext.Database.EnsureDeleted();
            _dbcontext.Database.EnsureCreated();
            _dbcontext.GeoLocations.Add(new GeoLocation { Latitude = 12.2, Longitude = 33.3 });
            _dbcontext.SaveChanges();
        }

        [Test]
        public void ShouldCalculateRadiansInSql()
        {
            var query = _dbcontext.GeoLocations
                .Select(ExpressionHelperExtensionMethods.AsExpressionOfFunc<GeoLocation, double>(ExpressionExtensionMethods.ToRadiansExpression, x => x.Latitude));
            Console.WriteLine("queryWithExpression=" + query.ToSql());
            Assert.IsTrue(query.ToSql().Contains("*"), "Calculation should be done in SQL query.");
            Assert.AreEqual(0.2129301687433082d, query.First());
        }

        [Test]
        public void ShouldCalculateRadiansInMemory()
        {
            var query = _dbcontext.GeoLocations
                .Select(x => ExpressionExtensionMethods.ToRadiansFunc(x.Latitude));
            Console.WriteLine("queryWithExpression=" + query.ToSql());
            Assert.IsFalse(query.ToSql().Contains("*"), "Calculation should be done in memory.");
            Assert.AreEqual(0.2129301687433082d, query.First());

        }

        [Test]
        public void ShouldCalculateDistanceInSql()
        {

            var query = _dbcontext.GeoLocations
                .CalculateDistanceInDatabase(x => x.CalculatedDistanceFromOrigin, 10.1, 20.2)
                //.CalculateDistanceInDatabase(x => x.CalculatedDistanceFromOrigin, 10.1, 20.2, x => x.Latitude, x => x.Longitude)
                .Take(10)
                ;
            Console.WriteLine("queryWithExpression=" + query.ToSql());
            Assert.IsTrue(query.ToSql().Contains("/ 2"), "Calculation should be done in SQL query.");

            if (_dbcontext.Database.IsSqlServer())
            {
                // SQLServer supports full distance calculation so query runs completely in SQL
                Assert.IsTrue(query.ToSql().Contains("ASIN"), "Calculation (including all Math functions) should be done in SQL query.");
                Assert.IsTrue(query.ToSql().Contains("TOP(10)"), "Calculation (including all Math functions) should be done in SQL query.");
            }

            // SQLite doesn't support trig functions
            Assert.AreEqual(899.74211318552204d, query.First().CalculatedDistanceFromOrigin);
        }
        [Test]
        public void ShouldCalculateDistanceInKilometers()
        {
            var query = _dbcontext.GeoLocations
                .CalculateDistanceInDatabase(x => x.CalculatedDistanceFromOrigin, 10.1, 20.2, distanceUnit: DistanceUnit.Kilometers)
                .Take(10)
                ;

            // SQLite doesn't support trig functions
            Assert.AreEqual(1447.9052798951657d, query.First().CalculatedDistanceFromOrigin);
        }
    }


    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options) {}

        public DbSet<GeoLocation> GeoLocations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeoLocation>()
                .Property(x => x.CalculatedDistanceFromOrigin)
                .HasDefaultValue(-1);
        }
    }
    public class GeoLocation
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double CalculatedDistanceFromOrigin { get; set; }
    }

    public static class IQueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryModelGenerator");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Microsoft.EntityFrameworkCore.Storage.Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) //where TEntity : class
        {
            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = modelGenerator.ParseQuery(query.Expression);
            var database = (IDatabase)DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }
    }

}
