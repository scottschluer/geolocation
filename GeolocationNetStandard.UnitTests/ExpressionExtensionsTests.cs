using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
            var query = _dbcontext.GeoLocations.Select(ExpressionHelperExtensionMethods.AsExpressionOfFunc<GeoLocation, double>(ExpressionExtensionMethods.ToRadiansExpression, x => x.Latitude));
            Console.WriteLine("queryWithExpression=" + query.ToSql());
            Assert.IsTrue(query.ToSql().Contains("*"), "Calculation should be done in SQL query.");
            Assert.AreEqual(0.2129301687433082d, query.First());
        }

        [Test]
        public void ShouldCalculateRadiansInMemory()
        {
            var query = _dbcontext.GeoLocations.Select(x => ExpressionExtensionMethods.ToRadiansFunc(x.Latitude));
            Console.WriteLine("queryWithExpression=" + query.ToSql());
            Assert.IsFalse(query.ToSql().Contains("*"), "Calculation should be done in memory.");
            Assert.AreEqual(0.2129301687433082d, query.First());

        }
    }


    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options) {}

        public DbSet<GeoLocation> GeoLocations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeoLocation>()
                .Property(x => x.LatitudeRadians)
                .HasComputedColumnSql("-1.0");
        }
    }
    public class GeoLocation
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double? LatitudeRadians { get; set; }
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
