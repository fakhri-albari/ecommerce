using API.DTO.Seller;
using DAL.Models;
using Moq;
using Nexus.Base.CosmosDBRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace BLL.Test
{
    public class ProductServicesTest
    {
        public class CreateProduct
        {
            public static List<object[]> GetProductData => new List<object[]>
            {
                new Object[]
                {
                    new Product
                    {
                        name = "S22",
                        price = 10000000,
                        stock = 10,
                        storeId = "SAM-12345",
                        store = new Store
                        {
                            name = "Samsung Official",
                            information = "Awesome"
                        }
                    }
                }
            };

            [Theory]
            [MemberData(nameof(GetProductData))]
            public async Task CreateProduct_Succeed(Product product)
            {
                List<Product> products = new List<Product>();
                var mockRepo = new Mock<IDocumentDBRepository<Product>>();
                mockRepo.Setup(c => c.CreateAsync(
                    It.IsAny<Product>(),
                    It.IsAny<EventGridOptions>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync((Product p, EventGridOptions evg, string str1, string str2) => p)
                .Callback((Product p, EventGridOptions evg, string createdBy, string activeFlag) =>
                {
                    products.Add(p);
                });

                var svc = new ProductServices(mockRepo.Object);
                var result = await svc.CreateProduct(product);

                Assert.NotNull(result);
                Assert.NotEmpty(products);
                Assert.Equal(product.name, products[0].name);
                Assert.Equal(product.price, products[0].price);
                Assert.Equal(product.stock, products[0].stock);
                Assert.Equal(product.store, products[0].store);
            }
        }

        public class UpdateProduct
        {
            public static List<object[]> GetProductData => new List<object[]>
            {
                new Object[]
                {
                    new Product
                    {
                        Id = "sam-12345-00001",
                        name = "Galaxy S22",
                        price = 10000000,
                        stock = 100,
                        store = new Store
                        {
                            name = "Samsung Official Store",
                            information = "Gadget Store"
                        }
                    },
                    new Product
                    {
                        Id = "sam-12345-00001",
                        name = "Galaxy S22 Ultra",
                        price = 15000000,
                        stock = 10,
                        store = new Store
                        {
                            name = "Samsung Official Store",
                            information = "Gadget Store"
                        }
                    },
                }
            };

            [Theory]
            [MemberData(nameof(GetProductData))]
            public async Task UpdateProduct_Succeed(Product oldData, Product updatedData)
            {

                var mockRepo = new Mock<IDocumentDBRepository<Product>>();

                mockRepo.Setup(c => c.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()))
                    .Returns(Task.FromResult(oldData));

                mockRepo.Setup(c => c.UpdateAsync(
                   It.IsAny<string>(),
                   It.IsAny<Product>(),
                   It.IsAny<EventGridOptions>(),
                   It.IsAny<string>(),
                   It.IsAny<bool>()))
                    .ReturnsAsync((string id, Product item, EventGridOptions options, string lastUpdatedBy, bool isOptimisticConcurrency) => item);

                var svc = new ProductServices(mockRepo.Object);
                var result = await svc.UpdateProduct(updatedData);

                Assert.Equal(result.name, updatedData.name);
                Assert.Equal(result.stock, updatedData.stock);
                Assert.Equal(result.price, updatedData.price);
            }

        }

        
    }

    public class DeleteProduct
    {
        public static List<object[]> GetProductData => new List<object[]>
            {
                new Object[]
                {
                    new Product
                    {
                        name = "S22",
                        price = 10000000,
                        stock = 10,
                        storeId = "SAM-12345",
                        store = new Store
                        {
                            name = "Samsung Official",
                            information = "Awesome"
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(GetProductData))]
        public async Task DeleteProduct_Succeed(Product product)
        {
            List<Product> products = new List<Product>();
            products.Add(product);
            var mockRepo = new Mock<IDocumentDBRepository<Product>>();
            mockRepo.Setup(c => c.DeleteAsync(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<EventGridOptions>()))
            .Callback((string sid, Dictionary<string, string> partitionKey, EventGridOptions evg) =>
            {
                products = products.Where(x => x.Id != sid && x.PartitionKey != partitionKey["storeId"]).ToList();
            });

            var svc = new ProductServices(mockRepo.Object);
            await svc.DeleteProduct(product);

            Assert.Empty(products);
        }
    }
}
