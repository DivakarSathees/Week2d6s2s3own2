using System;
using System.Reflection;
using NUnit.Framework;
using dotnetapp.Models;
using System.ComponentModel.DataAnnotations;
using dotnetapp.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace dotnetapp.Tests
{
    [TestFixture]
    public class PostTests
    {
        // private const string ViewsFolderPath = "Views";
        // private const string PostViewsFolderPath = "D:\\Visual Studio\\W5_D1_S1_Client\\dotnetapp\\dotnetapp\\Views\\Post";
        private Type _productType;
        private Type controllerType;
        private Type _viewType;
        private Assembly _assembly1;
        private string relativeFolderPath; // Set this to the relative path of the folder you want to check
        private string fileName; 
        private Mock<AppDbContext> _mockContext;
        private AppDbContext _context;
        private LibraryController _libraryController;
        private DbContextOptions<AppDbContext> _dbContextOptions;


        // private PostController _postcontroller;
        // private List<Post> _fakePosts;


        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryTestDatabase")
                .Options;

            var dbContext = new AppDbContext(_dbContextOptions);
            _libraryController = new LibraryController(dbContext);
           
        }

        [TearDown]
        public void TearDown()
        {
            //_postcontroller = null;
        }

        private static MethodInfo GetMethod(Type type, string methodName, Type[] parameterTypes)
        {
            return type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
        }
        


        [Test]
        public void Session_2_TestBook_ClassExists()
        {
            // Load the assembly at runtime
            Assembly assembly = Assembly.Load("dotnetapp");
            Type postType = assembly.GetType("dotnetapp.Models.Book");
            Assert.NotNull(postType, "Book class does not exist.");
        }
        [Test]
        public void Session_2_TestLibraryCard_ClassExists()
        {
            // Load the assembly at runtime
            Assembly assembly = Assembly.Load("dotnetapp");
            Type postType = assembly.GetType("dotnetapp.Models.LibraryCard");
            Assert.NotNull(postType, "LibraryCard class does not exist.");
        }
        [Test]
        public void Session_2_TestAppDbContext_ClassExists_in_Models()
        {
            // Load the assembly at runtime
            Assembly assembly = Assembly.Load("dotnetapp");
            Type postType = assembly.GetType("dotnetapp.Models.AppDbContext");
            Assert.NotNull(postType, "AppDbContext class does not exist.");
        }
        // [Test]
        // public void Session_2_TestCustomer_ClassExists()
        // {
        //     // Load the assembly at runtime
        //     Assembly assembly = Assembly.Load("dotnetapp");
        //     Type postType = assembly.GetType("dotnetapp.Models.Customer");
        //     Assert.NotNull(postType, "Customer class does not exist.");
        // }
        // [Test]
        // public void Session_2_TestOrderDetail_ClassExists()
        // {
        //     // Load the assembly at runtime
        //     Assembly assembly = Assembly.Load("dotnetapp");
        //     Type postType = assembly.GetType("dotnetapp.Models.OrderDetail");
        //     Assert.NotNull(postType, "OrderDetails class does not exist.");
        // }
        // [Test]
        // public void Session_2_TestOrdersDbContext_ClassExists()
        // {
        //     // Load the assembly at runtime
        //     Assembly assembly = Assembly.Load("dotnetapp");
        //     Type postType = assembly.GetType("dotnetapp.Models.OrdersDbContext");
        //     Assert.NotNull(postType, "OrdersDbContext class does not exist.");
        // }

        [Test]
        public void Session_2_TestTitlePropertyType()
        {
            Assembly assembly = Assembly.Load("dotnetapp");
            _productType = assembly.GetType("dotnetapp.Models.Book");
            PropertyInfo UnitPriceProperty = _productType.GetProperty("Title");
            Assert.NotNull(UnitPriceProperty, "Title property does not exist.");
            Assert.AreEqual(typeof(string), UnitPriceProperty.PropertyType, "Title property should be of type String.");
        }

        [Test]
        public void Session_2_TestTitlePropertyMaxLength100()
        {
            Assembly assembly = Assembly.Load("dotnetapp");
            _productType = assembly.GetType("dotnetapp.Models.Book");
            PropertyInfo titleProperty = _productType.GetProperty("Title");
            var maxLengthAttribute = titleProperty.GetCustomAttribute<MaxLengthAttribute>();
            
            Assert.NotNull(maxLengthAttribute, "MaxLength attribute not found on Title property.");
            Assert.AreEqual(100, maxLengthAttribute.Length, "Title property should have a max length of 100.");
        }

        [Test]
        public void Session_2_TestAuthorPropertyMaxLength50()
        {            
            Assembly assembly = Assembly.Load("dotnetapp");
            _productType = assembly.GetType("dotnetapp.Models.Book");
            PropertyInfo titleProperty = _productType.GetProperty("Author");
            var maxLengthAttribute = titleProperty.GetCustomAttribute<MaxLengthAttribute>();
            
            Assert.NotNull(maxLengthAttribute, "MaxLength attribute not found on Author property.");
            Assert.AreEqual(50, maxLengthAttribute.Length, "Author property should have a max length of 50.");
        }

        [Test]
        public void Session_2_TestPublishedYearPropertyRange()
        {
            Assembly assembly = Assembly.Load("dotnetapp");
            _productType = assembly.GetType("dotnetapp.Models.Book");
            PropertyInfo publishedYearProperty = _productType.GetProperty("PublishedYear");
            var rangeAttribute = publishedYearProperty.GetCustomAttribute<System.ComponentModel.DataAnnotations.RangeAttribute>();
            
            Assert.NotNull(rangeAttribute, "Range attribute not found on PublishedYear property.");
            Assert.AreEqual(0, rangeAttribute.Minimum, "PublishedYear property should have a minimum value of 0.");
            Assert.AreEqual(int.MaxValue, rangeAttribute.Maximum, "PublishedYear property should have a maximum value of int.MaxValue.");
        }

        [Test]
        public void Session_2_TestCardNumberPropertyRegularExpressionAttribute()
        {
            Assembly assembly = Assembly.Load("dotnetapp");
            _productType = assembly.GetType("dotnetapp.Models.LibraryCard");            
            PropertyInfo cardNumberProperty = _productType.GetProperty("CardNumber");
            var regexAttribute = cardNumberProperty.GetCustomAttribute<RegularExpressionAttribute>();

            Assert.NotNull(regexAttribute, "RegularExpression attribute not found on CardNumber property.");
            Assert.AreEqual(@"LC-\d{5}", regexAttribute.Pattern, "CardNumber property should have the correct regular expression pattern.");
        }

        [Test]
        public void Session_2_TestMemberNamePropertyMaxLength100()
        {
            Assembly assembly = Assembly.Load("dotnetapp");
            _productType = assembly.GetType("dotnetapp.Models.LibraryCard");
            PropertyInfo titleProperty = _productType.GetProperty("MemberName");
            var maxLengthAttribute = titleProperty.GetCustomAttribute<MaxLengthAttribute>();
            
            Assert.NotNull(maxLengthAttribute, "MaxLength attribute not found on MemberName property.");
            Assert.AreEqual(100, maxLengthAttribute.Length, "MemberName property should have a max length of 100.");
        }

        // [Test]
        // public void Session_2_TestDiscountPropertyType_OrderDetail_table()
        // {
        //     Assembly assembly = Assembly.Load("dotnetapp");
        //     _productType = assembly.GetType("dotnetapp.Models.OrderDetail");
        //     PropertyInfo DiscountProperty = _productType.GetProperty("Discount");
        //     Assert.NotNull(DiscountProperty, "Discount property does not exist.");
        //     Assert.AreEqual(typeof(float), DiscountProperty.PropertyType, "Discount property should be of type float.");
        // }

        // [Test]
        // public void Session_2_TestPicturePropertyType_Category_Table()
        // {
        //     Assembly assembly = Assembly.Load("dotnetapp");
        //     _productType = assembly.GetType("dotnetapp.Models.Category");
        //     PropertyInfo PictureProperty = _productType.GetProperty("Picture");
        //     Assert.NotNull(PictureProperty, "Picture property does not exist.");
        //     Assert.AreEqual(typeof(byte[]), PictureProperty.PropertyType, "Picture property should be of type byte[].");
        // }

        [Test]
        public void Session_2_TestMigrationExists()
        {
            bool viewsFolderExists = Directory.Exists(@"/home/coder/project/workspace/Week2_day6_s2_3_own/dotnetapp/Migrations");

            Assert.IsTrue(viewsFolderExists, "Migrations does not exist.");
        }

        [Test]
        public void Session_3_Test_DisplayAllBooks_Action()
        {
            Assembly assembly = Assembly.Load("dotnetapp");
            controllerType = assembly.GetType("dotnetapp.Controllers.LibraryController");
            var detailsMethod = GetMethod(controllerType, "DisplayAllBooks", new Type[] {  });

            Assert.NotNull(detailsMethod);
        }

        [Test]
        public void Session_3_Test_SearchBooksByTitle_Action()
        {
            Assembly assembly = Assembly.Load("dotnetapp");
            Type controllerType = assembly.GetType("dotnetapp.Controllers.LibraryController");
            var searchBooksByTitleMethod = GetMethod(controllerType, "SearchBooksByTitle", new Type[] { typeof(string) });

            Assert.NotNull(searchBooksByTitleMethod);
        }

        [Test]
        public void Session_3_Test_DisplayBooksForLibraryCard_Action()
        {
            Assembly assembly = Assembly.Load("dotnetapp");
            Type controllerType = assembly.GetType("dotnetapp.Controllers.LibraryController");
            var searchBooksByTitleMethod = GetMethod(controllerType, "DisplayBooksForLibraryCard", new Type[] { typeof(int) });

            Assert.NotNull(searchBooksByTitleMethod);
        }

        [Test]
        public void Session_3_Test_GetAvailableBooks_Action()
        {
            Assembly assembly = Assembly.Load("dotnetapp");
            controllerType = assembly.GetType("dotnetapp.Controllers.LibraryController");
            var detailsMethod = GetMethod(controllerType, "GetAvailableBooks", new Type[] {  });

            Assert.NotNull(detailsMethod);
        }

        
       
    }
}
